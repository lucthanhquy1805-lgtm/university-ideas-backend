using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UniversityIdeaDbContext _context;

        public AuthController(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        // Tạo 1 cái DTO nhỏ để hứng Email và Password từ form React gửi lên
        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                // Tìm người dùng trong Database dựa vào Email
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

                // Kiểm tra xem User có tồn tại không và Password có khớp không
                // (Lưu ý: Vì đang code demo nên ta so sánh trực tiếp với cột PasswordHash. Sau này đem chạy thực tế ta sẽ mã hóa mật khẩu sau)
                if (user == null || user.PasswordHash != dto.Password)
                {
                    return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác!" }); // Lỗi 401
                }

                // Nếu đúng, cấp "Thẻ từ" (Trả về thông tin User nhưng GIẤU password đi)
                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    departmentId = user.DepartmentId,
                    roleId = user.RoleId // Cột này siêu quan trọng để biết ai là Admin!
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi server: " + ex.Message);
            }
        }
    }
}