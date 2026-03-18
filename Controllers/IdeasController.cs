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

        [HttpGet]
        public async Task<IActionResult> GetAllIdeas()
        {
            var ideas = await _ideaRepository.GetAllIdeasAsync();
            return Ok(ideas);
        }
    }
}