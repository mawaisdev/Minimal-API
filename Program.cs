var builder = WebApplication.CreateBuilder(args);
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