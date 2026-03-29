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
        private readonly UniversityIdeaDbContext _context;
        private readonly IWebHostEnvironment _env;

        public IdeasController(IIdeaRepository ideaRepository, UniversityIdeaDbContext context, IWebHostEnvironment env)
        {
            _ideaRepository = ideaRepository;
            _context = context;
            _env = env;
        }

        // 1. Lấy danh sách Ideas
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

       
        [HttpGet("{id}")]
       
        public async Task<IActionResult> GetIdeaById(int id)
        {
            // Include đầy đủ để lấy tên Category, Topic, User, Department
            var idea = await _context.Ideas
                .Include(i => i.Category)
                .Include(i => i.Topic)
                .Include(i => i.User)
                    .ThenInclude(u => u.Department)
                // Nếu sau này bạn muốn đếm Like thật, hãy Include thêm Reactions và Comments ở đây
                .FirstOrDefaultAsync(i => i.Id == id);

            if (idea == null) return NotFound("Không tìm thấy ý tưởng này!");

            // Trả về dữ liệu
            return Ok(new
            {
                id = idea.Id,
                title = idea.Title,
                content = idea.Content,
                createdAt = idea.CreatedAt,
                viewCount = idea.ViewCount,

                // FIX LỖI 1, 2, 3: Vì bảng Idea không có cột Count, ta tạm để 0 
                // (Sau này làm chức năng Like/Comment xong ta sẽ viết code đếm thật sau)
                thumbsUpCount = 0,
                thumbsDownCount = 0,
                commentCount = 0,

                filePath = idea.FilePath,
                isAnonymous = idea.IsAnonymous,
                categoryId = idea.CategoryId,
                categoryName = idea.Category?.Name,
                topicId = idea.TopicId,
                topicName = idea.Topic?.Name,

                // FIX LỖI 4 (CS0266): Phải so sánh == true vì IsAnonymous là bool?
                authorName = (idea.IsAnonymous == true) ? "Anonymous" : idea.User?.FullName,
                departmentName = idea.User?.Department?.Name
            });
        }

        // 2. CÁC API PHỤ TRỢ cho Dropdown
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetCategoriesLookup()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true)
                .Select(c => new LookupDto { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<LookupDto>>> GetTopicsLookup([FromQuery] int? categoryId)
        {
            var query = _context.Topics.Where(t => t.IsActive == true);
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

        // 3. THÊM Ý TƯỞNG (Kèm file)
        [HttpPost]
        public async Task<IActionResult> AddIdea([FromForm] CreateIdeaDto dto)
        {
            try
            {
                var newIdea = new Idea
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CategoryId = dto.CategoryId,
                    TopicId = dto.TopicId,
                    UserId = dto.UserId,
                    IsAnonymous = dto.IsAnonymous,
                    AcademicYearId = dto.AcademicYearId,
                    CreatedAt = DateTime.Now,
                    ViewCount = 0
                };

                if (dto.File != null && dto.File.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.File.FileName;
                    string filePathInServer = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePathInServer, FileMode.Create))
                    {
                        await dto.File.CopyToAsync(fileStream);
                    }
                    newIdea.FilePath = "/uploads/" + uniqueFileName;
                }

                _context.Ideas.Add(newIdea);
                await _context.SaveChangesAsync();
                return Ok(newIdea);
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest("Lỗi khi thêm Idea/Lưu file: " + realError);
            }
        }

        // 4. CẬP NHẬT Ý TƯỞNG
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIdea(int id, [FromBody] CreateIdeaDto dto)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null) return NotFound("Không tìm thấy Ý tưởng!");

            idea.Title = dto.Title;
            idea.Content = dto.Content;
            idea.CategoryId = dto.CategoryId;
            idea.TopicId = dto.TopicId;

            await _context.SaveChangesAsync();
            return Ok(idea);
        }

        // 5. XÓA Ý TƯỞNG
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdea(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null) return NotFound("Không tìm thấy Ý tưởng!");

            _context.Ideas.Remove(idea);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công!" });
        }
    }
}