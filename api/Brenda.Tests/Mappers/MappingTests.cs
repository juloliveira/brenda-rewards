using AutoMapper;
using Brenda.Contracts.V1.Campaign;
using Brenda.Core;
using Brenda.Web.MappingProfiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Brenda.Tests.Mappers
{
    using Brenda.Core;
    using Brenda.Core.Identifiers;

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
            _campaign2 = new Campaign(_customer);
            _campaign3 = new Campaign(_customer);
        }

        [TestMethod]
        public void Configuration_IsValid()
        {
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Should_Map_CampaignOnGoing()
        {
            var mapper = _mapperConfiguration.CreateMapper();

            _campaign1.Definitions.ValidationStart = DateTime.UtcNow;
            _campaign1.Definitions.ValidationEnd = DateTime.UtcNow.AddDays(1);

            var campaignOnGoing = mapper.Map<CampaignOnGoing>(_campaign1);

            Assert.AreEqual(_campaign1.Id.ToString(), campaignOnGoing.Id);
            Assert.AreEqual(_campaign1.Customer.Id, campaignOnGoing.CustomerId);
        }

        [TestMethod]
        public void Should_Map_ChallengeOnGoing()
        {
            var mapper = _mapperConfiguration.CreateMapper();
            var challenge = new Campaign(_customer);
            challenge.Action = new Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;
            challenge.Definitions.ValidationStart = DateTime.UtcNow;
            challenge.Definitions.ValidationEnd = DateTime.UtcNow.AddDays(1);

            challenge.AddCampaign(_campaign1);
            challenge.AddCampaign(_campaign2);
            challenge.AddCampaign(_campaign3);

            var challengeOnGoing = mapper.Map<ChallengeOnGoing>(challenge);

            Assert.AreEqual(challenge.Id.ToString(), challengeOnGoing.Id);
            Assert.AreEqual(challenge.Customer.Id, challengeOnGoing.CustomerId);
            Assert.AreEqual(3, challengeOnGoing.Campaigns.Length);
        }
    }
}
