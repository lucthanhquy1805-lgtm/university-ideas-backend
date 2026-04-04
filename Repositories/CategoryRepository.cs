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
                    IsActive = c.IsActive 
                })
                .ToListAsync();

            result.Categories = categories;

            return result;
        }
        //add
        public async Task AddCategoryAsync(Category category)
        {
            
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        //delete
        public async Task DeleteCategoryAsync(int id)
        {
           
            var category = await _context.Categories.FindAsync(id);

            
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        //Update
        public async Task UpdateCategoryAsync(Category category)
        {
            
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}