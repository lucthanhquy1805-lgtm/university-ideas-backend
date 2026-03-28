namespace UniversityIdeas.API.DTOs
{
    public class IdeaDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public string AuthorName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    
        public string CategoryName { get; set; } = null!; 
        public string? TopicName { get; set; }

        
        public int ViewCount { get; set; }
        public int ThumbsUpCount { get; set; }
        public int ThumbsDownCount { get; set; }
        public int CommentCount { get; set; }

        
        public DateTime CreatedAt { get; set; }
    }
}