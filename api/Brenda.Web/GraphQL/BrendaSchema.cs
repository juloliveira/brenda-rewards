using GraphQL;
using GraphQL.Types;
using System;

namespace Brenda.Web.GraphQL
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
