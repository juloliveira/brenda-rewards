using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Utils
{
    public class EndpointOptions
    {
        public string ConfirmEmail { get; set; }

        public string NewUserConfirmation { get; set; }

        public string Build(Func<EndpointOptions, string> func, params string[] values)
        {
            return string.Format(func.Invoke(this), values);
        }
    }
}
