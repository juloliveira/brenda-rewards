namespace Brenda.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Brenda.Core.Identifiers;
    using Brenda.Contracts.V1.Requests;
    using Brenda.Contracts.V1.Responses;
    using Brenda.Core;
    using Brenda.Core.DTO;
    using Brenda.Core.Interfaces;
    using Brenda.Core.Services;
    using Brenda.Infrastructure.Models;
    using Brenda.Web.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QRCoder;
    using Brenda.Data;

    [Authorize]
    public class CampaignsController : BrendaController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICampaignValidator _validator;
        private readonly IMapper _mapper;
        private readonly ICampaignPublisher _campaignPublisher;

        public CampaignsController(
            IUnitOfWork uow,
            ICampaignValidator validator,
            TenantInfo tenantInfo,
            ICampaignPublisher campaignPublisher,
            IMapper mapper)
        {
            _uow = uow;
            _validator = validator;
            _campaignPublisher = campaignPublisher;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var campaigns = await _uow.Campaigns.GetActive();
            return View(_mapper.Map<IEnumerable<CampaignViewModel>>(campaigns));
        }

        [HttpGet("campaign/create")]
        public async Task<IActionResult> Create()
        {
            await LoadActions(_uow);

            return View();
        }

        [HttpPost("campaign/create")]
        public async Task<IActionResult> Create(CampaignForm viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await _uow.Customers.GetCurrentCustomerAsync();
                var campaign = customer.AddCampaign();
                HandleModel(campaign, viewModel);

                await _uow.AddAsync(campaign);
                await _uow.CommitAsync();
                
                return RedirectToAction("view", "campaigns", new { id = campaign.Id.ToString() });
            }

            await LoadActions(_uow);

            return View(viewModel);
        }

        [HttpGet("campaign/{id}/view")]
        public async Task<IActionResult> View(Guid id)
        {
            var campaign = await _uow.Campaigns.GetByIdToPublishAsync(id);
            var viewModel = _mapper.Map<CampaignViewModel>(campaign);
            
            await LoadActions(_uow);
            ViewBag.Tag = campaign.Tag;

            if (Actions.IsQuiz(campaign.Asset))
            {
                var quiz = await _uow.Assets.GetQuizByIdAsync(campaign.Asset.Id);
                ViewBag.Quiz = _mapper.Map<IEnumerable<QuizView>>(quiz);
            }
            
            return View(viewModel);
        }

        [HttpPost("campaign/edit")]
        public async Task<IActionResult> Edit(CampaignForm post)
        {
            if (ModelState.IsValid && post.Id.HasValue)
            {
                var campaign = await _uow.Campaigns.GetByIdAsync(post.Id.Value);
                HandleModel(campaign, post);

                await _uow.CommitAsync();
            }

            return RedirectToAction("view", "campaigns", new { id = post.Id.ToString() });
        }

        [HttpGet]
        public async Task<IActionResult> Restriction(Guid id)
        {
            var definitions = await _uow.Campaigns.GetDefinitionsById(id);
            var viewModel = _mapper.Map<Contracts.V1.Responses.Campaings.Restriction[]>(definitions.CoordinatesAllowed);

            return Ok(viewModel);
        }

        [HttpPost("campaign/restriction")]
        public async Task<IActionResult> PostRestriction(Contracts.V1.Requests.Campaigns.Restriction request)
        {
            var campaign = await _uow.Campaigns.GetByIdAsync(request.Id);
            var restriction = campaign.AddRestriction(request);

            await _uow.AddAsync(restriction);
            await _uow.CommitAsync();

            return Ok(Responses.Id(restriction.Id));
        }

        [HttpPut("campaign/restriction")]
        public async Task<IActionResult> PutRestriction(Contracts.V1.Requests.Campaigns.Restriction put)
        {
            var restriction = await _uow.Campaigns.GetRestrictionById(put.Id);
            restriction.Latitude = put.Lat;
            restriction.Longitude = put.Lng;
            restriction.Radius = put.Radius;

            await _uow.CommitAsync();

            return Ok(new Response(put));
        }

        [HttpDelete("campaign/restriction")]
        public async Task<IActionResult> DeleteRestriction(Contracts.V1.Requests.Campaigns.Restriction delete)
        {
            var restriction = await _uow.Campaigns.GetRestrictionById(delete.Id);
            if (_uow.Campaigns.RemoveRestriction(restriction))
            {
                await _uow.CommitAsync();
                return Ok(Responses.Successful);
            }

            return Problem();
        }

        [HttpPut("campaign/{id}/asset")]
        public async Task<IActionResult> PutAsset(Guid id, [FromBody] BaseEntity asset)
        {
            await _uow.Campaigns.SetAsset(campaignId: id, assetId: asset.Id);
            return Ok(Responses.Successful);
        }

        [HttpGet("campaign/{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var campaign = await _uow.Campaigns.GetByIdToPublishAsync(id);
            var viewModel = _mapper.Map<CampaignViewModel>(campaign);

            ViewBag.ResultValidation = await _validator.ValidateAsync(campaign);

            return View(viewModel);
        }

        [HttpPut("campaign/{id}/publish")]
        public async Task<IActionResult> PublishPut(Guid id)
        {
            var campaign = await _uow.Campaigns.GetByIdToPublishAsync(id);
            var publishValidation = await _campaignPublisher.Publish(campaign);
            if (publishValidation.IsValid)
            {
                await _uow.CommitAsync();
                return RedirectJson($"/campaign/{id}/ongoing");
            }

            return Error("publish", new { id }, "Esta campanha não pode ser publicada por conter error de validação.");
        }

        [HttpGet("campaign/{id}/ongoing")]
        public async Task<IActionResult> OnGoing(Guid id)
        {
            var campaign = await _uow.Campaigns.GetOnGoingById(id);

            if (campaign == null)
                return Error("publish", new { id }, "Não foi encontrada uma campanha publicada");

            var viewModel = _mapper.Map<CampaignViewModel>(campaign);

            return View(viewModel);
        }

        [HttpGet("campaign/{id}/qrcode")]
        public async Task<IActionResult> QrCode(Guid id)
        {
            var tag = await _uow.Campaigns.GetTagAsync(id);
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"http:////uau.tw/{tag.ToUpper()}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return File(BitmapToBytes(qrCodeImage), "image/png");
        }

        [HttpGet("campaigns/search")]
        public async Task<IActionResult> Search(string q) =>
            Json(new Response((await _uow.Campaigns.FindByName(q)).Select(x => new { x.Id, x.Title, action = x.Action.Name })));

        [HttpPut("challenge/{challengeId}/campaign/{campaignId}")]
        public async Task<IActionResult> PutChallenge(Guid challengeId, Guid campaignId)
        {
            var challenge = await _uow.Campaigns.GetByIdAsync(challengeId);
            var campaign = await _uow.Campaigns.GetByIdAsync(campaignId);

            challenge.AddCampaign(campaign);

            await _uow.CommitAsync();

            return Ok(Responses.Id(challenge.Id));
        }

        [HttpPut("campaign/{campaignId}/url")]
        public async Task<IActionResult> PutUrl(Guid campaignId)
        {
            var campaign = await _uow.Campaigns.GetByIdToPublishAsync(campaignId);
            var urlAction = campaign.AddUrlAction();

            await _uow.AddAsync(urlAction);
            await _uow.CommitAsync();

            return Ok(Responses.Id(urlAction.Id));
        }

        [HttpPut("campaign/{campaignId}/url/{urlActionId}/update")]
        public async Task<IActionResult> PutUrlValue(Guid campaignId, Guid urlActionId, ValuePut put)
        {
            var urlAction = await _uow.Campaigns.GetUrlActionAsync(campaignId, urlActionId);
            urlAction.SetUrl(put.Value);

            await _uow.CommitAsync();

            return Ok();
        }

        [HttpDelete("campaign/{campaignId}/url/{urlActionId}/remove")]
        public async Task<IActionResult> DeleteUrl(Guid campaignId, Guid urlActionId)
        {
            var urlAction = await _uow.Campaigns.GetUrlActionAsync(campaignId, urlActionId);
            _uow.Campaigns.RemoveUrlAction(urlAction);
            await _uow.CommitAsync();

            return Ok();
        }

        private void HandleModel(Campaign campaign, CampaignForm viewModel)
        {

            campaign.Title = viewModel.Title;
            campaign.Description = viewModel.Description;
            campaign.ActionId = viewModel.ActionId;
            campaign.Reward = viewModel.Reward.Value;
            campaign.Definitions.ValidationStart = viewModel.DefinitionsValidationStart.Value;
            campaign.Definitions.ValidationEnd = viewModel.DefinitionsValidationEnd.Value;
            campaign.Definitions.ValidateGeoLocation = viewModel.DefinitionsValidateGeoLocation;
        }

        private static byte[] BitmapToBytes(System.Drawing.Bitmap img)
        {
            using var stream = new System.IO.MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }


    }

    

}
