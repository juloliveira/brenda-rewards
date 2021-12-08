using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Brenda.Contracts.V1.Requests;
using Brenda.Contracts.V1.Responses;
using Brenda.Core;
using Brenda.Core.Identifiers;
using Brenda.Core.Interfaces;
using Brenda.Data;
using Brenda.Infrastructure.Models;
using Brenda.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Web.Controllers
{
    [Authorize]
    public class AssetsController : BrendaController
    {
        private readonly TenantInfo _tenantInfo;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _uow;

        public AssetsController(
            TenantInfo tenantInfo,
            IMapper mapper,
            IUnitOfWork uow
            )
        {
            _tenantInfo = tenantInfo;
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index() =>
            View(_mapper.Map<IEnumerable<AssetForm>>(await _uow.Assets.GetAllAsync()));

        [HttpGet("asset/create")]
        public async Task<IActionResult> Create()
        {
            await LoadActions(_uow);

            return View(new AssetForm());
        }

        [HttpPost("asset/create")]
        public async Task<IActionResult> Create(AssetForm form)
        {
            if (ModelState.IsValid)
            {
                var asset = new Asset(form.Title, _tenantInfo.CustomerId.Value, form.ActionId.Value);

                await _uow.AddAsync(asset);
                await _uow.CommitAsync();

                return RedirectToAction("view", new { asset.Id });
            }

            await LoadActions(_uow);

            return View(form);
        }

        [HttpGet("asset/{id}/view")]
        public async Task<IActionResult> View(Guid id)
        {
            await LoadActions(_uow);
            var asset = await _uow.Assets.GetByIdAsync(id);
            var viewModel = _mapper.Map<AssetForm>(asset);

            if (Brenda.Core.Identifiers.Actions.IsQuiz(asset.ActionId))
            {
                var quiz = await _uow.Assets.GetQuizByIdAsync(id);
                ViewBag.Quiz = _mapper.Map<IEnumerable<QuizView>>(quiz);
            }

            return View(viewModel);
        }

        [HttpPost("asset/edit")]
        public async Task<IActionResult> Edit(AssetForm form)
        {
            if (ModelState.IsValid && form.Id.HasValue)
            {
                var asset = await _uow.Assets.GetByIdAsync(form.Id.Value);
                asset.Title = form.Title;
                asset.ActionId = form.ActionId.Value;

                await _uow.CommitAsync();

                return RedirectToAction("view", new { asset.Id });
            }

            return View(form);
        }

        public class Aws
        {
            public string Filename { get; set; }
            public string Type { get; set; }
        }

        [HttpPut("asset/storage")]
        public IActionResult S3PreSignedUrl([FromBody] Aws post)
        {
            //arn:aws:s3:::brendarewards
            var aws = new Amazon.S3.AmazonS3Client("AKIAUENCKRUASG4INK4A", "OTvYq5UQNXfM7+HNKvODwodarz8DVKzqJcuhecnv", Amazon.RegionEndpoint.USEast2);
            var request = new Amazon.S3.Model.GetPreSignedUrlRequest
            {
                BucketName = "brendarewards",
                Key = $"videos/{_tenantInfo.CustomerId}/{post.Filename}",
                Expires = DateTime.UtcNow.AddMinutes(5),
                ContentType = post.Type,
                Verb = Amazon.S3.HttpVerb.PUT
            };

            var token = aws.GetPreSignedURL(request);

            return Json(new 
            { 
                method = HttpContext.Request.Method,
                url = token,
                fields = new string[0],
                headers = new []
                {
                    new string[] { "Content-Type", post.Type }
                }
            });

        }

        [HttpPut("asset/{id}/resource")]
        public async Task<IActionResult> Resource(Guid id, [FromBody] ResourcePut put)
        {
            var asset = await _uow.Assets.GetByIdAsync(id);
            asset.Resource = put.Resource;
            asset.Enable = true;

            await _uow.CommitAsync();

            return Ok(Responses.Successful);
        }

        [HttpGet("assets/search")]
        public async Task<IActionResult> Search(string q, string actionTag) =>
            Json(new Response((await _uow.Assets.FindByIdOrName(q, actionTag)).Select(x => new { x.Id, x.Title, x.Resource })));

        [HttpPost("asset/{id}/redirect")]
        public async Task<IActionResult> Redirect(Guid id, RedirectPost post)
        {
            if (id != post.Id) throw new ArgumentException();

            var urlchk = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (!urlchk.IsMatch(post.Url))
            {
                TempData["Error"] = "A URL não está em um formato válido. Atenção: Sua url deve iniciar com HTTPS.";
            }
            else
            {
                var asset = await _uow.Assets.GetByIdAsync(post.Id);
                asset.Resource = post.Url;
                asset.Enable = true;
                await _uow.CommitAsync();

                TempData["Message"] = "O <strong>asset de redirecionamento</strong> foi salvo com sucesso.";
            }

            return RedirectToAction("view", new { id });
        }

        [HttpPut("asset/{id}/question")]
        public async Task<IActionResult> PutQuestion(Guid id)
        {
            var asset = await _uow.Assets.GetByIdAsync(id);
            if (asset == null) throw new ArgumentException("Asset não encontrado.");
            if (!Actions.IsQuiz(asset.ActionId)) throw new ArgumentException("Asset Action deve ser Action Quiz.");
            if (asset.Questions.Count() >= 3) throw new InvalidOperationException("São permitidas apenas 3 questões.");

            var quizQuestion = asset.AddQuiz();

            await _uow.AddAsync(quizQuestion);
            await _uow.CommitAsync();

            return Ok(Responses.Id(quizQuestion.Id));
        }

        [HttpPut("asset/{assetId}/question/{questionId}/option")]
        public async Task<IActionResult> PutOption(Guid assetId, Guid questionId)
        {
            var quiz = await _uow.Assets.GetQuestionByIdAsync(assetId, questionId);
            if (quiz == null) throw new ArgumentException();
            var option = quiz.AddOption();

            await _uow.AddAsync(option);
            await _uow.CommitAsync();

            return Ok(Responses.Id(option.Id));
        }

        [HttpPut("asset/{assetId}/quiz/{quizId}")]
        public async Task<IActionResult> PutQuiz(Guid assetId, Guid quizId, ValuePut put)
        {
            if (string.IsNullOrEmpty(put.Value)) throw new ArgumentException();
            var questions = await _uow.Assets.GetQuizByIdAsync(assetId);
            if (questions == null) throw new ArgumentException();

            questions.Select(x => new { x, x.Options});

            // TODO: Precisa ser refatorado
            Quiz quiz;
            quiz = questions.FirstOrDefault(x => x.Id == quizId);
            if (quiz == null) quiz = questions.SelectMany(x => x.Options).FirstOrDefault(x => x.Id == quizId);

            quiz.Description = put.Value;
            await _uow.CommitAsync();

            return Ok(Responses.Id(quiz.Id));
        }

        [HttpDelete("asset/{assetId}/question/{questionId}/option/{optionId}/remove")]
        public async Task<IActionResult> DeleteOption(Guid assetId, Guid questionId, Guid optionId)
        {
            var questions = await _uow.Assets.GetQuizByIdAsync(assetId);
            if (questions == null || !questions.Any()) return Ok();

            var option = questions
                .Where(x => x.Id == questionId)
                .SelectMany(x => x.Options)
                .FirstOrDefault(x => x.Id == optionId);

            if (option == null) return Ok();

            _uow.Assets.Remove(option);
            await _uow.CommitAsync();

            return Ok();
        }
    }

    public class RedirectPost
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}
