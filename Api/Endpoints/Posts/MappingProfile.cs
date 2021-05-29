using System;
using System.Linq;
using AutoMapper;
using Core.Entities;

namespace Api.Endpoints.Posts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request.Create, Post>(MemberList.None)
                .ForMember(x => x.CreatedAt, 
                    expression => expression.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.UpdatedAt, 
                    expression => expression.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.IsActive, 
                    expression => expression.MapFrom(x => true))
                .ForMember(x => x.IsDraft, 
                    expression => expression.MapFrom(x => false))
                .ForMember(x => x.Pictures, expression => expression.Ignore())
                .ForMember(x => x.Tags, expression => expression.Ignore());

            CreateMap<Post, Response.Create>(MemberList.None)
                .ForMember(x => x.Username,
                    expression => expression.MapFrom(x => x.User.UserName))
                .ForMember(x => x.Pictures,
                    expression => expression.MapFrom(x => x.Pictures.Select(z => z.Url)))
                .ForMember(x => x.Tags,
                    expression => expression.MapFrom(x => x.Tags.Select(z => z.Name)));

            CreateMap<Post, Response.Update>(MemberList.None)
                .IncludeBase<Post, Response.Create>();

            CreateMap<Post, Response.Details>(MemberList.None)
                .IncludeBase<Post, Response.Create>()
                .ForMember(x => x.UserPictureUrl, 
                    expression => expression.MapFrom(x => x.User.ProfilePictureUrl));
            
            CreateMap<PostLike, Response.Likes>(MemberList.None)
                .ForMember(x => x.Username, 
                    expression => expression.MapFrom(x => x.User.UserName))
                .ForMember(x => x.ProfilePictureUrl, 
                    expression => expression.MapFrom(x => x.User.ProfilePictureUrl));
            
            CreateMap<Post, Response.List>(MemberList.None)
                .IncludeBase<Post, Response.Details>();
        }
    }
}
