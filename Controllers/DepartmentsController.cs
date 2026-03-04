using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models; // Đổi tên namespace này nếu project của bạn tên khác

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UniversityIdeaDbContext _context;

        // Bơm (Inject) Database vào Controller
        public DepartmentsController(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            // Lấy toàn bộ danh sách phòng ban từ Database
            var departments = await _context.Departments.ToListAsync();

            return Ok(departments); // Trả về mã 200 (Thành công) cùng với dữ liệu JSON
        }
    }
} 