using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Table;

namespace WebApplication1.Services
{

    public interface INewsService
    {
        Task<News> AddNews(News news);
        Task<List<News>> GetAllNews();
        Task<News> GetNewsById(int id);
        Task<bool> DeleteNews(int id);
        Task<News> UpdateNews(News news, int id);
    }

    public class NewsService : INewsService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<News> AddNews(News news)
        {
            var newAdd = new News(news.Id, news.Title, news.Description, news.ImagePath);
            _context.Add(newAdd);
            await _context.SaveChangesAsync();
            return newAdd;
        }

        public async Task<List<News>> GetAllNews()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<News> GetNewsById(int id)
        {
            return await _context.News.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteNews(int id)
        {
            var newsToDelete = await _context.News.FindAsync(id);
            if (newsToDelete == null)
                return false;

            _context.News.Remove(newsToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<News> UpdateNews(News news, int id)
        {
            var existingNews = await _context.News.FindAsync(id);
            if (existingNews == null)
            {
                return null;
            }

            existingNews.Title = news.Title;
            existingNews.Description = news.Description;
            existingNews.ImagePath = news.ImagePath;

            await _context.SaveChangesAsync();
            return existingNews;
        }
    }
}

