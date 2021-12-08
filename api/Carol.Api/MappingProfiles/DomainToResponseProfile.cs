using AutoMapper;
using Brenda.Utils;
using Carol.Api.Model;
using Carol.Core;

namespace Carol.Api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Transactions, o => o.Ignore());
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(x => x.CreatedAt, o => o.MapFrom(src => src.CreatedAt.ToTimestamp()));

        }
    }

}
