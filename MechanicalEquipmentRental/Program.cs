using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RentalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình CORS (Cho phép React Frontend truy cập API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Đổi nếu React chạy trên cổng khác
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// Đăng ký dịch vụ mã hóa mật khẩu cho khách hàng
builder.Services.AddScoped<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

// Thêm Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rental API", Version = "v1" });
});

var app = builder.Build();

// Kích hoạt CORS
app.UseCors("AllowReactApp");

// Kích hoạt Swagger nếu ở môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
