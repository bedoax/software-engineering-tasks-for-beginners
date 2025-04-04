using BackendTask6.Data;
using BackendTask6.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackendTask6.Controllers
{
    [ApiController]
   [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncPost()
        {
            using (var httpClinet = new HttpClient())
            {
                var respose = await httpClinet.GetAsync("https://jsonplaceholder.typicode.com/posts");
                if (!respose.IsSuccessStatusCode)
                {
                    return StatusCode((int)respose.StatusCode, "Error fetching posts");
                }
                var content = await respose.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<List<Post>>(content);

                if (post == null || post.Count == 0)
                {
                    return BadRequest("No posts found");
                }
                var user = await _context.Users.ToListAsync();
            
                foreach (var p in post)
                {
                    bool isExist = await _context.Posts.AnyAsync(x => x.id == p.id);
                    if (!isExist)
                    {
                        p.id = 0; // Reset the ID to avoid conflicts
                        _context.Posts.Add(p);
                        
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Posts are synced sucessffully");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            try
            {
                if (post == null) return BadRequest("Post cannot be null");

                post = new Post
                {
                    title = post.title,
                    body = post.body,
                    approved = false,
                    userId = post.userId
                };
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPostById), new { id = post.id }, post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [Authorize(Roles = "Reviewer")]
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApprovePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            post.approved = true;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return Ok(post);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedPosts()
        {
            var posts = await _context.Posts.Where(p => p.approved).ToListAsync();
            return Ok(posts);
        }
        [Authorize(Roles = "Admin,Reviewer")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
        {
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null) return NotFound();

            existingPost.title = post.title;
            existingPost.body = post.body;
            existingPost.approved = post.approved;

            _context.Posts.Update(existingPost);
            await _context.SaveChangesAsync();
            return Ok(existingPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Ok(post);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            if (posts == null || posts.Count == 0) return NotFound("No posts found");
            _context.Posts.RemoveRange(posts);
            await _context.SaveChangesAsync();
            return Ok("All posts deleted successfully");
        }
    }
}
