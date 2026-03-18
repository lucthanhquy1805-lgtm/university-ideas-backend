namespace UniversityIdeas.API.DTOs
{
    // Đại diện cho 1 dòng trong bảng User
    public class UserItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public int IdeaCount { get; set; } // Đếm số lượng Idea giống như thiết kế
        public string Status { get; set; } = null!;
    }

    // Đại diện cho toàn bộ trang
    public class UserPageDto
    {
        // 4 Thẻ thống kê
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int Administrators { get; set; }
        public int StaffUsers { get; set; }

        // Danh sách hiển thị
        public List<UserItemDto> Users { get; set; } = new List<UserItemDto>();
    }
}