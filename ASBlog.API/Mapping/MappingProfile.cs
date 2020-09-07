using ASBlog.API.Models.Auth;
using ASBlog.API.Models.Blog;
using ASBlog.API.Resources;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<User, UserResource>();

            CreateMap<Comment, CommentResource>();

            CreateMap<Post, AllStoriesResource>()
                .ForMember(u => u.Content, opt => opt.MapFrom(src => SubStrContent(src.Content)))
                .ForMember(x => x.User, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));

            CreateMap<Post, StoryResource>()
                .ForMember(x => x.User, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(x => x.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(x => x.UpdateUser, opt => opt.MapFrom(src => src.UpdateUser.FirstName + " " + src.UpdateUser.LastName))
                .ForMember(x => x.Comments, opt => opt.MapFrom(src => src.Comments.ToList()));


            // Resource to Domain
            CreateMap<SaveStoryResource, Post>();
            CreateMap<SaveCommentResource, Comment>();
            CreateMap<RegisterResource, User>()
              .ForMember(u => u.UserName, opt => opt.MapFrom(ur => ur.Email));
        }

        private string SubStrContent(string str)
        {
            return str.Length > 150 ? str.Substring(0, 150) : str;
        }
    }
}
