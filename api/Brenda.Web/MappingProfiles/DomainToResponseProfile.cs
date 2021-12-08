using AutoMapper;
using Brenda.Contracts.V1.Campaign;
using Brenda.Contracts.V1.Requests;
using Brenda.Contracts.V1.Responses.Campaings;
using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Web.Models;
using Brenda.Utils;
using Brenda.Core.Identifiers;

namespace Brenda.Web.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Campaign, CampaignViewModel>();
            CreateMap<Campaign, CampaignForm>();

            CreateMap<Asset, AssetForm>();

            CreateMap<UrlAction, UrlActionViewModel>();

            CreateMap<Action, BaseEntity>();
            CreateMap<GeoRestriction, CampaignRestriction>();
            CreateMap<Quiz, QuizView>();
            CreateMap<Quiz, CampaignQuiz>();

            CreateMap<Customer, SettingsPost>()
                .ForMember(x => x.Logo, o => o.Ignore());

            CreateMap<GeoRestriction, Restriction>()
                .ForMember(x => x.Lat, o => o.MapFrom(src => src.Latitude))
                .ForMember(x => x.Lng, o => o.MapFrom(src => src.Longitude));


            CreateMap<Campaign, CampaignOnGoing>()
                .ForMember(x => x.Brand, o => o.Ignore()) // TODO: Precisa inserir a marca do cliente
                .ForMember(x => x.CustomerId, o => o.MapFrom(src => src.Customer.Id))
                .ForMember(x => x.CustomerName, o => o.MapFrom(src => src.Customer.Name))
                .ForMember(x => x.Resource, o => o.MapFrom(src => src.Asset.Resource))
                .ForMember(x => x.Action, o => o.MapFrom(src => src.Action.Tag))
                .ForMember(x => x.Restrictions, o => o.MapFrom(src => src.Definitions.CoordinatesAllowed))
                .ForMember(x => x.ValidationStart, o => o.MapFrom(src => src.Definitions.ValidationStart.Value.ToTimestamp()))
                .ForMember(x => x.ValidationEnd, o => o.MapFrom(src => src.Definitions.ValidationEnd.Value.ToTimestamp()));

            CreateMap<Campaign, ChallengeOnGoing>()
                .ForMember(x => x.Id, opt =>
                {
                    opt.PreCondition(campaign => Actions.IsChallenge(campaign));
                    opt.MapFrom(src => src.Id);
                })
                .ForMember(x => x.Brand, o => o.Ignore()) // TODO: Precisa inserir a marca do cliente
                .ForMember(x => x.CustomerId, o => o.MapFrom(src => src.Customer.Id))
                .ForMember(x => x.CustomerName, o => o.MapFrom(src => src.Customer.Name))
                .ForMember(x => x.Resource, o => o.MapFrom(src => src.Asset.Resource))
                .ForMember(x => x.Action, o => o.MapFrom(src => src.Action.Tag))
                .ForMember(x => x.Restrictions, o => o.MapFrom(src => src.Definitions.CoordinatesAllowed))
                .ForMember(x => x.ValidationStart, o => o.MapFrom(src => src.Definitions.ValidationStart.Value.ToTimestamp()))
                .ForMember(x => x.ValidationEnd, o => o.MapFrom(src => src.Definitions.ValidationEnd.Value.ToTimestamp()));
                
        }

    }
}
