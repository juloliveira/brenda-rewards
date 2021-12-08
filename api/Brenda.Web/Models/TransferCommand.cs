using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Web.Models
{
    public class TransferCommand
    {
        public Guid CampaignId { get; set; }

        [Range(1, double.MaxValue)]
        public double ValueToTransfer { get; set; }
    }
}
