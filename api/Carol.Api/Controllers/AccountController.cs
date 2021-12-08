using System.Threading.Tasks;
using AutoMapper;
using Brenda.Contracts.V1.Responses;
using Carol.Api.Model;
using Carol.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Carol.Core.Services;
using MassTransit;
using Sara.Contracts.Commands;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Carol.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly CarolContext _context;
        private readonly TenantInfo _tenantInfo;
        private readonly IMapper _mapper;
        private readonly ITransferService _transferService;
        private readonly IBus _bus;

        public AccountController(
            CarolContext context,
            TenantInfo tenantInfo,
            IMapper mapper,
            ITransferService transferService,
            IBus bus
            )
        {
            _context = context;
            _tenantInfo = tenantInfo;
            _mapper = mapper;
            _transferService = transferService;
            _bus = bus;
        }

        [HttpGet("/account")]
        public async Task<IActionResult> Get()
        {
            var user = await _context.Users
                .Where(x => x.Id == _tenantInfo.UserId)
                .SingleAsync();

            var transactions = await _context.Transaction
                .Where(x => x.UserId == _tenantInfo.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();
            
            var viewmodel = _mapper.Map<UserViewModel>(user);
            viewmodel.Transactions = _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);

            return Ok(Responses.Data(viewmodel));
        }

        [HttpPost("/send")]
        public async Task<IActionResult> GetUserSend(UserSendPost post)
        {
            var userSend = await _context.Users.SingleOrDefaultAsync(x => x.Id == post.UserId);
            if (userSend == null)
                return Problem();

            return Ok(Responses.Data(new { userSend.Id, userSend.Email, userSend.PhoneNumber }));
        }

        [HttpPut("/transfer")]
        public async Task<IActionResult> Transfer(UserTransfer put)
        {
            var sender = await _context.Users.SingleAsync(x => x.Id == _tenantInfo.UserId);
            var destination = await _context.Users.SingleAsync(x => x.Id == put.ToUserId);

            if (sender.Id == destination.Id || put.Value <= 0) throw new ArgumentException(nameof(put));

            var sendTransaction = await _transferService.Transfer(sender, destination, put.Value);

            await _bus.Publish(new TransferReceivedPushMessage 
            { 
                UserId = destination.Id, 
                FromUserEmail = sender.Email, 
                Value = sendTransaction.Value,
                Balance = destination.Balance
            });

            await _bus.Publish(new BalanceChangePushMessage { UserId = sender.Id, Balance = sender.Balance });

            return Ok(Responses.Data(new { TransferId = sendTransaction.Id }));
        }
    }

    public struct UserTransfer
    {
        public Guid ToUserId { get; set; }
        public string ToUserEmail { get; set; }
        public string ToUserPhone { get; set; }
        public double Value { get; set; }
    }

    public struct UserSendPost
    {
        public Guid UserId { get; set; }
    }
}
