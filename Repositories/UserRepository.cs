using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversityIdeaDbContext _context;

        public UserRepository(UniversityIdeaDbContext context)
        {
            _context = context;
        }

        public async Task<UserPageDto> GetUserPageDataAsync()
        {
            var result = new UserPageDto();

            // 1. Tính toán 4 thẻ KPI
            result.TotalUsers = await _context.Users.CountAsync();
            result.ActiveUsers = await _context.Users.CountAsync(u => u.IsActive);
            result.Administrators = await _context.Users.CountAsync(u => u.Role != null && u.Role.Name == "Administrator");
            result.StaffUsers = await _context.Users.CountAsync(u => u.Role != null && u.Role.Name == "Staff");

            // 2. Lấy danh sách Users, gom luôn Tên Phòng ban, Tên Vai trò và Đếm Ý tưởng
            var users = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.Role)
                .Select(u => new UserItemDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    DepartmentName = u.Department.Name,
                    RoleName = u.Role != null ? u.Role.Name : "No Role",
                    IdeaCount = _context.Ideas.Count(i => i.UserId == u.Id), // Tự động đếm ý tưởng
                    Status = u.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync();

            result.Users = users;

            return result;
        }
    }
}