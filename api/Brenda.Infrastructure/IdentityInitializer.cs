//using Brenda.Core;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;

//namespace Brenda.Data
//{
//    public class IdentityInitializer
//    {
//        private readonly BrendaContext _context;
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public IdentityInitializer(
//            BrendaContext context,
//            UserManager<User> userManager,
//            RoleManager<IdentityRole> roleManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        public void Initialize()
//        {
//            if (_context.Database.EnsureCreated())
//            {
//                if (!_roleManager.RoleExistsAsync(Roles.ROOT).Result)
//                {
//                    var resultado = _roleManager.CreateAsync(
//                        new IdentityRole(Roles.ROOT)).Result;
//                    if (!resultado.Succeeded)
//                    {
//                        throw new Exception(
//                            $"Erro durante a criação da role {Roles.ROOT}.");
//                    }
//                }

//                var man = new GenderIdentity { Mnemonic = "MAN", Name = "Homem" };
//                var woman = new GenderIdentity { Mnemonic = "WOMAN", Description = "Homem" };

//                CreateGenderIdentity(man);
//                CreateGenderIdentity(woman);

//                var user = new ApplicationUser(
//                        "Juliano",
//                        "Oliveira",
//                        "juliano@brendarewards.com",
//                        new DateTime(2019, 12, 24),
//                        man);
//                user.LockoutEnabled = true;
//                user.EmailConfirmed = true;

//                CreateUser(user, "Brenda@12324", Roles.ROOT);
//            }
//        }

//        private void CreateGenderIdentity(GenderIdentity genderIdentity)
//        {
//            if (!_context.GenderIdentity.Any(x => x.Mnemonic == genderIdentity.Mnemonic))
//            {
//                _context.GenderIdentity.Add(genderIdentity);
//                _context.SaveChanges();
//            }
//        }

//        private void CreateUser(
//            User user,
//            string password,
//            string initialRole = null)
//        {
//            if (_userManager.FindByNameAsync(user.UserName).Result == null)
//            {
//                var resultado = _userManager
//                    .CreateAsync(user, password).Result;

//                if (resultado.Succeeded &&
//                    !String.IsNullOrWhiteSpace(initialRole))
//                {
//                    _userManager.AddToRoleAsync(user, initialRole).Wait();
//                }
//            }
//        }
//    }
//}
