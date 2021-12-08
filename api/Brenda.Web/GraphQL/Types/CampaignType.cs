using GraphQL.Types;

namespace Brenda.Web.GraphQL.Types
{
    public class ActionType : ObjectGraphType
    {
        public ActionType()
        {
            Field<GuidGraphType>("id");
            Field<StringGraphType>("name");
        }
    }

    public class CampaignType : ObjectGraphType
    {
        public CampaignType()
        {
            Field<IntGraphType>("amount");
        }
    }
}
