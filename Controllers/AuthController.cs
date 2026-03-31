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
            public string? Email { get; set; }
            public string? Password { get; set; }
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
        // =========================================================
        // 1. DTO hứng dữ liệu đăng ký từ form React gửi lên
        // =========================================================
        public class RegisterDto
        {
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public int DepartmentId { get; set; }
        }

        // =========================================================
        // 2. API Xử lý Đăng ký
        // =========================================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                // Bước A: Kiểm tra xem Email này đã có ai dùng để đăng ký chưa
                var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (emailExists)
                {
                    return BadRequest("Email này đã được sử dụng trong hệ thống!");
                }

                // Bước B: Tạo tài khoản mới
                var newUser = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PasswordHash = dto.Password, // Ở đồ án ta lưu thẳng mật khẩu
                    DepartmentId = dto.DepartmentId,

                    // 🔥 QUAN TRỌNG: TỰ ĐỘNG GẮN QUYỀN STAFF (4) CHO NGƯỜI MỚI ĐĂNG KÝ
                    RoleId = 4,

                    IsActive = true
                };

                // Bước C: Lưu vào Database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đăng ký tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi server: " + ex.Message);
            }
        }
    }
}