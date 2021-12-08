using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Core.Exceptions
{
    public class CampainInvalidException : Exception
    {
        public CampainInvalidException() : base("Campanha inválida.")
        {
            
        }
    }
}
