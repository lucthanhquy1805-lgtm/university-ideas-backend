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

            // 1. Lấy dữ liệu cho 3 thẻ thống kê trên cùng
            result.TotalTopics = await _context.Topics.CountAsync();
            result.ActiveTopics = await _context.Topics.CountAsync(t => t.IsActive);
            result.TotalIdeas = await _context.Ideas.CountAsync(); // Đếm tổng tất cả Idea

            // 2. Lấy danh sách cho bảng, kèm theo việc tự động đếm IdeaCount và lấy CategoryName
            var topics = await _context.Topics
                .Include(t => t.Category) // Sử dụng 'Include' để lấy 'CategoryName'
                .Select(t => new TopicItemDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    CategoryName = t.Category.Name, // Lấy tên danh mục
                    Description = t.Description,
                    IdeaCount = _context.Ideas.Count(i => i.TopicId == t.Id), // Tự động đếm ý tưởng theo ID chủ đề
                    Status = t.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync();

            result.Topics = topics;

            return result;
        }
    }
}