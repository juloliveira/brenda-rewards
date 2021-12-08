using Brenda.Core;
using Brenda.Data;
using Brenda.Web.GraphQL.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;

namespace Brenda.Web.GraphQL
{
    public class BrendaQuery : ObjectGraphType
    {
        public BrendaQuery(Func<BrendaContext> dataContext)
        {
            Field<CampaignType>("campaigns",
                resolve: graph =>
                {
                    return new { Amount = 20 };
                });

            FieldAsync<ListGraphType<ActionType>>("actions",
                resolve: async graph =>
                {
                    using var context = dataContext();
                    return await context.Actions.ToListAsync();
                });
        }
    }
}
