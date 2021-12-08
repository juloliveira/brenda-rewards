using System.Collections.Generic;

namespace Julia.Api.Models
{
    public class Requests
    {
        public Requests()
        {
            Max = 0;
            Y = new List<int>();
            X = new List<string>();
        }

        public int Max { get; set; }
        public IList<int> Y { get; set; }
        public IList<string> X { get; set; }
    }
}
