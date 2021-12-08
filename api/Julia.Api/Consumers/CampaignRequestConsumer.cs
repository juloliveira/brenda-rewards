using MassTransit;
using Sara.Contracts.Events;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Julia.Api.Consumers
{
    public class CampaignOutOfDateConsumer : IConsumer<CampaignOutOfDate>
    {
        public async Task Consume(ConsumeContext<CampaignOutOfDate> context)
        {
            await context.Publish(context.Message.CampaignRequest);
        }
    }

    public class CampaignAlreadyRewardedConsumer : IConsumer<CampaignAlreadyRewarded>
    {
        public async Task Consume(ConsumeContext<CampaignAlreadyRewarded> context)
        {
            await context.Publish(context.Message.CampaignRequest);
        }
    }

    public class CampaignRequestConsumer : IConsumer<CampaignRequest>
    {
        public async Task Consume(ConsumeContext<CampaignRequest> context)
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_julia;Uid=hartbroot;Pwd=D45m7eba;");
            await conn.OpenAsync();
            await conn.InsertAsync(context.Message);
        }
    }
}
