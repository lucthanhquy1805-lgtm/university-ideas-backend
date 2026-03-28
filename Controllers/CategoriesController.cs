using Microsoft.AspNetCore.Mvc;
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
    }
}