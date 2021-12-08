using Brenda.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            var users = builder.Metadata.FindNavigation(nameof(Customer.Users));
            users.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
