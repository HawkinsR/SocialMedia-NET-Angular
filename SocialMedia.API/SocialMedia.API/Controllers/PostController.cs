using SocialMedia.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data;
using SocialMedia.Models;
using System.Security.Cryptography.X509Certificates;

namespace SocialMedia.API.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly IRepository _repo;
        private readonly ILogger<PostController> _logger;

        public PostController(IRepository repo, ILogger<PostController> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPosts()
        {
            try
            {
                return Ok(await _repo.GetAllPostsAsync());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<Post>> UpsertPost([FromBody] PostDTO newPost)
        {
            if (newPost.id == 0)
            {
                try
                {
                    Post resp = await _repo.CreateNewPostAndReturnPost(newPost);
                    return Ok(resp);
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(new Post());
            }
        }
    }
}
