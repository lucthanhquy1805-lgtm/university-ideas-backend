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
                .FirstOrDefaultAsync(i => i.Id == id);

            if (idea == null) return NotFound("Không tìm thấy ý tưởng này!");

            idea.ViewCount += 1;
            await _context.SaveChangesAsync();


            int realUpvotes = await _context.Reactions.CountAsync(r => r.IdeaId == id && r.ReactionType == 1);
            int realDownvotes = await _context.Reactions.CountAsync(r => r.IdeaId == id && r.ReactionType == 2);

            // Đếm luôn số lượng Comment thật cho chắc chắn
            int realCommentCount = await _context.Comments.CountAsync(c => c.IdeaId == id);

            // Trả về dữ liệu
            return Ok(new
            {
                id = idea.Id,
                title = idea.Title,
                content = idea.Content,
                createdAt = idea.CreatedAt,
                viewCount = idea.ViewCount,

                // THAY THẾ SỐ 0 BẰNG BIẾN ĐẾM THẬT
                thumbsUpCount = realUpvotes,
                thumbsDownCount = realDownvotes,
                commentCount = realCommentCount,

                filePath = idea.FilePath,
                isAnonymous = idea.IsAnonymous,
                categoryId = idea.CategoryId,
                categoryName = idea.Category?.Name,
                topicId = idea.TopicId,
                topicName = idea.Topic?.Name,
                authorName = (idea.IsAnonymous == true) ? "Anonymous" : idea.User?.FullName,
                departmentName = idea.User?.Department?.Name
            });
        }
        // =========================================================
        [HttpGet("{ideaId}/comments")]
        public async Task<IActionResult> GetComments(int ideaId)
        {
            var comments = await _context.Comments
                .Include(c => c.User) // Để lấy tên người bình luận
                .Where(c => c.IdeaId == ideaId)
                .OrderByDescending(c => c.CreatedAt) // Bình luận mới nhất lên đầu
                .Select(c => new {
                    c.Id,
                    c.Content,
                    c.CreatedAt,
                    c.IsAnonymous,
                    authorName = (c.IsAnonymous == true) ? "Anonymous" : c.User.FullName,
                    // (Tùy chọn) Thêm avatar mặc định dựa vào tên
                    avatar = $"https://ui-avatars.com/api/?name={(c.IsAnonymous == true ? "A" : c.User.FullName)}&background=random"
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost("comments")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
        {
            try
            {
                // A. Chuyển DTO sang Entity Comment
                var newComment = new Comment
                {
                    IdeaId = dto.IdeaId,
                    Content = dto.Content,
                    IsAnonymous = dto.IsAnonymous,

                    // 🔥 ĐÃ SỬA: Lấy UserId từ DTO truyền lên thay vì gắn cứng số 1
                    UserId = dto.UserId,

                    CreatedAt = DateTime.Now
                };

                // B. Lưu vào Database
                _context.Comments.Add(newComment);
                await _context.SaveChangesAsync();

                // C. Lấy lại Comment vừa lưu (kèm theo User để lấy tên)
                var savedComment = await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == newComment.Id);

                // D. Trả về cho React đúng chuẩn format nó đang cần
                return Ok(new
                {
                    id = savedComment.Id,
                    content = savedComment.Content,
                    createdAt = savedComment.CreatedAt,
                    isAnonymous = savedComment.IsAnonymous,
                    authorName = (savedComment.IsAnonymous == true) ? "Anonymous" : savedComment.User?.FullName,
                    avatar = $"https://ui-avatars.com/api/?name={(savedComment.IsAnonymous == true ? "A" : savedComment.User?.FullName)}&background=random"
                });
            }
            catch (Exception ex)
            {
                // Ép C# phải khai ra lỗi thật nếu có
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest("Lỗi khi lưu bình luận: " + realError);
            }
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
                // 🔥 ĐÃ THÊM: Tìm user để lấy DepartmentId
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null) return BadRequest("Người dùng không tồn tại!");

                var newIdea = new Idea
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CategoryId = dto.CategoryId,
                    TopicId = dto.TopicId,
                    UserId = dto.UserId,

                    IsAnonymous = dto.IsAnonymous,
                    AcademicYearId = 1,
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
                return Ok(new { message = "Thêm ý tưởng thành công!", id = newIdea.Id });
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest("Lỗi khi thêm Idea/Lưu file: " + realError);
            }
        }

        // 4. CẬP NHẬT Ý TƯỞNG
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIdea(int id, [FromForm] CreateIdeaDto dto) // 🔥 ĐÃ SỬA: [FromBody] thành [FromForm]
        {
            try
            {
                var idea = await _context.Ideas.FindAsync(id);
                if (idea == null) return NotFound("Không tìm thấy Ý tưởng!");

                idea.Title = dto.Title;
                idea.Content = dto.Content;
                idea.CategoryId = dto.CategoryId;
                idea.TopicId = dto.TopicId;
                idea.IsAnonymous = dto.IsAnonymous;

                // 🔥 BỔ SUNG: Xử lý file nếu người dùng có đính kèm file mới lúc Cập nhật
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

                    // Cập nhật đường dẫn file mới vào Database
                    idea.FilePath = "/uploads/" + uniqueFileName;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật thành công!", idea });
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest("Lỗi khi cập nhật Idea: " + realError);
            }
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

        [HttpPost("{id}/react")]
        public async Task<IActionResult> ReactToIdea(int id, [FromBody] ReactionRequest request)
        {
            try
            {
                // 🔥 ĐÃ SỬA: Lấy UserId từ dữ liệu truyền lên thay vì gắn cứng
                int currentUserId = request.UserId;

                // Kiểm tra xem User này đã từng bấm Like/Dislike bài này chưa
                var existingReaction = await _context.Reactions
                    .FirstOrDefaultAsync(r => r.IdeaId == id && r.UserId == currentUserId);

                if (existingReaction != null)
                {
                    if (existingReaction.ReactionType == request.ReactionType)
                    {
                        // Nếu bấm lại đúng cái nút đang sáng -> Rút lại Vote (Xóa khỏi DB)
                        _context.Reactions.Remove(existingReaction);
                    }
                    else
                    {
                        // Nếu bấm sang nút kia (Đang Like chuyển sang Dislike) -> Cập nhật lại
                        existingReaction.ReactionType = request.ReactionType;
                    }
                }
                else
                {
                    // Nếu chưa từng bấm -> Thêm mới vào DB
                    var newReaction = new Reaction
                    {
                        IdeaId = id,
                        UserId = currentUserId,
                        ReactionType = request.ReactionType
                    };
                    _context.Reactions.Add(newReaction);
                }

                await _context.SaveChangesAsync();

                // Đếm lại tổng số Like và Dislike MỚI NHẤT từ bảng Reactions
                int upvotes = await _context.Reactions.CountAsync(r => r.IdeaId == id && r.ReactionType == 1);
                int downvotes = await _context.Reactions.CountAsync(r => r.IdeaId == id && r.ReactionType == 2);

                // Trả về con số mới cho React để nó cập nhật giao diện
                return Ok(new { thumbsUpCount = upvotes, thumbsDownCount = downvotes });
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest("Lỗi khi đánh giá: " + realError);
            }
        }
        // =========================================================
        // API XÓA BÌNH LUẬN (DÀNH CHO ADMIN)
        // =========================================================
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(commentId);
                if (comment == null)
                {
                    return NotFound("Không tìm thấy bình luận này!");
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã xóa bình luận thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa bình luận: " + ex.Message);
            }
        }
    }
}