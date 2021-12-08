using System;
using System.Collections.Generic;

namespace Sara.Core
{
    public class SaraUser : Microsoft.AspNetCore.Identity.IdentityUser<System.Guid>
    {
        private readonly List<Voucher> _vouchers;

        protected SaraUser() { }

        public SaraUser(string email, 
            string document, 
            string phoneNumber, 
            DateTime birthdate, 
            int sex, 
            GenderIdentity genderIdentity, 
            Sexuality sexuality, 
            Income income, 
            EducationLevel educationLevel) : base(email)
        {
            Email = email;
            Document = document;
            PhoneNumber = phoneNumber;
            Birthdate = birthdate;
            Sex = (Sex)sex;

            GenderIdentityId = genderIdentity.Id;
            SexualityId = sexuality.Id;
            IncomeId = income.Id;
            EducationLevelId = educationLevel.Id;

            _vouchers = new List<Voucher>();
        }

        public string Document { get; set; }

        public DateTime Birthdate { get; set; }

        public Sex Sex { get; set; }

        public Guid GenderIdentityId { get; set; }
        public GenderIdentity GenderIdentity { get; set; }

        public Guid SexualityId { get; set; }
        public Sexuality Sexuality { get; set; }

        public Guid IncomeId { get; set; }
        public Income Income { get; set; }

        public Guid EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }


        public IReadOnlyCollection<Voucher> Vouchers { get { return _vouchers; } }

        public string FirebaseToken { get; set; }

        public Voucher CreateVoucher(Campaign campaign)
        {
            var voucher = new Voucher(this, campaign);
            _vouchers.Add(voucher);

            return voucher;
        }
    }

    public class Voucher : Entity
    {
        protected Voucher() { }
        public Voucher(SaraUser user, Campaign campaign) : base()
        {
            UserId = user.Id;
            CampaignId = campaign.Id;
            WasRewarded = false;
            RewardedAt = null;
        }

        public Guid CampaignId { get; protected set; }

        public Campaign Campaign { get; protected set; }

        public Guid UserId { get; protected set; }

        public SaraUser User { get; protected set; }


        public bool WasRewarded { get; protected set; }

        public DateTime? RewardedAt { get; protected set; }

        public void ConfirmReward()
        {
            WasRewarded = true;
            RewardedAt = DateTime.UtcNow;
        }
    }
}
