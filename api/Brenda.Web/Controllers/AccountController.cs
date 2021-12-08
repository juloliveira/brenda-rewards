using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Brenda.Contracts.V1.Customer;
using Brenda.Core;
using Brenda.Core.Services;
using Brenda.Data;
using Brenda.Infrastructure.AWS;
using Brenda.Web.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Brenda.Web.Controllers
{
    [Authorize(Roles.Administrator)]
    public class AccountController : BrendaController
    {
        private readonly IUnitOfWork _uow;

        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
            return View();
        }

        public async Task<IActionResult> Transfer()
        {
            await LoadCampaigns(_uow, status: CampaignStatus.Pending);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferCommand command)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "O valor a ser transferido deve ser maior que 0.";
                return RedirectToAction("transfer");
            }

            var customer = await _uow.Customers.GetCurrentCustomerAsync();

            if (customer.Balance < command.ValueToTransfer)
            {
                TempData["Error"] = "Você não tem saldo suficiente para executar a transferencia neste valor.";
                return RedirectToAction("transfer");
            }

            var campaign = await _uow.Campaigns.GetByIdToPublishAsync(command.CampaignId);

            if (campaign == null)
            {
                TempData["Error"] = "A campanha não foi encontrada.";
                return RedirectToAction("transfer");
            }

            var user = await _uow.Users.GetCurrentAsync();

            var activityAccount = customer.Transfer(user, to: campaign, value: command.ValueToTransfer);

            await _uow.AddAsync(activityAccount);
            await _uow.CommitAsync();


            return RedirectToAction("ActivityShow", new { activityAccount.Id });
        }

        [HttpGet("account/activity/{id}/show")]
        public async Task<IActionResult> ActivityShow(Guid id) =>
            View(await _uow.Customers.GetAccountActivityByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> Settings([FromServices] AutoMapper.IMapper mapper)
        {
            var customer = await _uow.Customers.GetCurrentCustomerAsync();
            var settingsPost = mapper.Map<SettingsPost>(customer);
            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
            return View(settingsPost);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
            
            return View(await _uow.Users.GetActive());
        }

        [HttpGet("account/user/new")]
        public async Task<IActionResult> NewUser(
            [FromServices] RoleManager<BrendaRole> roleManager)
        {
            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
            ViewBag.Roles = new SelectList(await roleManager.Roles.Where(x => x.IsPublic).ToListAsync(), "Name", "Name");


            return View(new UserForm());
        }

        [HttpPost("account/user/new")]
        public async Task<IActionResult> NewUser(
            UserForm post,
            [FromServices] IUserRegisterService userRegistry,
            [FromServices] RoleManager<BrendaRole> roleManager)
        {
            if (ModelState.IsValid)
            {
                var customer = await _uow.Customers.GetCurrentCustomerAsync();
                var user = new BrendaUser(post.Name, post.Email);

                await userRegistry.Register(customer, post.Name, post.Email, post.Role);

                return RedirectToAction("Users");
            }

            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
            ViewBag.Roles = new SelectList(await roleManager.Roles.Where(x => x.IsPublic).ToListAsync(), "Name", "Name", post.Role);

            return View(post);
        }

        [HttpGet("account/user/{id}/edit")]
        public async Task<IActionResult> EditUser(
            Guid id,
            [FromServices] UserManager<BrendaUser> userManager)
        {
            var user = await _uow.Users.GetById(id);
            ViewBag.Roles = await userManager.GetRolesAsync(user);
            ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(
            SettingsPost post,
            [FromServices] IBus bus)
        {
            var customer = await _uow.Customers.GetCurrentCustomerAsync();
            if (post.Logo != null) await SaveLogo(post.Logo, customer);

            if (ModelState.IsValid)
            {
                customer.Name = post.Name;
                customer.Settings.Description = post.SettingsDescription;
                customer.Settings.Email = post.SettingsEmail;
                customer.Settings.PhoneNumber = post.SettingsPhoneNumber;
                customer.Settings.Site = post.SettingsSite;

                await _uow.CommitAsync();
            }
            else
            {
                ViewBag.Info = await _uow.Customers.GetAccountStatementAsync();
                return View(post);
            }

            if (ModelState.IsValid || post.Logo != null)
                await bus.Publish(new CustomerInfo { Id = customer.Id, Name = customer.Name, LogoAvatar = customer.Settings.LogoAvatar });

            return RedirectToAction("settings", "account");

        }

        private async Task SaveLogo(IFormFile formFile, Customer customer)
        {
            using var image = Image.FromStream(formFile.OpenReadStream(), true, true);

            if (false) //(image != null && (image.Width != 1920 || image.Height != 1920))
            {
                TempData["Swal"] = "Imagem deve ter 1920x1920 pixels.";
            }
            else
            {
                using AwsUploader aws = new AwsUploader();
                using MemoryStream originalStream = new MemoryStream(), avatarStream = new MemoryStream();
                using var avatar = new Bitmap(360, 360);
                using var graphic = Graphics.FromImage(avatar);

                image.Save(originalStream, System.Drawing.Imaging.ImageFormat.Png);
                var originalPath = await aws.WritingAnObjectAsync($"customers/{customer.Tag}/original.png", originalStream, "image/png");

                graphic.DrawImage(image, 0, 0, 360, 360);
                avatar.Save(avatarStream, System.Drawing.Imaging.ImageFormat.Png);

                var avatarPath = await aws.WritingAnObjectAsync($"customers/{customer.Tag}/avatar.png", avatarStream, "image/png");

                customer.Settings.SetLogo(originalPath, avatarPath);
                await _uow.CommitAsync();
            }
        }
    }
}
