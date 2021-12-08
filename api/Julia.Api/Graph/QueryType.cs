using HotChocolate.Types;
using Julia.Api.Graph.Types;
using System.Collections.Generic;

namespace Julia.Api.Graph
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor
                .Field(x => x.GetRequests(default, default))
                .Argument("customerId", x => x.Type<StringType>())
                .Type<RequestsType>();

            descriptor.Field(x => x.GetSexualities(default, default, default))
                .Argument("customerId", x => x.Type<StringType>())
                .Argument("sinceAt", x => x.Type<DateTimeType>())
                .Type<ListType<SexType>>();

            descriptor.Field(x => x.GetIncomes(default, default, default))
                .Argument("customerId", x => x.Type<StringType>())
                .Argument("sinceAt", x => x.Type<DateTimeType>())
                .Type<ListType<IncomeType>>();

            descriptor.Field(x => x.GetSchooling(default, default, default))
                .Argument("customerId", x => x.Type<StringType>())
                .Argument("sinceAt", x => x.Type<DateTimeType>())
                .Type<ListType<SchoolingType>>();

        }
    }


}
