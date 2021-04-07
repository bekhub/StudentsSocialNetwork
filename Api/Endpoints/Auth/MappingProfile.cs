using AutoMapper;
using Core.Entities;

namespace Api.Endpoints.Auth
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request.Register, ApplicationUser>(MemberList.None)
                .ForMember(x => x.UserName, 
                    expression => expression.MapFrom(x => x.Username));
        }
    }
}
