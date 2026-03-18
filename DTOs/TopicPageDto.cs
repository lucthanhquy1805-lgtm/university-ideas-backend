namespace UniversityIdeas.API.DTOs
{
    // Đại diện cho 1 dòng trong bảng Figma
    public class TopicItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!; // Lấy từ bảng joined Categories
        public string? Description { get; set; }
        public int IdeaCount { get; set; } // Hiển thị số 23, 19...
        public string Status { get; set; } = null!; // Hiển thị "Active" hoặc "Inactive"
    }

    // Đại diện cho toàn bộ trang Topics
    public class TopicPageDto
    {
        // 3 Thẻ ở trên cùng
        public int TotalTopics { get; set; }
        public int ActiveTopics { get; set; }
        public int TotalIdeas { get; set; }

        // Danh sách bảng ở dưới
        public List<TopicItemDto> Topics { get; set; } = new List<TopicItemDto>();
    }
}