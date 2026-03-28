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

            // 1. Lấy các chỉ số KPI
            summary.TotalIdeas = await _context.Ideas.CountAsync();
            summary.TotalUsers = await _context.Users.CountAsync();
            summary.TotalComments = await _context.Comments.CountAsync();

            // Đã tối ưu: Chỉ đếm các danh mục đang Active
            summary.ActiveCategories = await _context.Categories.CountAsync(c => c.IsActive == true);

            // 2. Dữ liệu cho Biểu đồ (Ideas by Department)
            summary.IdeasByDepartment = await _context.Ideas
                // Đã tối ưu: Bỏ Include, EF Core sẽ tự động JOIN dựa vào Navigation Properties
                .GroupBy(i => i.User.Department.Name)
                .Select(g => new DepartmentIdeaCountDto
                {
                    DepartmentName = g.Key,
                    IdeaCount = g.Count()
                })
                .OrderByDescending(d => d.IdeaCount)
                .ToListAsync();

            // 3. Dữ liệu Hoạt động gần đây (Recent Activities)
            summary.RecentActivities = await _context.ActivityLogs
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new ActivityLogDto
                {
                    ActionType = a.ActionType,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return summary;
        }
    }
}