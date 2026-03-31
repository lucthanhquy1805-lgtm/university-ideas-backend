using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public TopicRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<TopicPageDto> GetTopicPageDataAsync()
        {
            var result = new TopicPageDto();
            //3 thẻ 
            result.TotalTopics = await _context.Topics.CountAsync();
            result.ActiveTopics = await _context.Topics.CountAsync(t => t.IsActive);
            result.TotalIdeas = await _context.Ideas.CountAsync(); 

            // Lấy danh sách 
            var topics = await _context.Topics
                .Include(t => t.Category) 
                .Select(t => new TopicItemDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name, 
                    Description = t.Description,
                    IdeaCount = _context.Ideas.Count(i => i.TopicId == t.Id), 
                    Status = t.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync();

            result.Topics = topics;

            return result;
        }

        public async Task CreateTopicAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }
    }
}