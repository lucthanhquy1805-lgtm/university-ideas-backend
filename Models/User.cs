namespace UniversityIdeas.API.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }

        // Khóa ngoại cho Role (Đã sửa)
        public int? RoleId { get; set; }

        // --- Các Navigation Properties ---
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual Department Department { get; set; } = null!;

        // Chỉ có 1 Role thay vì ICollection (Đã sửa)
        public virtual Role? Role { get; set; }

        public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}