using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Core.Exceptions
{
    public class ActivityUserInvalidException : Exception
    {
        public ActivityUserInvalidException() : base("User not allowed to this operation.")
        {
            
        }
    }
}
