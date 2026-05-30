using OleleLibraryAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "OleleLibraryAPI v1");
    options.DocumentTitle = "OleleLibraryAPI";
});

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "OleleLibraryAPI"
}));

app.MapControllers();

await app.RunAsync();

