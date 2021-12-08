using System;
using System.Collections.Generic;

namespace Julia.Api.Models
{
    public class Schooling
    {
        public string EducationLevelId { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
    }

    public class Income
    {
        public long RequestedAt { get; set; }
        public string IncomeId { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
    }

    public class Sex
    {
        public long RequestedAt { get; set; }
        public int Code { get; set; }
        public int Amount { get; set; }
    }


}
