namespace UniversityIdeas.API.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string ActionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}