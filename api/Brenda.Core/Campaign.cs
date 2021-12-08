using Brenda.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Brenda.Core
{
    public class Campaign : Entity
    {
        public readonly List<Campaign> _campaigns;
        public readonly List<UrlAction> _urlActions;

        protected Campaign() { }

        public Campaign(Customer customer)
        {
            CustomerId = customer.Id;
            Customer = customer;
            Status = CampaignStatus.Pending;
            
            Definitions = new CampaignDefinitions();
        }

        #region Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public double Reward { get; set; }
        public virtual double Balance { get; protected set; }
        public string JsonOnGoing { get; protected set; }
        public CampaignStatus Status { get; set; }
        public CampaignDefinitions Definitions { get; protected set; }
        #endregion

        #region Customer
        public Guid CustomerId { get; set; }
        public Customer Customer { get; protected set; }
        #endregion

        #region Action
        public virtual Guid ActionId { get; set; }
        public Action Action { get; set; }

        #endregion

        #region Asset
        public Guid? AssetId { get; set; }
        public Asset Asset { get; set; }
        #endregion

        public Guid? ChallengeId { get; private set; }
        public Campaign Challenge { get; private set; }

        public virtual IReadOnlyCollection<Campaign> Campaigns => _campaigns?.AsReadOnly();

        public virtual IReadOnlyCollection<UrlAction> UrlActions => _urlActions?.AsReadOnly();

        #region Methods
        public void SetDefinition(CampaignDefinitions definitions)
        {
            Definitions = definitions;
        }

        public virtual void PutOnGoing(string json)
        {
            JsonOnGoing = json;
            Status = CampaignStatus.OnGoing;
        }

        public bool IsExpired()
        {
            return Balance <= 0 
                || Balance < Reward
                || Definitions.WithinExpirationDate();
        }

        public bool IsCoordinateAllowed(double latitude, double longitude)
        {
            if (!Definitions.ValidateGeoLocation)
                return true;

            foreach (var coordinateAllowed in Definitions.CoordinatesAllowed)
            {
                var R = 6371;
                var dLat = ToRadians(coordinateAllowed.Latitude - latitude);
                var dLon = ToRadians(coordinateAllowed.Longitude - longitude);
                var a =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(latitude)) * Math.Cos(ToRadians(coordinateAllowed.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var distanceBetweenPoints = R * c; // Distance in KM

                if (coordinateAllowed.Radius < distanceBetweenPoints)
                    return false;
            }

            return true;
        }

        public void DepositPoints(int depositPoints)
        {
            Balance = depositPoints;
        }

        public GeoRestriction AddRestriction(Contracts.V1.Requests.Campaigns.Restriction restriction)
        {
            return Definitions.AddGeoPermition(restriction.Radius, restriction.Lat, restriction.Lng);
        }

        public void AddCampaign(Campaign campaign)
        {
            if (!Identifiers.Actions.IsChallenge(this)) throw new ChallengeException("Campanha deve ser Challenge para adicionar outras campanhas.");
            if (Identifiers.Actions.IsChallenge(campaign)) throw new ChallengeException("Você não pode adicionar uma Challenge em uma Challenge.");
            if (campaign.Status != CampaignStatus.Pending) throw new ChallengeException("Não é permitido adicionar campanhas em andamento ou arquivadas.");
            if (!this.Customer.Equals(campaign.Customer)) throw new ChallengeException("Campanha inválida.");
            if (_campaigns.Any(item => item.Equals(campaign))) throw new ChallengeException("Campanha já está na lista de campanhas.");

            // TODO: Resolver definitions orfãs que ficam no banco
            campaign.SetDefinition(this.Definitions);
            campaign.AddChallenge(this);
            _campaigns.Add(campaign);
        }

        protected internal void AddChallenge(Campaign campaign)
        {
            ChallengeId = campaign.Id;
            Challenge = campaign;
        }
        #endregion

        #region Helpers
        internal void AddBalance(double value)
        {
            Balance += value;
        }

        internal double GetRewardFromPointsBalance()
        {
            Balance -= Reward;
            return Reward;
        }

        double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public UrlAction AddUrlAction()
        {
            var urlAction = new UrlAction(this);
            _urlActions.Add(urlAction);
            return urlAction;
        }
        #endregion
    }

    public class UrlAction : Entity
    {
        protected UrlAction() { }

        public UrlAction(Campaign campaign)
        {
            this.Campaign = campaign;
        }

        public string Url { get; protected set; }
        public Campaign Campaign { get; protected set; }

        public void SetUrl(string value)
        {
            if (!Regex.IsMatch(value, "^http(s)??://?(www.)?((uau.tw/)|(youtu.be/))([a-zA-Z0-9-_])+"))
                throw new ArgumentException("URL inválida.");

            this.Url = value;
        }
    }
}
