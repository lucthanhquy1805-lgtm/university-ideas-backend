using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public DashboardRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetDashboardDataAsync()
        {
            var summary = new DashboardSummaryDto();

            // 1. KPI cũ
            summary.TotalIdeas = await _context.Ideas.CountAsync();
            summary.TotalUsers = await _context.Users.CountAsync();
            summary.TotalComments = await _context.Comments.CountAsync();
            summary.ActiveCategories = await _context.Categories.CountAsync(c => c.IsActive == true);

            // 2. KPI mới cho User Dashboard
            summary.ActiveTopicsCount = await _context.Topics.CountAsync(t => t.IsActive == true);
            summary.PopularIdeasCount = await _context.Ideas.CountAsync(i => i.ViewCount >= 50); // Ví dụ > 50 view là popular

            // 3. Biểu đồ (Giữ nguyên - Đã quá chuẩn)
            summary.IdeasByDepartment = await _context.Ideas
                .GroupBy(i => i.User.Department.Name)
                .Select(g => new DepartmentIdeaCountDto
                {
                    DepartmentName = g.Key,
                    IdeaCount = g.Count()
                })
                .OrderByDescending(d => d.IdeaCount)
                .ToListAsync();

            // 4. Lấy 3 bài viết mới nhất (Dữ liệu thật cho User Dashboard)
            summary.LatestIdeas = await _context.Ideas
                .OrderByDescending(i => i.CreatedAt)
                .Take(3)
                .Select(i => new IdeaDashboardSummaryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    AuthorName = i.IsAnonymous == true ? "Anonymous" : i.User.FullName,
                    DepartmentName = i.User.Department.Name,
                    CreatedAt = i.CreatedAt.GetValueOrDefault(),
                    Upvotes = _context.Reactions.Count(r => r.IdeaId == i.Id && r.ReactionType == 1),
                    CommentCount = _context.Comments.Count(c => c.IdeaId == i.Id)
                }).ToListAsync();

            // 5. Lấy 3 bài xem nhiều nhất
            summary.MostViewedIdeas = await _context.Ideas
                .OrderByDescending(i => i.ViewCount)
                .Take(3)
                .Select(i => new IdeaDashboardSummaryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    DepartmentName = i.User.Department.Name,
                    ViewCount = (int)i.ViewCount,
                    Upvotes = _context.Reactions.Count(r => r.IdeaId == i.Id && r.ReactionType == 1)
                }).ToListAsync();

            // 6. Hoạt động (Giữ nguyên cho Admin)
            summary.RecentActivities = await _context.ActivityLogs
                .OrderByDescending(a => a.CreatedAt).Take(5)
                .Select(a => new ActivityLogDto
                {
                    ActionType = a.ActionType,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt
                }).ToListAsync();

            return summary;
        }
    }
}