using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Core
{
    public class Customer : Entity
    {
        protected Customer() { }

        public Customer(Guid id) : base(id)
        {

        }

        public string Name { get; set; }

        public string LogoAvatar { get; set; }
    }
}
