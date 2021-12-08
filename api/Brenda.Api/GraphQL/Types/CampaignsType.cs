using GraphQL.Types;

namespace Brenda.Api.GraphQL.Types
{
    public class CampaignTypesType : ObjectGraphType
    {
        public CampaignTypesType()
        {
            Field<IntGraphType>("id");
        }
    }

    public class CampaignsType : ObjectGraphType
    {
        public CampaignsType()
        {
            Field<IntGraphType>("amount");
        }
    }
}
