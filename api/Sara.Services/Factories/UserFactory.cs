using Sara.Contracts.Security;
using Sara.Core;
using Sara.Core.Factories;
using Sara.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sara.Services.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IIncomes _incomes;
        private readonly ISexualities _sexualities;
        private readonly IEducationLevels _educationLevels;
        private readonly IGenders _genders;

        public UserFactory(
            IIncomes incomes,
            ISexualities sexualities,
            IEducationLevels educationLevels,
            IGenders genders)
        {
            _incomes = incomes;
            _sexualities = sexualities;
            _educationLevels = educationLevels;
            _genders = genders;
        }

        public async Task<SaraUser> Create(UserRegister userRegister)
        {
            var income = await _incomes.GetByIdAsync(userRegister.IncomeId.Value);
            var sexuality = await _sexualities.GetByIdAsync(userRegister.SexualityId.Value);
            var educationLevel = await _educationLevels.GetByIdAsync(userRegister.EducationLevelId.Value);
            var genderIdentity = await _genders.GetByIdAsync(userRegister.GenderIdentityId.Value);

            var newUser = new SaraUser(
                userRegister.Email.Trim(), 
                userRegister.Document,
                userRegister.PhoneNumber,
                userRegister.Birthdate.Value,
                userRegister.Sex,
                genderIdentity,
                sexuality,
                income,
                educationLevel);

            return newUser;
        }
    }
}
