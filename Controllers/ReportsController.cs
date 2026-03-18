using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetReportSummary()
        {
            var data = await _reportRepository.GetReportSummaryAsync();
            return Ok(data);
        }
    }
}