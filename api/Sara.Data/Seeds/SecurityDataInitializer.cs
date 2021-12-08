using Microsoft.AspNetCore.Identity;
using Sara.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Data.Seeds
{
    public static class SecurityDataInitializer
    {
        public static void SeedRoles(RoleManager<SaraRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new SaraRole { Name = "Administrator" };
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Default").Result)
            {
                var role = new SaraRole { Name = "Default" };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
