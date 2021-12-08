using AutoMapper;
using Brenda.Core;
using Brenda.Web.MappingProfiles;
using Brenda.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Brenda.Tests.Web
{
    [TestClass]
    public class MappingTests
    {
        private readonly Customer _customer;
        private readonly Campaign _campaign1;
        private readonly Campaign _campaign2;
        private readonly Campaign _campaign3;
        private readonly MapperConfiguration _mapperConfiguration;

        public MappingTests()
        {
            _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseProfile>());

            _customer = new Customer("CUSTOMER NAME", "");
            _campaign1 = new Campaign(_customer);
            _campaign1.Definitions.ValidationStart = DateTime.UtcNow;
            _campaign1.Definitions.ValidationEnd = DateTime.UtcNow.AddDays(1);

            _campaign2 = new Campaign(_customer);
            _campaign2.Definitions.ValidationStart = DateTime.UtcNow;
            _campaign2.Definitions.ValidationEnd = DateTime.UtcNow.AddDays(1);

            _campaign3 = new Campaign(_customer);
            _campaign3.Definitions.ValidationStart = DateTime.UtcNow;
            _campaign3.Definitions.ValidationEnd = DateTime.UtcNow.AddDays(1);
        }

        [TestMethod]
        public void Configuration_IsValid()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CampaignViewModel()
        {
            var mapper = _mapperConfiguration.CreateMapper();

            var viewModel = mapper.Map<CampaignViewModel>(_campaign1);

            Assert.AreEqual(_campaign1.Id, viewModel.Id);
        }
    }
}
