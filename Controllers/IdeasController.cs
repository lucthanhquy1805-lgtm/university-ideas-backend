using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IIdeaRepository _ideaRepository;

        public IdeasController(IIdeaRepository ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }

        // Dùng [FromQuery] để nhận tham số từ URL
        [HttpGet]
        public async Task<IActionResult> GetAllIdeas(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] int? topicId,
            [FromQuery] int? departmentId,
            [FromQuery] string? sortBy)
        {
            var ideas = await _ideaRepository.GetAllIdeasAsync(search, categoryId, topicId, departmentId, sortBy);
            return Ok(ideas);
        }
    }
}