namespace UniversityIdeas.API.DTOs
{
    // Đại diện cho 1 dòng
    public class CategoryItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int IdeaCount { get; set; } 
        public string Status { get; set; } = null!; 
    }

    // trang Categories
    public class CategoryPageDto
    {
        // 3 Thẻ ở trên cùng
        public int TotalCategories { get; set; }
        public int ActiveCategories { get; set; }
        public int TotalIdeas { get; set; }

        // Danh sách bảng ở dưới
        public List<CategoryItemDto> Categories { get; set; } = new List<CategoryItemDto>();
    }
}