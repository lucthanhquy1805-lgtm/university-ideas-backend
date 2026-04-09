using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Repositories
{
    public interface IUserRepository
    {
        Task<UserPageDto> GetUserPageDataAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(int id, UpdateUserDto dto);
        Task DeleteUserAsync(int id);
    }
}