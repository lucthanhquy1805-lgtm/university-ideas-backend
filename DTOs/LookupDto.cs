namespace UniversityIdeas.API.DTOs
{
    // DTO chung dùng cho các ô Dropdown (Category, Topic, Department)
    public class LookupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}