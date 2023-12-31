using Microsoft.OpenApi.Models;
using notesBE;
using notesBE.Models;
using notesBE.Services;

var root = Directory.GetCurrentDirectory();
NetEnv.LoadEnvironmentVariables(Path.Combine(root, ".env"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<NotesDatabaseSettings>((settings) =>
{
    var fileConfig = builder.Configuration.GetSection("notesDatabase");
    
    settings.ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
    settings.DatabaseName = fileConfig.GetSection("DatabaseName").Value;
    settings.ArticlesCollectionName = fileConfig.GetSection("ArticlesCollectionName").Value;
});

builder.Services.AddSingleton<ArticlesService>();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "notesBE", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "notesBE v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();