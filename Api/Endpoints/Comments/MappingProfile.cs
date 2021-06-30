using System;
using AutoMapper;
using Core.Entities;

namespace Api.Endpoints.Comments
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request.Create, Comment>(MemberList.None)
                .ForMember(x => x.CreatedAt, 
                    expression => expression.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.UpdatedAt, 
                    expression => expression.MapFrom(x => DateTime.UtcNow));

            CreateMap<Comment, Response.ListComment>(MemberList.None)
                .ForMember(x => x.Username,
                    expression => expression.MapFrom(x => x.User.UserName))
                .ForMember(x => x.UserProfile,
                    expression => expression.MapFrom(x => x.User.ProfilePictureUrl))
                .ForMember(x => x.Replies, expression => expression.Ignore());
        }
    }
}
