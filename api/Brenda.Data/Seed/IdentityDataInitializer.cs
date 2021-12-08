using Brenda.Core;
using Brenda.Core.Identifiers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace Brenda.Data.Seed
{
    public class IdentityDataInitializer
    {
        public static void SeedData(
            UserManager<BrendaUser> userManager,
            RoleManager<BrendaRole> roleManager,
            BrendaContext context)
        {
            SeedRoles(roleManager);
            SeedFormats(context);
            SeedCustomers(context);
            SeedUsers(userManager);
            SeedErrorMessages(context);
        }

        public static void SeedErrorMessages(BrendaContext ctx)
        {
            if (!ctx.ErrorMessages.Any(x => x.Field == "Brenda.Core.Campaign:Title"))
            {
                var errors = new ErrorMessage[]
                {
                    new ErrorMessage { FieldName = "Título", Field = "Brenda.Core.Campaign:Title", Message = "Você deve definir um título para a campanha.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Recompensa", Field = "Brenda.Core.Campaign:Reward", Message = "A campanha deve ter um valor de recompensa definido.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Saldo", Field = "Brenda.Core.Campaign:Balance", Message = "A campanha deve ter saldo.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Asset", Field = "Brenda.Core.Campaign:AssetId", Message = "Você deve definir um asset para a campanha", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Status", Field = "Brenda.Core.Campaign:Status", Message = "O status da campanha é inválido.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Data de Início", Field = "Brenda.Core.CampaignDefinitions:ValidationStart", Message = "A data inicial da campanha está maior que a data de hoje.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Data Final", Field = "Brenda.Core.CampaignDefinitions:ValidationEnd", Message = "A data final é menor que a data de hoje.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Restrições", Field = "Brenda.Core.CampaignDefinitions:CoordinatesAllowed", Message = "A campanha deve ter pelo menos uma restrição geográfica definida no mapa.", Lang = "pt-BR" },

                    new ErrorMessage { FieldName = "Campanhas", Field = "Challenge:MustHaveCampaigns", Message = "Uma challenge deve ter pelo menos 2 outras campanhas", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Campanhas", Field = "Challenge:CampaignsBalance", Message = "As campanhas que pasticipam de uma CHallenge não podem ter saldo.", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Campanhas", Field = "Challenge:MustNotHaveChallenge", Message = "Uma campanha Challenge não pode ter outras campanhas Challenge", Lang = "pt-BR" },
                    new ErrorMessage { FieldName = "Campanhas", Field = "Challenge:CampaignAsset", Message = "Cada uma das Campanhas que fazem parte da Challenge devem ter um Asset Action válido definido.", Lang = "pt-BR" },

                };

                ctx.ErrorMessages.AddRange(errors);
                ctx.SaveChanges();
            }
        }

        public static void SeedFormats(BrendaContext context)
        {
            if (!context.Actions.Any(x => x.Id == new Guid(Actions.Redirect)))
            {
                context.Actions.Add(new Core.Action(new Guid(Actions.Redirect)) { Name = "Redirecionamento", Image = "/assets/media/svg/icons/Communication/Sending.svg" });
                context.Actions.Add(new Core.Action(new Guid(Actions.Video)) { Name = "Video", Image = "/assets/media/svg/icons/Media/Youtube.svg" });
                context.Actions.Add(new Core.Action(new Guid(Actions.Quiz)) { Name = "Quiz", Image = "/assets/media/svg/icons/Communication/Chat6.svg" });
                context.Actions.Add(new Core.Action(new Guid(Actions.Challenge)) { Name = "Challenge", Image = "/assets/media/svg/icons/Devices/Gameboy.svg" });

                context.SaveChanges();
            }
        }
      
        public static void SeedCustomers(BrendaContext context)
        {
            if (!context.Customers.Any(x => x.Id == new Guid("B68EBBDF-9D15-4BD9-B2EB-35C1FD61D382")))
            {
                var customer = new Customer("Brenda Rewards", "36575053000188");
                typeof(Customer).GetProperty("Id").SetValue(customer, new Guid("B68EBBDF-9D15-4BD9-B2EB-35C1FD61D382"));

                context.Customers.AddAsync(customer);
                context.SaveChanges();
            }
        }

        public static void SeedUsers(UserManager<BrendaUser> userManager)
        {
            if (userManager.FindByEmailAsync("juliano@brendarewards.com").Result == null)
            {
                var user = new BrendaUser("Juliano Oliveira", "juliano@brendarewards.com");
                user.EmailConfirmed = true;
                user.MobilePhone = "11966496848";

                typeof(BrendaUser).GetProperty("CustomerId").SetValue(user, new Guid("B68EBBDF-9D15-4BD9-B2EB-35C1FD61D382"));

                var identityResult = userManager.CreateAsync(user, "Brenda@1234").Result;

                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.ROOT).Wait();
                    userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<BrendaRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("3c978dbdde2e447fa15ef4ab58b70f48").Result)
            {
                var role = new BrendaRole(isPublic: false) { Name = Roles.ROOT, Description = "Administrador da Plataforma BrendaRewards" };
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new BrendaRole(isPublic: true) { Name = Roles.Administrator, Description = "Administrador de Conta de Cliente" };
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Collaborator").Result)
            {
                var role = new BrendaRole(isPublic: true) { Name = Roles.Collaborator, Description = "Usuários Colaboradores das Contas de Cliente" };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
