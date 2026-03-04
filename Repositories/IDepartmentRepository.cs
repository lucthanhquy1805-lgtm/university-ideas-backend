using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    }
}