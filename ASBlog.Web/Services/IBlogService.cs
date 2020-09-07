using ASBlog.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Services
{
    public interface IBlogService
    {
        Task<bool> AddComment(CommentRq commentRq);
        Task<bool> ApproveStory(int id);
        Task<bool> CreateStory(StoryRq storyRq);
        Task<bool> DeleteStory(int id);
        Task<List<StoryRs>> GetAllStoriesAsync();
        Task<List<StoryRs>> GetAllStoriesByStatusAsync(int status);
        Task<StoryRs> GetStory(int id);
        Task<StoryRs> UpdateStory(int id, StoryRq storyRq);
    }
}
