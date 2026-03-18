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

            summary.TotalIdeas = await _context.Ideas.CountAsync();
            summary.TotalUsers = await _context.Users.CountAsync();
            summary.TotalComments = await _context.Comments.CountAsync();
            summary.ActiveCategories = await _context.Categories.CountAsync();

            summary.IdeasByDepartment = await _context.Ideas
                .Include(i => i.User)
                .ThenInclude(u => u.Department)
                .GroupBy(i => i.User.Department.Name)
                .Select(g => new DepartmentIdeaCountDto
                {
                    DepartmentName = g.Key,
                    IdeaCount = g.Count()
                })
                .OrderByDescending(d => d.IdeaCount)
                .ToListAsync();

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