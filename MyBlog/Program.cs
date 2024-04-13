using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Mapping;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Repository;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));


builder.Services.AddTransient<IUserService,UserService>();

builder.Services.AddTransient<IUserRepository, UserRepository>();



// Add service AutoMapper.
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var imgFiles = new[] { "Logo.jpg", "user.png" };


/*foreach (var file in imgFiles)
{
    app.MapGet($"/images/{file}", async context =>
    {
        var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "images", file);
        var img = await File.ReadAllBytesAsync(imgPath);
        await context.Response.Body.WriteAsync(img);
    });
}*/
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
