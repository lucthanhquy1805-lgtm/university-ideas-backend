namespace UniversityIdeas.API.DTOs
{
   
    public class DepartmentStatsDto
    {
        public string DepartmentName { get; set; } = null!;
        public int IdeaCount { get; set; }
        public double IdeaPercentage { get; set; } 
        public int ContributorCount { get; set; }
        public double ContributorPercentage { get; set; }
    }

   
    public class ExceptionItemDto
    {
        public string Title { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateTime Date { get; set; }
    }

    
    public class ReportSummaryDto
    {
    
        public int TotalIdeas { get; set; }
        public int TotalContributors { get; set; }
        public int TotalComments { get; set; }

       
        public List<DepartmentStatsDto> DepartmentStats { get; set; } = new List<DepartmentStatsDto>();

       
        public List<ExceptionItemDto> IdeasWithoutComments { get; set; } = new List<ExceptionItemDto>();
        public List<ExceptionItemDto> AnonymousIdeas { get; set; } = new List<ExceptionItemDto>();
        public List<ExceptionItemDto> AnonymousComments { get; set; } = new List<ExceptionItemDto>();
    }
}