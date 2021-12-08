using Brenda.Api.GraphQL.Types;
using GraphQL.Types;
using System;

namespace Brenda.Api.GraphQL
{
    public class BrendaQuery : ObjectGraphType
    {
        public BrendaQuery()
        {
            Field<CampaignsType>("campaigns",
                resolve: context => 
                {
                    return new { Amount = 20 };
                });

            Field<CampaignTypesType>("campaign_types",
                resolve: context =>
                {
                    return new { Id = 123 };
                });
        }
    }
}
