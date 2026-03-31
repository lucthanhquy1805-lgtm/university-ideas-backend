using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;

        public TopicsController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }


        [HttpGet("page-data")]
        public async Task<IActionResult> GetPageData()
        {
            var data = await _topicRepository.GetTopicPageDataAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] Topic newTopic)
        {
            try
            {
                await _topicRepository.CreateTopicAsync(newTopic);
                return Ok(new { message = "Thêm Topic thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi thêm Topic: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] Topic updatedTopic)
        {
            if (id != updatedTopic.Id)
                return BadRequest(new { message = "ID không khớp!" });

            try
            {
                await _topicRepository.UpdateTopicAsync(updatedTopic);
                return Ok(new { message = "Cập nhật Topic thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật Topic: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            try
            {
                await _topicRepository.DeleteTopicAsync(id);
                return Ok(new { message = "Xóa Topic thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa Topic: " + ex.Message });
            }
        }
    }
}