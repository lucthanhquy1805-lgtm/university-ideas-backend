namespace UniversityIdeas.API.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Mối quan hệ: 1 Topic thuộc về 1 Category
        public virtual Category Category { get; set; } = null!;

        // Mối quan hệ: 1 Topic có nhiều Ideas
        public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
    }
}