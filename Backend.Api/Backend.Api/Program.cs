using Backend.Api;
//using Backend.API.Frontend.Services;
using Backend.Application.Services;
using Backend.Core.Interfaces;
using Backend.Infrastructure;
using Backend.Infrastructure.appSettingsData;
using Backend.Infrastructure.Clients;
using Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();
builder.Services.AddScoped<IAgencyValidationService, AgencyValidationService>();
builder.Services.AddScoped<IFlightSearchService, FlightSearchService>();
builder.Services.AddScoped<IAgencyApiClient, AgencyApiClient>();
//builder.Services.AddScoped<Backend.API.Frontend.Services.IFrontendFlightService, Backend.API.Frontend.Services.FlightService>();
builder.Services.AddScoped<Backend.Core.Interfaces.IFlightSearchService, Backend.Application.Services.FlightSearchService>();
//builder.Services.AddScoped<Backend.API.Frontend.Services.IFrontendFlightService, Backend.API.Frontend.Services.FlightService>();
builder.Services.AddScoped<Validation>();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("agencycredentials.json", optional: false, reloadOnChange: true);
// Add to the existing Program.cs
builder.Services.Configure<AgencyApiSettings>(builder.Configuration.GetSection("AgencyApiSettings"));
builder.Services.Configure<AgencyCredentials>(
    builder.Configuration.GetSection("AgencyCredentials"));


//builder.Services.Configure<ApiRequestDefaults>(
//    builder.Configuration.GetSection("ApiRequestDefaults"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppDI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Flight}/{action=Search}/{id?}"); // MVC routes (/Flight/Search, etc.)

app.Run();
