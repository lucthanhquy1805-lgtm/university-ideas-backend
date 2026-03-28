namespace UniversityIdeas.API.DTOs
{
    // Item
    public class TopicItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!; 
        public string? Description { get; set; }
        public int IdeaCount { get; set; } 
        public string Status { get; set; } = null!; 
    }

    // Page
    public class TopicPageDto
    {

        public int TotalTopics { get; set; }
        public int ActiveTopics { get; set; }
        public int TotalIdeas { get; set; }

        public List<TopicItemDto> Topics { get; set; } = new List<TopicItemDto>();
    }
}