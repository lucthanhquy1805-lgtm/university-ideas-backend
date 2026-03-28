using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public CategoryRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryPageDto> GetCategoryPageDataAsync()
        {
            var result = new CategoryPageDto();

          
            result.TotalCategories = await _context.Categories.CountAsync();
            result.ActiveCategories = await _context.Categories.CountAsync(c => c.IsActive);
            result.TotalIdeas = await _context.Ideas.CountAsync(); 

            
            var categories = await _context.Categories
                .Select(c => new CategoryItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IdeaCount = _context.Ideas.Count(i => i.CategoryId == c.Id), 
                    Status = c.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync();

            result.Categories = categories;

            return result;
        }
    }
}