using System.Text.Json.Serialization;
using NatalChart.Astrology;
using NatalChart.Core.Interfaces;
using NatalChart.Infrastructure;
using NatalChart.Interpretation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddSingleton<IEphemerisService, EphemerisService>();
builder.Services.AddSingleton<IHouseService, HouseService>();
builder.Services.AddSingleton<IAspectService, AspectService>();
builder.Services.AddSingleton<IChartCalculator, ChartCalculator>();
builder.Services.AddSingleton<IInterpretationService, InterpretationService>();
builder.Services.AddSingleton<ITimeZoneService, TimeZoneService>();
builder.Services.AddHttpClient<IGeocodingService, GeocodingService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
