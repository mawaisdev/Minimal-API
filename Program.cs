using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CmsDbContext>(options => options.UseInMemoryDatabase("CMSDatabase"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courses", () => {});

app.Run();

public class Course {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long Duration { get; set; }
    public int Type { get; set; }
}

public class CmsDbContext : DbContext {
    public DbSet<Course> Courses => Set<Course>();
    public CmsDbContext(DbContextOptions options) : base(options)
    {
    }
}