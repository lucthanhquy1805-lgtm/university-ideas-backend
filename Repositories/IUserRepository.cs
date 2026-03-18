using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface IUserRepository
    {
        Task<UserPageDto> GetUserPageDataAsync();
    }
}