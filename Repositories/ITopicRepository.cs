using UniversityIdeas.API.DTOs;

namespace UniversityIdeas.API.Repositories
{
    public interface ITopicRepository
    {
        Task<TopicPageDto> GetTopicPageDataAsync();
    }
}