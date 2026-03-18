using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IIdeaRepository
    {
        Task<IEnumerable<IdeaDto>> GetAllIdeasAsync();
    }
}