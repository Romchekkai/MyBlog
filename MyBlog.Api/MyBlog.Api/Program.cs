using Microsoft.EntityFrameworkCore;
using MyBlog.Api.Contracts.Mapping;
using MyBlog.Api.Contracts.Services;
using MyBlog.Api.DLL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Error");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
            .AddTransient<ICommentRepository, CommentRepository>()
            .AddTransient<IArticleRepository, ArticleRepository>()
            .AddTransient<IUserRepository, UserRepository>();

builder.Services
            .AddTransient<IUserService, UserService>()
            .AddTransient<ICommentService, CommentService>()
            .AddTransient<IArticleService, ArticleService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
