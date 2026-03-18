namespace UniversityIdeas.API.DTOs
{
    public class IdeaDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        // --- Thông tin Tác giả ---
        public string AuthorName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!; // Ví dụ: IT Services

        // --- Phân loại ---
        public string CategoryName { get; set; } = null!; // Ví dụ: Technology

        // --- Chỉ số tương tác (Giống hàng icon dưới cùng của mỗi Idea trên Figma) ---
        public int ViewCount { get; set; }
        public int ThumbsUpCount { get; set; }
        public int ThumbsDownCount { get; set; }
        public int CommentCount { get; set; }

        // --- Thời gian đăng ---
        public DateTime CreatedAt { get; set; }
    }
}