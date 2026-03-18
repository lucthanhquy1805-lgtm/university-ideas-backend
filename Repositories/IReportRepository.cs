using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IReportRepository
    {
        Task<ReportSummaryDto> GetReportSummaryAsync();
    }
}