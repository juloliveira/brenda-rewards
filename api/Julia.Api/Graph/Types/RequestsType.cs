using HotChocolate.Types;
using Julia.Api.Models;

namespace Julia.Api.Graph.Types
{
    public class RequestsType : ObjectType<Requests>
    {
        protected override void Configure(IObjectTypeDescriptor<Requests> descriptor)
        {
            descriptor.Field(x => x.Max).Type<IntType>();
            descriptor.Field(x => x.X).Type<ListType<StringType>>();
            descriptor.Field(x => x.Y).Type<ListType<IntType>>();
        }
    }
}
