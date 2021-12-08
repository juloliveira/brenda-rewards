using System;

namespace Sara.Contracts.Security
{
    public class UserRegister
    {
        public string Email { get; set; }
        public string Document { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int Sex { get; set; }
        public Guid? GenderIdentityId { get; set; }
        public Guid? IncomeId { get; set; }
        public Guid? SexualityId { get; set; }
        public Guid? EducationLevelId { get; set; }

        public string DeviceId { get; set; }
        public string DeviceData { get; set; }

        public string Token { get; set; }
        public string Password { get; set; }
    }
}
