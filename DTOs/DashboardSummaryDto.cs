namespace UniversityIdeas.API.DTOs
{
    
    public class DepartmentIdeaCountDto
    {
        public string DepartmentName { get; set; } = null!;
        public int IdeaCount { get; set; }
    }

    
    public class ActivityLogDto
    {
        public string ActionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }

    public class IdeaDashboardSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int ViewCount { get; set; }
        public int Upvotes { get; set; }
        public int CommentCount { get; set; }
    }
    public class DashboardSummaryDto
    {
        public int TotalIdeas { get; set; }
        public int TotalUsers { get; set; }
        public int TotalComments { get; set; }
        public int ActiveCategories { get; set; }
        public int PopularIdeasCount { get; set; }
        public int ActiveTopicsCount { get; set; }

        public List<DepartmentIdeaCountDto> IdeasByDepartment { get; set; } = new List<DepartmentIdeaCountDto>();
        public List<ActivityLogDto> RecentActivities { get; set; } = new List<ActivityLogDto>();
        public List<IdeaDashboardSummaryDto> LatestIdeas { get; set; } = new List<IdeaDashboardSummaryDto>();
        public List<IdeaDashboardSummaryDto> MostViewedIdeas { get; set; } = new List<IdeaDashboardSummaryDto>();
    }
}