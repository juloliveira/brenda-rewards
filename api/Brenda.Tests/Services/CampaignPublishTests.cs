using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brenda.Contracts.V1.Campaign;
using Brenda.Core;
using Brenda.Core.Identifiers;
using Brenda.Core.Services;
using Brenda.Core.Validations;
using Brenda.Services;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brenda.Tests.Services
{
    [TestClass]
    public class CampaignPublishTests
    {

        private readonly Customer _customer;

        ICampaignPublisher _publisher;
        
        Mock<IBus> _busMock;
        Mock<ICampaignValidator> _validatorMock;
        Mock<IMapper> _mapperMock;

        Mock<PublishValidation> _publishValidationMock;

        public CampaignPublishTests()
        {
            _customer = new Customer("", "");
        }

        [TestInitialize]
        public void Setup()
        {
            _busMock = new Mock<IBus>();
            _validatorMock = new Mock<ICampaignValidator>();
            _mapperMock = new Mock<IMapper>();

            _publishValidationMock = new Mock<PublishValidation>();

            _publisher = new CampaignPublisher(_busMock.Object, _validatorMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Publish_Must_Validate()
        {
            var campaign = new Campaign(_customer);
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<Campaign>())).Returns(Task.FromResult(_publishValidationMock.Object));

            var resultValidation = _publisher.Publish(campaign).Result;

            _validatorMock.Verify(x => x.ValidateAsync(campaign));
            _publishValidationMock.Verify(x => x.IsValid);
        }

        [TestMethod]
        public void Publish_Valid_Must_Send_Message()
        {
            var campaign = new Campaign(_customer);
            _validatorMock.Setup(x => x.ValidateAsync(campaign)).Returns(Task.FromResult(_publishValidationMock.Object));
            _publishValidationMock.Setup(x => x.IsValid).Returns(true);

            var resultValidation = _publisher.Publish(campaign).Result;

            _mapperMock.Verify(x => x.Map<CampaignOnGoing>(It.IsAny<Campaign>()));
            _busMock.Verify(x => x.Publish(It.IsAny<CampaignOnGoing>(), It.IsAny<CancellationToken>()));
        }

        [TestMethod]
        public void Pusbli_Valid_Challenge_Must_Send_Messages()
        {
            var challenge = new Campaign(_customer);
            challenge.ActionId = new Guid(Actions.Challenge);

            challenge.AddCampaign(new Campaign(_customer));
            challenge.AddCampaign(new Campaign(_customer));
            challenge.AddCampaign(new Campaign(_customer));

            _validatorMock.Setup(x => x.ValidateAsync(challenge)).Returns(Task.FromResult(_publishValidationMock.Object));
            _publishValidationMock.Setup(x => x.IsValid).Returns(true);

            var resultValidation = _publisher.Publish(challenge).Result;

            _mapperMock.Verify(x => x.Map<ChallengeOnGoing>(challenge));
            _busMock.Verify(x => x.Publish(It.IsAny<ChallengeOnGoing>(), It.IsAny<CancellationToken>()));
        }
        
    }

    public sealed class AssertEx
    {
        public static void NoExceptionThrown<T>(System.Action a) where T : Exception
        {
            try
            {
                a();
            }
            catch (T)
            {
                Assert.Fail("Expected no {0} to be thrown", typeof(T).Name);
            }
        }
    }
}
