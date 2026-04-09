namespace UniversityIdeas.API.DTOs
{
    //Item 
    public class UserItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public int IdeaCount { get; set; } 
        public string Status { get; set; } = null!;
    }

    // Page
    public class UserPageDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int Administrators { get; set; }
        public int StaffUsers { get; set; }

        public List<UserItemDto> Users { get; set; } = new List<UserItemDto>();
    }

    public class UpdateUserDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int DepartmentId { get; set; } 
        public int RoleId { get; set; }      
        public bool IsActive { get; set; }

        // Nhận pass (cho phép Null nếu bỏ trống)
        public string? PasswordHash { get; set; }
    }
}