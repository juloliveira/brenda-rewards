using MassTransit.Transports.Metrics;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Julia.Api.Models;
using Brenda.Utils;

namespace Julia.Api.Data
{
    public class JuliaContext : IAsyncDisposable
    {
        private readonly MySqlConnection _connection;

        const int UTC_MINUS_3 = 10800;
        const int DAY_SECONDS = 86400;

        public JuliaContext()
        {
            _connection = new MySqlConnection("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_julia;Uid=hartbroot;Pwd=D45m7eba;");
            _connection.Open();
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.DisposeAsync();
        }

        internal async Task<IEnumerable<Schooling>> GetSchooling(string customerId, DateTime sinceAt)
        {
            var query = @"SELECT  
                            cu.EducationLevelId,
                            (SELECT Description FROM Descriptors d WHERE d.DescriptorId = cu.EducationLevelId) Description, 
                            Count(cu.EducationLevelId) Amount    
                        FROM CampaignRequests cr 
                        INNER JOIN CreateUsers cu ON cr.UserId = cu.UserId
                        WHERE cr.CustomerId = @customerId AND cr.RequestedAt > @sinceAt
                        GROUP BY cu.EducationLevelId";

            return await _connection.QueryAsync<Schooling>(query, new { customerId, sinceAt = sinceAt.Date.ToTimestamp() });
        }

        internal async Task<IEnumerable<Sex>> GetSexualities(string customerId, DateTime sinceAt)
        {
            var query = @"SELECT 
	                        (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400) AS RequestedAt,
                            cu.Sex Code, 
                            Count(cu.Sex) Amount    
                        FROM CampaignRequests cr 
                        INNER JOIN CreateUsers cu ON cr.UserId = cu.UserId
                        WHERE cr.CustomerId = @customerId AND cr.RequestedAt >= @sinceAt
                        GROUP BY (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400), cu.Sex
                        ORDER BY cr.RequestedAt ASC";

            return await _connection.QueryAsync<Sex>(query, new { customerId, sinceAt = sinceAt.Date.ToTimestamp() });
        }

        internal async Task<IEnumerable<RequestPerDay>> GetRequestPerDay(string customerId)
        {
            var query = @"SELECT 
	                        FROM_UNIXTIME(FLOOR((cr.RequestedAt - @UTC_MINUS_3) / @DAY_SECONDS ) * @DAY_SECONDS) AS RequestsAt, count(1) as Requests 
                          FROM CampaignRequests cr
                          WHERE cr.CustomerId = @customerId
                          GROUP BY (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400) 
                          ORDER BY cr.RequestedAt DESC
                          LIMIT 10;";

            return await _connection.QueryAsync<RequestPerDay>(query, new { customerId, UTC_MINUS_3, DAY_SECONDS });
        }

        internal async Task<IEnumerable<Income>> GetIncomes(string customerId, DateTime sinceAt)
        {
            var query = @"SELECT 
	                        (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400) AS RequestedAt,
	                        cu.IncomeId,
                            (SELECT Description FROM Descriptors d WHERE d.DescriptorId = cu.IncomeId) Description, 
                            Count(cu.IncomeId) Amount    
                        FROM CampaignRequests cr 
                        INNER JOIN CreateUsers cu ON cr.UserId = cu.UserId
                        WHERE cr.CustomerId = @customerId and cr.RequestedAt >= @sinceAt
                        GROUP BY (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400), cu.IncomeId
                        ORDER BY (FLOOR((cr.RequestedAt - 10800) / 86400 ) * 86400) ASC, Amount ASC";

            return await _connection.QueryAsync<Income>(query, new { customerId, sinceAt = sinceAt.Date.ToTimestamp() });
        }
    }

    
}
