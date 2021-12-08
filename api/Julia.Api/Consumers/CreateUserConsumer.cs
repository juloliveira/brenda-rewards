using MassTransit;
using Sara.Contracts.Events;
using System;
using System.Threading.Tasks;

namespace Julia.Api.Consumers
{
    using Dapper.Contrib.Extensions;

    public class CreateUserConsumer : IConsumer<CreateUser>
    {
        public async Task Consume(ConsumeContext<Sara.Contracts.Events.CreateUser> context)
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection("Server=hartb-aurora-data-cluster.cluster-ctdczij8yo56.us-east-1.rds.amazonaws.com;Port=3306;Database=hartb_brenda_julia;Uid=hartbroot;Pwd=D45m7eba;");
            await conn.OpenAsync();
            await conn.InsertAsync((CreateUserConsumer.CreateUser)context.Message);
        }

        public class CreateUser
        {
            public static explicit operator CreateUser(Sara.Contracts.Events.CreateUser original)
            {
                return new CreateUserConsumer.CreateUser
                {
                    UserId = original.UserId,
                    Birthdate = original.Birthdate,
                    Sex = original.Sex,
                    GenderIdentityId = original.GenderIdentityId,
                    IncomeId = original.IncomeId,
                    SexualityId = original.SexualityId,
                    EducationLevelId = original.EducationLevelId,
                    Latitude = original.Latitude,
                    Longitude = original.Longitude,
                    DeviceId = original.DeviceId,
                    DeviceData = original.DeviceData,
                    CreatedAt = original.CreatedAt
                };
            }

            public Guid UserId { get; set; }
            public DateTime Birthdate { get; set; }
            public int Sex { get; set; }
            public Guid GenderIdentityId { get; set; }
            public Guid IncomeId { get; set; }
            public Guid SexualityId { get; set; }
            public Guid EducationLevelId { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }

            public string DeviceId { get; set; }
            public string DeviceData { get; set; }

            public DateTime CreatedAt { get; set; }
        }

    }
}
