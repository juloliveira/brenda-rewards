using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carol.Api.Model
{
    public struct UserViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public double Balance { get; set; }

        public IEnumerable<TransactionViewModel> Transactions { get; set; }
    }

    public struct TransactionViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Customer { get; set; }

        public int Origin { get; set; }

        public Guid Reference { get; set; }

        public int Operation { get; set; }

        public double Value { get; set; }

        public long CreatedAt { get; set; }
    }
}
