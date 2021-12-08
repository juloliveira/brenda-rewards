using System;
using System.Collections.Generic;

namespace Julia.Api.Data
{
    public class RequestPerDay
    {
        public DateTime RequestsAt { get; set; }
        public int Requests { get; set; }
    }
}
