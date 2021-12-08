using System;
using System.Linq;
using System.Threading.Tasks;
using Brenda.Contracts.V1.Campaign;
using Brenda.Contracts.V1.Responses;
using Brenda.Utils;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sara.Api.Exceptions;
using Sara.Contracts.Events;
using Sara.Contracts.Messages;
using Sara.Core;
using Sara.Data;

namespace Sara.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class CampaignController : ControllerBase
    {
        private readonly SaraContext _context;
        private readonly IBus _bus;
        private readonly UserManager<SaraUser> _userManager;

        public CampaignController(
            SaraContext context,
            IBus bus,
            UserManager<SaraUser> userManager)
        {
            _context = context;
            _bus = bus;
            _userManager = userManager;
        }

        [HttpPost("/campaign/{tag}")]
        public async Task<Models.CampaignRequested> Post(string tag, GetCampaign get)
        {
            if (!tag.Equals(get.Tag)) throw new ArgumentException("Requisição inválida.");

            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var campaignOnGoing = await _context.Campaigns.FirstOrDefaultAsync(x => x.Tag == get.Tag.Trim());
                var saraRequest = new CampaignRequest
                {
                    UserId = user.Id,
                    CustomerId = campaignOnGoing.CustomerId,
                    Latitude = get.Latitude,
                    Longitude = get.Longitude,
                    Acceleration = get.Acceleration,
                    Altitude = get.Altitude,
                    Speed = get.Speed,
                    SpeedAccuracy = get.SpeedAccuracy,
                    Heading = get.Healing,
                    DeviceId = get.DeviceId,
                    DeviceData = get.DeviceData,
                    RequestedAt = DateTime.UtcNow.ToTimestamp()
                };

                if (campaignOnGoing == null) throw new CampaignNotFoundException(saraRequest);

                saraRequest.CampaignId = campaignOnGoing.Id;

                var wasRewarded = await _context.Vouchers.AnyAsync(x => x.CampaignId == campaignOnGoing.Id && x.UserId == user.Id && x.WasRewarded);
                //if (wasRewarded) throw new CampaignRewardedException(saraRequest);

                var campaign = JsonConvert.DeserializeObject<CampaignOnGoing>(campaignOnGoing.Content);

                //if (campaign.IsOutOfDate()) throw new CampaignOutOfDateException(saraRequest);
                //if (!campaign.IsGeoAllowed(get.Latitude, get.Longitude)) throw new CampaignInvalidLocationException(saraRequest);

                var voucher = new Voucher(user, campaignOnGoing); // user.CreateVoucher(ongoing);
                var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == campaign.CustomerId);
                
                saraRequest.VoucherId = voucher.Id;

                await _context.AddAsync(voucher);
                await _context.SaveChangesAsync();
                
                await tx.CommitAsync();
                await _bus.Publish(saraRequest);

                return new Models.CampaignRequested(voucher, campaign, customer);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw ex;
            }
        }

        [HttpPut("/campaign/{campaignId}/voucher/{voucherId}/reward")]
        public async Task<IActionResult> PutReward(Guid campaignId, Guid voucherId, PutVoucher put)
        {
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                if (!campaignId.Equals(put.CampaignId) || !voucherId.Equals(put.VoucherId))
                    throw new ArgumentException(typeof(Voucher).FullName);

                var voucher = await _context.Vouchers
                    .Where(x => x.Id == put.VoucherId
                            && x.CampaignId == put.CampaignId
                            && !x.WasRewarded)
                    .SingleAsync();
                var user = await _userManager.GetUserAsync(User);
                var campaign = await _context.Campaigns.FirstAsync(x => x.Id == put.CampaignId);
                var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == campaign.CustomerId);
                var ongoing = JsonConvert.DeserializeObject<CampaignOnGoing>(campaign.Content);
                voucher.ConfirmReward();

                await _bus.Publish(
                    new Carol.Contracts.RewardUser(user.Id, customer.Name, campaign.ChallengeId ?? campaign.Id, put.VoucherId, ongoing.Reward));

                await _bus.Publish(new Sara.Contracts.Events.RewardUser(user.Id, voucher.Id, ongoing.Reward));

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                
                var message = new RewardMessage(customer.Name, customer.LogoAvatar, ongoing.Reward);
                return Ok(Responses.Data(message));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw ex;
            }
        }
    }

    public class PutVoucher
    {
        public Guid VoucherId { get; set; }
        public Guid CampaignId { get; set; }

        public string[] Replies { get; set; }
    }

    public class GetCampaign
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }

        [JsonProperty("acc")]
        public double Acceleration { get; set; }

        [JsonProperty("alt")]
        public double Altitude { get; set; }

        [JsonProperty("spe")]
        public double Speed { get; set; }

        [JsonProperty("spa")]
        public double SpeedAccuracy { get; set; }

        [JsonProperty("hea")]
        public double Healing { get; set; }

        [JsonProperty("did")]
        public string DeviceId { get; set; }

        [JsonProperty("dev")]
        public string DeviceData { get; set; }
    }
}
