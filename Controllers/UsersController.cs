using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.Repositories;
using UniversityIdeas.API.DTOs;
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
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                // Đẩy thẳng xuống Repository xử lý
                await _userRepository.UpdateUserAsync(id, dto);

                return Ok(new { message = "User updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when updating User: " + ex.Message });
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
                // Lấy thông báo lỗi sâu nhất từ Entity Framework (thường chứa chi tiết về khóa ngoại)
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                // Kiểm tra nếu lỗi có chứa từ khóa liên quan đến ràng buộc (Constraint)
                if (realError.Contains("REFERENCE constraint") || realError.Contains("FOREIGN KEY"))
                {
                    return BadRequest(new { message = "\"This user cannot be deleted because they have posted an Idea or Comment. Please use the Edit feature and change the Status to 'Inactive' (Account locked)!" });
                }

                // Các lỗi khác
                return BadRequest(new { message = "Error when deleting User: " + realError });
            }
        }
    }
}