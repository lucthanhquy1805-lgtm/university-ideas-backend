using Microsoft.AspNetCore.Mvc;
using UniversityIdeas.API.DTOs;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.Repositories;

namespace UniversityIdeas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

       
        [HttpGet("page-data")]
        public async Task<IActionResult> GetPageData()
        {
            var data = await _categoryRepository.GetCategoryPageDataAsync();
            return Ok(data);
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category newCategory) // Sửa thành Category
        {
            try
            {
                // Xóa chữ GetCategoryPageDataAsync đi, gõ dấu chấm (.) để tìm hàm Add
                await _categoryRepository.AddCategoryAsync(newCategory);// 

                return Ok(new { message = "Thêm danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi thêm danh mục: " + ex.Message });
            }
        }

        //Delete
        [HttpDelete("{id}")] // Chú ý: Có chữ {id} ở đây để bắt được cái mã số xe tải gửi lên
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return Ok(new { message = "Xóa danh mục thành công!" });
            }
            catch (Exception ex)
            {
                // Thường lỗi xảy ra ở đây là do: Danh mục này đang chứa Idea, SQL Server không cho xóa để bảo toàn dữ liệu
                return BadRequest(new { message = "Không thể xóa danh mục này: " + ex.Message });
            }
        }

        //Edit
        [HttpPut("{id}")] // Bắt buộc phải có ID để biết đang sửa danh mục nào
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            // Kiểm tra an ninh: ID trên thanh địa chỉ phải khớp với ID trong gói hàng
            if (id != updatedCategory.Id) return BadRequest(new { message = "ID không khớp!" });

            try
            {
                await _categoryRepository.UpdateCategoryAsync(updatedCategory);
                return Ok(new { message = "Cập nhật danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật: " + ex.Message });
            }
        }
    }
}