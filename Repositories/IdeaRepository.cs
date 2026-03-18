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

        public async Task<IEnumerable<IdeaDto>> GetAllIdeasAsync()
        {
            return await _context.Ideas
                .Select(i => new IdeaDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Content = i.Content,
                    // Xử lý ẩn danh: Nếu IsAnonymous = true thì đổi tên thành "Anonymous"
                    AuthorName = i.IsAnonymous == true ? "Anonymous" : i.User.FullName,
                    DepartmentName = i.User.Department.Name,
                    CategoryName = i.Category.Name,
                    ViewCount = i.ViewCount ?? 0,

                    // Tự động đếm số Like (ReactionType = 1) và Dislike (ReactionType = 2)
                    ThumbsUpCount = i.Reactions.Count(r => r.ReactionType == 1),
                    ThumbsDownCount = i.Reactions.Count(r => r.ReactionType == 2),

                    // Tự động đếm số lượng Comment
                    CommentCount = i.Comments.Count(),

                    CreatedAt = i.CreatedAt ?? DateTime.Now
                })
                .OrderByDescending(i => i.CreatedAt) // Sắp xếp bài mới nhất lên đầu
                .ToListAsync();
        }
    }
}