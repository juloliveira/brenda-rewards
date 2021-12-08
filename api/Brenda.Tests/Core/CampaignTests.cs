using Brenda.Core;
using Brenda.Core.Exceptions;
using Brenda.Core.Identifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brenda.Tests.Core
{
    [TestClass]
    public class CampaignTests
    {
        private readonly Customer _customer;

        public CampaignTests()
        {
            _customer = new Customer("ACME", "");
        }

        [TestMethod]
        public void Must_Be_Challenge_To_Add_Campaign()
        {
            var campaign = new Campaign(_customer);

            Assert.ThrowsException<ChallengeException>(() => campaign.AddCampaign(new Campaign(_customer)));
        }

        [TestMethod]
        public void Challenge_Cannot_Add_Campaign_Challenge()
        {
            var challenge = new Campaign(_customer);
            challenge.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;

            var campaign = new Campaign(_customer);
            campaign.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            campaign.ActionId = campaign.Action.Id;

            Assert.ThrowsException<ChallengeException>(() => challenge.AddCampaign(campaign));
        }

        [TestMethod]
        public void Challenge_Cannot_Add_Campaign_Different_Customer()
        {
            var challenge = new Campaign(_customer);
            challenge.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;

            var campaign = new Campaign(new Customer("", ""));
            campaign.Action = new Brenda.Core.Action(new Guid(Actions.Video));
            campaign.ActionId = campaign.Action.Id;
            campaign.Status = CampaignStatus.Pending;

            Assert.ThrowsException<ChallengeException>(() => challenge.AddCampaign(campaign));
        }

        [TestMethod]
        public void Challenge_Add_Campaign_Only_Status_Pending()
        {
            var challenge = new Campaign(_customer);
            challenge.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;

            var campaign = new Campaign(_customer);
            campaign.Status = CampaignStatus.Archived;

            Assert.ThrowsException<ChallengeException>(() => challenge.AddCampaign(campaign));
        }

        [TestMethod]
        public void Challenge_Can_Add_Campaign()
        {
            var challenge = new Campaign(_customer);
            challenge.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;

            var campaign = new Campaign(_customer);
            campaign.Action = new Brenda.Core.Action(new Guid(Actions.Video));
            campaign.ActionId = campaign.Action.Id;
            campaign.Status = CampaignStatus.Pending;

            challenge.AddCampaign(campaign);

            Assert.AreEqual(1, challenge.Campaigns.Count);
            Assert.AreEqual(true, Actions.IsVideo(challenge.Campaigns.ElementAt(0)), "Expected on campaign in campaigns collection");
        }

        [TestMethod]
        public void Challenge_Cannot_Add_Same_Campaign_Two_Times()
        {
            var challenge = new Campaign(_customer);
            challenge.Action = new Brenda.Core.Action(new Guid(Actions.Challenge));
            challenge.ActionId = challenge.Action.Id;

            var campaign = new Campaign(_customer);
            campaign.Action = new Brenda.Core.Action(new Guid(Actions.Video));
            campaign.ActionId = campaign.Action.Id;
            campaign.Status = CampaignStatus.Pending;

            challenge.AddCampaign(campaign);

            Assert.ThrowsException<ChallengeException>(() => challenge.AddCampaign(campaign));
        }
    }
}
