using ASBlog.API.Models.Auth;
using ASBlog.API.Models.Blog;
using ASBlog.API.Resources;
using ASBlog.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BlogController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("Story/GetAllStories")]
        public async Task<IActionResult> GetAllStories()
        {
            var stories = await _unitOfWork.Posts.GetAllStoriesByStatus(1);

            var result = _mapper.Map<List<Post>, List<AllStoriesResource>>(stories.ToList());

            return Ok(result);
        }

        [HttpGet("Story/GetAllStoriesByStatus/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStoriesByStatus(int status)
        {
            var stories = await _unitOfWork.Posts.GetAllStoriesByStatus(status);

            var result = _mapper.Map<List<Post>, List<AllStoriesResource>>(stories.ToList());

            return Ok(result);
        }

        [HttpGet("Story/{id}")]
        public async Task<IActionResult> GetStory(int id)
        {
            var story = await _unitOfWork.Posts.GetStoryWithComments(id);

            if (story == null)
                return NotFound();

            var mappedStory = _mapper.Map<Post, StoryResource>(story);

            return Ok(mappedStory);
        }

        [HttpPost("Story/Create")]
        [Authorize(Roles = "Admin,Writer")]
        public async Task<IActionResult> CreateStory(SaveStoryResource saveStoryResource)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByIdAsync(userId);

            var story = _mapper.Map<SaveStoryResource, Post>(saveStoryResource);

            story.CreatedDate = DateTime.Now;
            story.Status = 1 /*User.IsInRole("Admin") ? 1 : 0*/;
            story.User = user;

            await _unitOfWork.Posts.AddAsync(story);

            await _unitOfWork.CommitAsync();

            return Created(string.Empty, string.Empty);
        }


        [HttpPut("Story/Update/{id}")]
        [Authorize(Roles = "Admin,Writer")]
        public async Task<IActionResult> UpdateStory(int id, [FromBody]SaveStoryResource saveStoryResource)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByIdAsync(userId);

            var story = _mapper.Map<SaveStoryResource, Post>(saveStoryResource);

            var storyToUpdate = await _unitOfWork.Posts.GetByIdAsync(id);

            storyToUpdate.Title = story.Title;
            storyToUpdate.Content = story.Content;
            storyToUpdate.UpdateDate = DateTime.Now;
            storyToUpdate.UpdateUser = user;


            await _unitOfWork.CommitAsync();

            var UpdatedStory = await _unitOfWork.Posts.GetStoryWithComments(id);

            var mappedStory = _mapper.Map<Post, StoryResource>(UpdatedStory);

            return Ok(mappedStory);
        }

        [HttpPut("Story/Approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveStory(int id)
        {
            var storyToUpdate = await _unitOfWork.Posts.GetByIdAsync(id);

            storyToUpdate.Status = 1;

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpDelete("Story/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStory(int id)
        {
            var story = await _unitOfWork.Posts.GetByIdAsync(id);

            _unitOfWork.Posts.Remove(story);

            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpPost("Comment/Create")]
        public async Task<IActionResult> AddComment(SaveCommentResource saveCommentResource)
        {
            var comment = _mapper.Map<SaveCommentResource, Comment>(saveCommentResource);

            comment.CreatedDate = DateTime.Now;
            comment.Status = 1;

            await _unitOfWork.Comments.AddAsync(comment);

            await _unitOfWork.CommitAsync();

            return Created(String.Empty, String.Empty);
        }





    }
}
