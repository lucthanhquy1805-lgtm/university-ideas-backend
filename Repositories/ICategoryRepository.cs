using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryPageDto> GetCategoryPageDataAsync();
        Task AddCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task UpdateCategoryAsync(Category category);
    }


}
