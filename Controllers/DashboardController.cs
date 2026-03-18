using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Repositories; 
namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var data = await _dashboardRepository.GetDashboardDataAsync();
            return Ok(data);
        }
    }
}