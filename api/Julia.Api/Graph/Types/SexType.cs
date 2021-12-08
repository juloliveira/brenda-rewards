using HotChocolate.Types;
using Julia.Api.Models;

namespace Julia.Api.Graph.Types
{
    public class SchoolingType : ObjectType<Schooling>
    {
        protected override void Configure(IObjectTypeDescriptor<Schooling> descriptor)
        {
            descriptor.Field(x => x.EducationLevelId).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Amount).Type<IntType>();
        }
    }

    public class IncomeType : ObjectType<Income>
    {
        protected override void Configure(IObjectTypeDescriptor<Income> descriptor)
        {
            descriptor.Field(x => x.RequestedAt).Type<LongType>();
            descriptor.Field(x => x.IncomeId).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Amount).Type<IntType>();
        }
    }

    public class SexType : ObjectType<Sex>
    {
        protected override void Configure(IObjectTypeDescriptor<Sex> descriptor)
        {
            descriptor.Field(x => x.RequestedAt).Type<LongType>();
            descriptor.Field(x => x.Code).Type<IntType>();
            descriptor.Field(x => x.Amount).Type<IntType>();
        }
    }
}
