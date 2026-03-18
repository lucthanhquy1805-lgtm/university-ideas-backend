using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public IdeaRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdeaDto>> GetAllIdeasAsync(string? search, int? categoryId, int? topicId, int? departmentId, string? sortBy)
        {
            // 1. Lấy toàn bộ Query gốc ban đầu
            var query = _context.Ideas.AsQueryable();

            // 2. LỌC: Gõ chữ vào thanh Tìm kiếm (Search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.Title.Contains(search) || i.Content.Contains(search));
            }

            // 3. LỌC: Chọn từ các Dropdown
            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(i => i.CategoryId == categoryId);

            if (topicId.HasValue && topicId > 0)
                query = query.Where(i => i.TopicId == topicId);

            if (departmentId.HasValue && departmentId > 0)
                query = query.Where(i => i.User.DepartmentId == departmentId);

            // 4. Transform sang DTO
            var ideasQuery = query.Select(i => new IdeaDto
            {
                Id = i.Id,
                Title = i.Title,
                Content = i.Content,
                AuthorName = i.IsAnonymous == true ? "Anonymous" : i.User.FullName,
                DepartmentName = i.User.Department.Name,
                CategoryName = i.Category.Name,
                TopicName = i.Topic != null ? i.Topic.Name : null, // An toàn lấy tên Topic
                ViewCount = i.ViewCount ?? 0,
                ThumbsUpCount = i.Reactions.Count(r => r.ReactionType == 1),
                ThumbsDownCount = i.Reactions.Count(r => r.ReactionType == 2),
                CommentCount = i.Comments.Count(),
                CreatedAt = i.CreatedAt ?? DateTime.Now
            });

            // 5. SẮP XẾP: Dựa vào nút bấm (Latest, Most Viewed, Most Popular)
            ideasQuery = sortBy switch
            {
                "Most Viewed" => ideasQuery.OrderByDescending(i => i.ViewCount),
                "Most Popular" => ideasQuery.OrderByDescending(i => i.ThumbsUpCount),
                _ => ideasQuery.OrderByDescending(i => i.CreatedAt) // Mặc định là Latest (Mới nhất)
            };

            return await ideasQuery.ToListAsync();
        }
    }
}