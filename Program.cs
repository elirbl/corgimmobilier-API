using Microsoft.EntityFrameworkCore;
using YmmoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<YmmoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Ymmo API", Version = "v1" }));

builder.Services.AddCors(o => o.AddPolicy("AllowApp",
    p => p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<YmmoDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ymmo API v1"));
}

app.UseCors("AllowApp");
app.UseAuthorization();
app.MapControllers();
app.Run();
