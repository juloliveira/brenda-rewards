using AutoMapper;
using Brenda.Contracts.V1.Campaign;
using Brenda.Core;
using Brenda.Core.Identifiers;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Brenda.Core.Validations;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Brenda.Services
{
    public class CampaignPublisher : ICampaignPublisher
    {
        private readonly IBus _bus;
        private readonly ICampaignValidator _validator;
        private readonly IMapper _mapper;

        public CampaignPublisher(
            IBus bus,
            ICampaignValidator validator, 
            IMapper mapper)
        {
            _bus = bus;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<PublishValidation> Publish(Campaign campaign)
        {
            if (campaign == null) throw new ArgumentNullException(typeof(Campaign).Name);
            var resultValidation = await _validator.ValidateAsync(campaign).ConfigureAwait(false);
            if (!resultValidation.IsValid) return resultValidation;

            if (Actions.IsChallenge(campaign))
            {
                foreach (var campaigItem in campaign.Campaigns)
                {
                    campaigItem.Status = CampaignStatus.OnGoing;
                    campaigItem.Reward = campaign.Reward;
                }

                var challengeOnGoing = _mapper.Map<ChallengeOnGoing>(campaign);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(challengeOnGoing);
                campaign.PutOnGoing(json);
                await _bus.Publish(challengeOnGoing);
            }
            else
            {
                var campaignOnGoing = _mapper.Map<CampaignOnGoing>(campaign);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(campaignOnGoing);
                campaign.PutOnGoing(json);
                await _bus.Publish(campaignOnGoing);
            }

            return resultValidation;
        }
    }
}
