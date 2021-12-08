using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;

namespace Brenda.Core
{
    public class BrendaRole : IdentityRole<Guid>
    {
        protected BrendaRole() { }

        public BrendaRole(bool isPublic)
        {
            Id = Guid.NewGuid();
            IsPublic = isPublic;
        }

        public string Description { get; set; }

        public bool IsPublic { get; protected set; }
    }

    public class BrendaUser : IdentityUser<Guid>
    {
        protected BrendaUser() {}

        public BrendaUser(string name, string email) 
        {
            Id = Guid.NewGuid();
            Name = name;
            UserName = email;
            Email = email;
        }

        public BrendaUser(string name, string email, Customer customer) : this(name, email)
        {
            CustomerId = customer.Id;
        }

        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public double PointsBalance { get; set; }
        public string PushToken { get; set; }
        
        public Guid CustomerId { get; protected set; }
        public Customer Customer { get; protected set; }
        public bool IsDeleted { get; set; }


        protected internal void SetCustomer(Customer customer)
        {
            Customer = customer;
        }
    }
}
