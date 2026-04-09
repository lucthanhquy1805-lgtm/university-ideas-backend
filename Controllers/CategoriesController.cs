using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

       
        [HttpGet("page-data")]
        public async Task<IActionResult> GetPageData()
        {
            var data = await _categoryRepository.GetCategoryPageDataAsync();
            return Ok(data);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category newCategory) 
        {
            try
            {
                await _categoryRepository.AddCategoryAsync(newCategory);

                return Ok(new { message = "Category added successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error when adding a category: " + ex.Message });
            }
        }

        //Delete
        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return Ok(new { message = "Category deletion successful!" });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { message = "This category cannot be deleted: " + ex.Message });
            }
        }

        //Edit
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            
            if (id != updatedCategory.Id) return BadRequest(new { message = "IDs don't match!" });

            try
            {
                await _categoryRepository.UpdateCategoryAsync(updatedCategory);
                return Ok(new { message = "Catalog update successful!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error during update: " + ex.Message });
            }
        }
    }
}