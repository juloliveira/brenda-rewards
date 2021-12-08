using HotChocolate;
using Julia.Api.Data;
using Julia.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Julia.Api.Graph
{
    public class Query
    {
        public async Task<IEnumerable<Sex>> GetSexualities(
            string customerId,
            DateTime sinceAt,
            [Service] JuliaContext context) => await context.GetSexualities(customerId, sinceAt);

        public async Task<Requests> GetRequests(
            string customerId,
            [Service] JuliaContext context)
        { 
            var requestPerDay = await context.GetRequestPerDay(customerId);
            var requests = new Requests();

            if (requestPerDay.Any())
                requests.Max = requestPerDay.Max(MySqlX => MySqlX.Requests);

            var day = DateTime.Now.AddDays(-10);
            var yesterday = DateTime.Now.AddDays(-1);
            for (; day.Date <= DateTime.Now.Date; day = day.AddDays(1))
            {
                if (day.Date == yesterday.Date)
                    requests.X.Add("Ontem");
                else if (day.Date == DateTime.Now.Date)
                    requests.X.Add("Hoje");
                else
                    requests.X.Add(day.ToString("dd-MM"));

                if (requestPerDay.Any(e => e.RequestsAt.Date == day.Date))
                    requests.Y.Add(requestPerDay.First(x => x.RequestsAt.Date == day.Date).Requests);
                else
                    requests.Y.Add(0);
            }

            return requests;
        }

        public async Task<IEnumerable<Income>> GetIncomes(
            string customerId,
            DateTime sinceAt,
            [Service] JuliaContext context) => await context.GetIncomes(customerId, sinceAt);

        public async Task<IEnumerable<Schooling>> GetSchooling(
            string customerId,
            DateTime sinceAt,
            [Service] JuliaContext context) => await context.GetSchooling(customerId, sinceAt);
    }


}
