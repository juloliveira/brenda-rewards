using AutoMapper;
using Brenda.Contracts.V1.Responses;
using Brenda.Core;

namespace Brenda.Api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<RewardTicket, CampaignResponse>()
                .ForMember(x => x.Ticket, opt => opt.MapFrom(src => src.Id))
                //.ForMember(x => x.Type, opt => opt.MapFrom(src => src.Campaign.Type))
                .ForMember(x => x.Tag, opt => opt.MapFrom(src => src.Campaign.Tag))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Campaign.Title))
                .ForMember(x => x.ResourceUri, opt => opt.MapFrom<ResourceUriResolver>())
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<UserRewardTicket, RewardedTicketResponse>()
                .ForMember(x => x.Ticket, opt => opt.MapFrom(src => src.Ticket))
                .ForMember(x => x.Tag, opt => opt.MapFrom(src => src.Campaign.Tag))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<BrendaUser, UserDashboard>()
                .ForMember(x => x.UserId, o => o.MapFrom(src => src.Id))
                .ForMember(x => x.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(x => x.PointsBalance, o => o.MapFrom(src => src.PointsBalance))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
            
        }
    }

    public class ResourceUriResolver : IValueResolver<RewardTicket, CampaignResponse, string>
    {
        public string Resolve(RewardTicket source, CampaignResponse destination, string destMember, ResolutionContext context)
        {
            //if (source.Campaign.Type == CampaignFormat.Video)
            //    return source.Campaign.VideoDefs.Uri;

            return null;
        }
    }
}
