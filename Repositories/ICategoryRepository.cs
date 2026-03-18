using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryPageDto> GetCategoryPageDataAsync();
    }
}