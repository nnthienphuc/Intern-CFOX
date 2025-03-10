using CFOXApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký ApplicationDbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

var app = builder.Build();

// Lấy logger để ghi log ra Output Debug
var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
            logger.LogInformation("✅ Kết nối SQL Server thành công!");
        else
            logger.LogError("❌ Không thể kết nối SQL Server.");
    }
    catch (Exception ex)
    {
        logger.LogError($"❌ Lỗi kết nối SQL Server: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
