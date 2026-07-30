using Microsoft.EntityFrameworkCore;
using BudgetForecastingAPI.Data;
using BudgetForecastingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Veritabanı servisini ve SQLite bağlantısını sisteme kaydediyoruz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Katman 2: Bütçe tahmin servisini DI container'a kaydediyoruz
builder.Services.AddScoped<IBudgetPredictionService, BudgetPredictionManager>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/openapi/v1.json", "Budget Forecasting API"); });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
