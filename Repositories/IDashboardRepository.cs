using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDto> GetDashboardDataAsync();
    }
}