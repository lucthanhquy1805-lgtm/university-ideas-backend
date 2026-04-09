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

            result.TotalUsers = await _context.Users.CountAsync();
            result.ActiveUsers = await _context.Users.CountAsync(u => u.IsActive);
            result.Administrators = await _context.Users.CountAsync(u => u.Role != null && u.Role.Name == "Administrator");
            result.StaffUsers = await _context.Users.CountAsync(u => u.Role != null && u.Role.Name == "Staff");

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
                    IdeaCount = _context.Ideas.Count(i => i.UserId == u.Id), 
                    Status = u.IsActive ? "Active" : "Inactive"
                })
                .ToListAsync();

            result.Users = users;

            return result;
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto dto)
        {
            // 1. Tìm User cũ dưới Database lên
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser != null)
            {
                // 2. Ghi đè các thông tin cơ bản
                existingUser.FullName = dto.FullName;
                existingUser.Email = dto.Email;
                existingUser.DepartmentId = dto.DepartmentId;
                existingUser.RoleId = dto.RoleId;
                existingUser.IsActive = dto.IsActive;

                // 3. LOGIC MẤU CHỐT: Chỉ cập nhật mật khẩu nếu có gõ pass mới
                if (!string.IsNullOrWhiteSpace(dto.PasswordHash))
                {
                    existingUser.PasswordHash = dto.PasswordHash;
                }

                // 4. Lưu lại (Lưu ý: Không cần dùng _context.Users.Update() nữa vì FindAsync đã giúp EF Core tự động theo dõi existingUser rồi)
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Không tìm thấy người dùng này trong cơ sở dữ liệu!");
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}