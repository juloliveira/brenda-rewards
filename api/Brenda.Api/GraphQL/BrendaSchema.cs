using GraphQL;
using GraphQL.Types;
using System;

namespace Brenda.Api.GraphQL
{
    public class BrendaSchema : Schema
    {
        public BrendaSchema(IServiceProvider serviceProvider)
            : base(new FuncDependencyResolver(t => serviceProvider.GetService(t)))
        {
            Query = (IObjectGraphType)serviceProvider.GetService(typeof(BrendaQuery));
            Mutation = null;
        }
    }
}
