using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Core.Exceptions
{
    public class ChallengeException : Exception
    {
        public ChallengeException(string message) : base(message)
        {

        }
    }
}
