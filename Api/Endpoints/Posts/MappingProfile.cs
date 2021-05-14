using AutoMapper;
using Core.Entities;

namespace Api.Endpoints.Posts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request.CreatePost, Post>()
                .ForMember(p => p.Body, option => option.Ignore())
                .ForMember(p => p.IsDraft, option => option.Ignore())
                .ForMember(p => p.UserId, option => option.Ignore())
                .ForMember(p => p.Pictures, option => option.Ignore())
                .ForMember(p => p.Tags, option => option.Ignore())
                ;
        }
    }
}