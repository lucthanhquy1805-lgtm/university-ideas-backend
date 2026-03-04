using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public DepartmentRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToListAsync();
        }
    }
}