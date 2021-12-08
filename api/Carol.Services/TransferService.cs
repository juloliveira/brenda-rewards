using Carol.Core;
using Carol.Core.Services;
using Carol.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Carol.Services
{
    public class TransferService : ITransferService
    {
        private readonly CarolContext _context;

        public TransferService(CarolContext context)
        {
            _context = context;
        }

        public async Task<Transaction> Transfer(User sender, User destination, double value)
        {
            using (var tx = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var txSender = sender.Transfer(to: destination, value: value);
                    var txDestination = destination.ReceiveTransfer(sender, txSender);

                    _context.Entry(sender).Property(x => x.Balance).IsModified = true;
                    _context.Entry(destination).Property(x => x.Balance).IsModified = true;

                    await _context.AddRangeAsync(new[] { txSender, txDestination });
                    await _context.SaveChangesAsync();
                    await tx.CommitAsync();

                    return txSender;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}
