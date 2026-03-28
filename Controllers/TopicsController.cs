using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;

        public TopicsController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }


        [HttpGet("page-data")]
        public async Task<IActionResult> GetPageData()
        {
            var data = await _topicRepository.GetTopicPageDataAsync();
            return Ok(data);
        }
    }
}