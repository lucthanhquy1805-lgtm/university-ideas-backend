using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UniversityIdeaDbContext _context;


        public DepartmentsController(UniversityIdeaDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {

            var departments = await _context.Departments.ToListAsync();

            return Ok(departments);
        }
    }
}