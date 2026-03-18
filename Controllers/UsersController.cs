using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("page-data")]
        public async Task<IActionResult> GetPageData()
        {
            var data = await _userRepository.GetUserPageDataAsync();
            return Ok(data);
        }
    }
}