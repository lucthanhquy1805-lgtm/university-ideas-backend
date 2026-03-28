using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Repositories;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly UniversityIdeaDbContext _context; // THÊM: Gọi trực tiếp DbContext cho các truy vấn nhỏ

        // Cập nhật Constructor để nhận cả 2
        public IdeasController(IIdeaRepository ideaRepository, UniversityIdeaDbContext context)
        {
            _ideaRepository = ideaRepository;
            _context = context;
        }

        // =========================================================
        // 1. API CHÍNH: Lấy danh sách Ideas (Code của bạn - Giữ nguyên)
        // =========================================================
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

        // =========================================================
        // 2. CÁC API PHỤ TRỢ: Cung cấp dữ liệu cho ô Dropdown bên React
        // =========================================================

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetCategoriesLookup()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true) // Chỉ lấy danh mục đang hoạt động
                .Select(c => new LookupDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetTopicsLookup([FromQuery] int? categoryId)
        {
            var query = _context.Topics.Where(t => t.IsActive == true);

            // LOGIC QUAN TRỌNG: Nếu React gửi lên CategoryId, chỉ lọc Topic thuộc Category đó
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            return await query
                .Select(t => new LookupDto { Id = t.Id, Name = t.Name })
                .ToListAsync();
        }

        [HttpGet("departments")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetDepartmentsLookup()
        {
            return await _context.Departments
                .Select(d => new LookupDto { Id = d.Id, Name = d.Name })
                .ToListAsync();
        }
    }
}