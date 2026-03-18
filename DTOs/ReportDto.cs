namespace UniversityIdeas.API.DTOs
{
    // DTO cho biểu đồ và bảng theo từng phòng ban
    public class DepartmentStatsDto
    {
        public string DepartmentName { get; set; } = null!;
        public int IdeaCount { get; set; }
        public double IdeaPercentage { get; set; } // Tính % cho bảng
        public int ContributorCount { get; set; }
        public double ContributorPercentage { get; set; } // Tính % cho bảng
    }

    // DTO chung cho danh sách các báo cáo ngoại lệ (Exception Reports)
    public class ExceptionItemDto
    {
        public string Title { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateTime Date { get; set; }
    }

    // Siêu DTO: Gom toàn bộ trang Reports
    public class ReportSummaryDto
    {
        // 3 KPI đầu trang
        public int TotalIdeas { get; set; }
        public int TotalContributors { get; set; }
        public int TotalComments { get; set; }

        // Dữ liệu chung cho cả Biểu đồ và Bảng Thống kê
        public List<DepartmentStatsDto> DepartmentStats { get; set; } = new List<DepartmentStatsDto>();

        // 3 Bảng Ngoại lệ ở cuối trang
        public List<ExceptionItemDto> IdeasWithoutComments { get; set; } = new List<ExceptionItemDto>();
        public List<ExceptionItemDto> AnonymousIdeas { get; set; } = new List<ExceptionItemDto>();
        public List<ExceptionItemDto> AnonymousComments { get; set; } = new List<ExceptionItemDto>();
    }
}