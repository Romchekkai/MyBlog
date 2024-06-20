using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyBlog.BLL.Services.UserServices;
using MyBlog.DAL.Repository;
using MyBlog.Mapping;
using MyBlog.Middleware;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);


    string connection = builder.Configuration.GetConnectionString("DefaultConnection")?? throw new ArgumentException("Error");

    // добавляем контекст ApplicationContext в качестве сервиса в приложение
    builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var logger = new LoggerConfiguration()
       .Enrich.FromLogContext()
       .Enrich.WithExceptionDetails()
       .Enrich.WithMachineName()
       .WriteTo.Console()
       .WriteTo.File("D:\\MyProjects\\MyBlog\\MyBlog\\logger.txt")
.Enrich.WithProperty("Environment", "Debug")

    //   .ReadFrom.Configuration(configuration)
       .CreateLogger();

// Add services to the container.
/*builder.Services.AddLogging(config =>
{
    config.AddTelegramBot();
});*/
builder.Host.UseSerilog(logger);
builder.Logging.SetMinimumLevel(LogLevel.Debug);



builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();

builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();


// Add exeption middleware
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();



// Add service AutoMapper.
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/AuthenticationError";
        options.AccessDeniedPath = "/Home/AuthorizationError";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/UnexpectedError", "?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");   
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();


   
 




