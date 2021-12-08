using MassTransit;
using Sara.Contracts.Events;
using System.Threading.Tasks;

namespace Julia.Api.Consumers
{
    using Dapper.Contrib.Extensions;

    public class RewardUserConsumer : IConsumer<RewardUser>
    {
        public async Task Consume(ConsumeContext<RewardUser> context)
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_julia;Uid=hartbroot;Pwd=D45m7eba;");
            await conn.OpenAsync();
            await conn.InsertAsync(context.Message);
        }
    }
}
