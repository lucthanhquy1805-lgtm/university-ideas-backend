using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            try
            {
                await _userRepository.CreateUserAsync(newUser);
                return Ok(new { message = "User added successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when adding User: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest(new { message = "IDs don't match!" });

            try
            {
                await _userRepository.UpdateUserAsync(updatedUser);
                return Ok(new { message = "User update successful!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when updating User:" + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
                return Ok(new { message = "User deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when deleting User: " + ex.Message });
            }
        }
    }
}