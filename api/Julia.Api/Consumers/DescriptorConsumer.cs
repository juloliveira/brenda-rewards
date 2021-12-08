using Dapper.Contrib.Extensions;
using MassTransit;
using Sara.Contracts.Commands;
using System;
using System.Threading.Tasks;
using Dapper;

namespace Julia.Api.Consumers
{
    public class DescriptorConsumer : IConsumer<Descriptor>
    {
        public async Task Consume(ConsumeContext<Descriptor> context)
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_julia;Uid=hartbroot;Pwd=D45m7eba;");
            await conn.OpenAsync();

            var query = @"INSERT INTO Descriptors (DescriptorId, Description, Entity) 
                            VALUES (@Id, @Description, @Entity)
                            ON DUPLICATE KEY UPDATE DescriptorId = @Id, Description = @Description, Entity = @Entity";
            await conn.ExecuteAsync(query, context.Message);
            
        }
    }
}
