using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IIdeaRepository
    {
       
        Task<IEnumerable<IdeaDto>> GetAllIdeasAsync(string? search, int? categoryId, int? topicId, int? departmentId, string? sortBy);
    }
}