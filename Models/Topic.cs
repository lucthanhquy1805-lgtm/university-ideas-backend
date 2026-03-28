namespace UniversityIdeas.API.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; } = null!;

        // 1 Topic có nhiều Ideas
        public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
    }
}