using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Services;
using WebApplication1.Table;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: api/News
        [HttpGet("GetAllNews")]
        public async Task<ActionResult<IEnumerable<News>>> GetAllNews()
        {
            var newsList = await _newsService.GetAllNews();
            return Ok(newsList);
        }

        // GET: api/News/{id}
        [HttpGet("GetNewsById{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            var news = await _newsService.GetNewsById(id);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        // POST: api/News
        [HttpPost("AddNews")]
        public async Task<ActionResult<News>> AddNews([FromBody] NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news = new News
            {
                Title = newsDto.Title,
                Description = newsDto.Description,
                ImagePath = newsDto.ImageUrl
            };

            var addedNews = await _newsService.AddNews(news);
            return CreatedAtAction(nameof(GetNewsById), new { id = addedNews.Id }, addedNews);
        }



        // PUT: api/News/{id}
        [HttpPut("UpdateNews{id}")]
        public async Task<ActionResult<News>> UpdateNews(int id, News news)
        {
            if (id != news.Id)
            {
                return BadRequest();
            }

            var updatedNews = await _newsService.UpdateNews(news, id);

            if (updatedNews == null)
            {
                return NotFound();
            }

            return Ok(updatedNews);
        }

        // DELETE: api/News/{id}
        [HttpDelete("DeletNews{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var deleted = await _newsService.DeleteNews(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    
}
