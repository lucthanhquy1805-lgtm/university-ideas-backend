using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IIdeaRepository
    {
        // Nhận vào các tham số để Lọc và Sắp xếp
        Task<IEnumerable<IdeaDto>> GetAllIdeasAsync(string? search, int? categoryId, int? topicId, int? departmentId, string? sortBy);
    }
}