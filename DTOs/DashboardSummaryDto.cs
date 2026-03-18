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

   
    public class DashboardSummaryDto
    {
        public int TotalIdeas { get; set; }
        public int TotalUsers { get; set; }
        public int TotalComments { get; set; }
        public int ActiveCategories { get; set; }

        public List<DepartmentIdeaCountDto> IdeasByDepartment { get; set; } = new List<DepartmentIdeaCountDto>();
        public List<ActivityLogDto> RecentActivities { get; set; } = new List<ActivityLogDto>();
    }
}