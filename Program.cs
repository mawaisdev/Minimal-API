using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CmsDbContext>(options => options.UseInMemoryDatabase("CMSDatabase"));
builder.Services.AddAutoMapper(typeof(CMSMapper));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courses", async (CmsDbContext db) => 
{
    var result = await db.Courses.ToListAsync();
    return Results.Ok(result);
});

app.MapPost("/courses", async ([FromBody]CourseDto courseDto, [FromServices] CmsDbContext dbContext, [FromServices] IMapper mapper ) => {
    if(courseDto.Name == "")
        return Results.BadRequest("Invalid Data");

    var course = mapper.Map<Course>(courseDto);

    await dbContext.Courses.AddAsync(course);
    await dbContext.SaveChangesAsync();
    var courses = await dbContext.Courses.ToListAsync();
    return Results.Ok(courses);

});

app.MapGet("/courses/{Id}", async ([FromRoute]long Id,CmsDbContext db, IMapper mapper) => {
    var course = await db.Courses.SingleOrDefaultAsync(x => x.Id == Id);
    if(course != null){
        var courseDto = mapper.Map<CourseDto>(course);
        return Results.Ok(courseDto);
    }

    return Results.NotFound();
});

app.Run();

public class CMSMapper : Profile {
    public CMSMapper()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<CourseDto, Course>();
    }
}

public class Course {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long Duration { get; set; }
    public int Type { get; set; }
}

public class CourseDto {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public COURSE_TYPE Type { get; set; }
}

public enum COURSE_TYPE {
    Engineering = 1,
    Medical,
    Management
}

public class CmsDbContext : DbContext {
    public DbSet<Course> Courses => Set<Course>();
    public CmsDbContext(DbContextOptions options) : base(options)
    {
    }
}