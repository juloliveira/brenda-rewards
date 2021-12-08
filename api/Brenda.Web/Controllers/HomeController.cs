using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Brenda.Web.Models;
using Microsoft.AspNetCore.Authorization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Newtonsoft.Json;
using Brenda.Utils;

namespace Brenda.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(
            [FromServices] Brenda.Infrastructure.Models.TenantInfo tenantInfo)
        {
#if DEBUG
            var graphQLClient = new GraphQLHttpClient("http://julia/stats", new NewtonsoftJsonSerializer());
#else
            var graphQLClient = new GraphQLHttpClient("http://193.122.177.89/stats", new NewtonsoftJsonSerializer());
#endif
            var request = new GraphQLRequest
            {
                Query = @"
			    query Dashboard ($customerId: String, $sinceAt: DateTime) {
                    sexualities (customerId: $customerId, sinceAt: $sinceAt) { requestedAt, code, amount },
                    requests (customerId: $customerId) { max, x, y },
                    incomes(customerId: $customerId, sinceAt: $sinceAt) {
                            requestedAt,
                            incomeId,
                            description,
                            amount
                    },
                    schooling(customerId: $customerId, sinceAt: $sinceAt) {
                            educationLevelId,
                            description,
                            amount
                    }
                }", 
                OperationName = "Dashboard",
                Variables = new
                {
                    customerId = tenantInfo.CustomerId.ToString(),
                    sinceAt = DateTime.UtcNow.AddDays(-7)
                }
            };

            var stats = await graphQLClient.SendQueryAsync<Dashboard>(request);

            ViewBag.RequestsMax = stats.Data.Requests.Max;
            ViewBag.RequestsX = JsonConvert.SerializeObject(stats.Data.Requests.X);
            ViewBag.RequestsY = JsonConvert.SerializeObject(stats.Data.Requests.Y);

            ViewBag.Sexualities = JsonConvert.SerializeObject(stats.Data.Sexualities);
            ViewBag.Incomes = JsonConvert.SerializeObject(stats.Data.Incomes);
            ViewBag.Schooling = JsonConvert.SerializeObject(stats.Data.Schooling);

            return View();
        }
    }

    public class Dashboard
    {
        public Requests Requests { get; set; }

        public IEnumerable<Sex> Sexualities { get; set; }

        public IEnumerable<Income> Incomes { get; set; }
        public IEnumerable<Schooling> Schooling { get; set; }
    }

    public class Schooling
    {
        public string EducationLevelId { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
    }

    public class Income
    {
        public long RequestedAt { get; set; }
        public string IncomeId { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }

        public string Date => DateTimeExtentions.FromTimestamp(RequestedAt).ToString("dd/MM");
    }

    public class Sex
    {
        public long RequestedAt { get; set; }
        public int Code { get; set; }
        public int Amount { get; set; }

        public string Description => Code == 1 ? "Homens" : "Mulheres";

        public string Date => DateTimeExtentions.FromTimestamp(RequestedAt).ToString("dd/MM");
    }

    public class Requests
    {
        public int Max { get; set; }
        public IEnumerable<int> Y { get; set; }
        public IEnumerable<string> X { get; set; }
    }
}
