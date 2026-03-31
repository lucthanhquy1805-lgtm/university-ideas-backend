using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;

namespace UniversityIdeas.API.Repositories
{
    public interface ITopicRepository
    {
        Task<TopicPageDto> GetTopicPageDataAsync();
        Task CreateTopicAsync(Topic topic);
        Task UpdateTopicAsync(Topic topic);
        Task DeleteTopicAsync(int id);
    }
}