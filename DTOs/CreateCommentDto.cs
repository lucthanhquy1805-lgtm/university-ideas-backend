namespace UniversityIdeas.API.DTOs
{
    public class CreateCommentDto
    {
        public int IdeaId { get; set; }
        public string? Content { get; set; }
        public bool IsAnonymous { get; set; }
        public int UserId { get; set; }
    }
}
