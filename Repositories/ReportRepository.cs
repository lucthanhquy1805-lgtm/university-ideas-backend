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

      
            report.TotalIdeas = await _context.Ideas.CountAsync();
            report.TotalComments = await _context.Comments.CountAsync();
      
            report.TotalContributors = await _context.Ideas.Select(i => i.UserId).Distinct().CountAsync();

         
            var deptStats = await _context.Departments
                .Select(d => new
                {
                    DepartmentName = d.Name,
                 
                    IdeaCount = d.Users.SelectMany(u => u.Ideas).Count(),
                   
                    ContributorCount = d.Users.Where(u => u.Ideas.Any()).Count()
                }).ToListAsync();

            
            report.DepartmentStats = deptStats.Select(d => new DepartmentStatsDto
            {
                DepartmentName = d.DepartmentName,
                IdeaCount = d.IdeaCount,
                IdeaPercentage = report.TotalIdeas == 0 ? 0 : Math.Round((double)d.IdeaCount / report.TotalIdeas * 100, 1),
                ContributorCount = d.ContributorCount,
                ContributorPercentage = report.TotalContributors == 0 ? 0 : Math.Round((double)d.ContributorCount / report.TotalContributors * 100, 1)
            })
            .OrderByDescending(d => d.IdeaCount) 
            .ToList();

            
         
            report.IdeasWithoutComments = await _context.Ideas
                .Where(i => !i.Comments.Any())
                .Select(i => new ExceptionItemDto
                {
                    Title = i.Title,
                    DepartmentName = i.User.Department.Name,
                    Date = i.CreatedAt ?? DateTime.Now
                }).ToListAsync();

   
            report.AnonymousIdeas = await _context.Ideas
                .Where(i => i.IsAnonymous == true)
                .Select(i => new ExceptionItemDto
                {
                    Title = i.Title,
                    DepartmentName = i.User.Department.Name,
                    Date = i.CreatedAt ?? DateTime.Now
                }).ToListAsync();

        
            report.AnonymousComments = await _context.Comments
                .Where(c => c.IsAnonymous == true)
                .Select(c => new ExceptionItemDto
                {
                    Title = c.Idea.Title, 
                    DepartmentName = c.User.Department.Name,
                    Date = c.CreatedAt ?? DateTime.Now
                }).ToListAsync();

            return report;
        }
    }
}