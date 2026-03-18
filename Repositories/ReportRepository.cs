using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public ReportRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<ReportSummaryDto> GetReportSummaryAsync()
        {
            var report = new ReportSummaryDto();

            // 1. Tính toán 3 thẻ KPI trên cùng
            report.TotalIdeas = await _context.Ideas.CountAsync();
            report.TotalComments = await _context.Comments.CountAsync();
            // Lấy danh sách ID người dùng duy nhất đã từng đăng Idea
            report.TotalContributors = await _context.Ideas.Select(i => i.UserId).Distinct().CountAsync();

            // 2. Thống kê theo từng Khoa (Department)
            var deptStats = await _context.Departments
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    // Đếm tổng số Ideas của các User thuộc Khoa này
                    IdeaCount = d.Users.SelectMany(u => u.Ideas).Count(),
                    // Đếm số lượng User thuộc Khoa này đã từng đăng Idea
                    ContributorCount = d.Users.Where(u => u.Ideas.Any()).Count()
                }).ToListAsync();

            // Tính tỷ lệ % và map sang DTO
            report.DepartmentStats = deptStats.Select(d => new DepartmentStatsDto
            {
                DepartmentName = d.DepartmentName,
                IdeaCount = d.IdeaCount,
                IdeaPercentage = report.TotalIdeas == 0 ? 0 : Math.Round((double)d.IdeaCount / report.TotalIdeas * 100, 1),
                ContributorCount = d.ContributorCount,
                ContributorPercentage = report.TotalContributors == 0 ? 0 : Math.Round((double)d.ContributorCount / report.TotalContributors * 100, 1)
            })
            .OrderByDescending(d => d.IdeaCount) // Xếp khoa có nhiều ý tưởng nhất lên đầu
            .ToList();

            // 3. Báo cáo ngoại lệ (Exception Reports)
            // - Ideas không có comment nào
            report.IdeasWithoutComments = await _context.Ideas
                .Where(i => !i.Comments.Any())
                .Select(i => new ExceptionItemDto
                {
                    Title = i.Title,
                    DepartmentName = i.User.Department.Name,
                    Date = i.CreatedAt ?? DateTime.Now
                }).ToListAsync();

            // - Ideas đăng ẩn danh
            report.AnonymousIdeas = await _context.Ideas
                .Where(i => i.IsAnonymous == true)
                .Select(i => new ExceptionItemDto
                {
                    Title = i.Title,
                    DepartmentName = i.User.Department.Name,
                    Date = i.CreatedAt ?? DateTime.Now
                }).ToListAsync();

            // - Comments đăng ẩn danh
            report.AnonymousComments = await _context.Comments
                .Where(c => c.IsAnonymous == true)
                .Select(c => new ExceptionItemDto
                {
                    Title = c.Idea.Title, // Tên của Idea bị comment ẩn danh
                    DepartmentName = c.User.Department.Name,
                    Date = c.CreatedAt ?? DateTime.Now
                }).ToListAsync();

            return report;
        }
    }
}