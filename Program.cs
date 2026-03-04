using Microsoft.EntityFrameworkCore;
using UniversityIdeas.API.Models;
using UniversityIdeas.API.Repositories; // Thư mục chứa Interface của bạn

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database
builder.Services.AddDbContext<UniversityIdeaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Repository (Để đáp ứng yêu cầu chuyên nghiệp của đồ án)
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddControllers();

// 3. QUAN TRỌNG: Khai báo dịch vụ Swagger (Lỗi của bạn nằm ở đây)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Cấu hình sử dụng Swagger trong môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Mặc định sẽ mở tại /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();