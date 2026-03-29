namespace UniversityIdeas.API.DTOs
{
    // Đây là class chỉ chứa ĐÚNG những gì React gửi lên
    public class CreateIdeaDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int CategoryId { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public bool IsAnonymous { get; set; }
        public int AcademicYearId { get; set; }
        public IFormFile? File { get; set; }
    }
}