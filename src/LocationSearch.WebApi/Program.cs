using FluentValidation;
using FluentValidation.AspNetCore;
using LocationSearch.Application.Search;
using LocationSearch.Core;
using LocationSearch.Infrastructure.Cache;
using LocationSearch.Infrastructure.Repositories;
using LocationSearch.WebApi;
using LocationSearch.WebApi.Models;
using LocationSearch.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddTransient<IApplicationConfiguration, WebApplicationConfiguration>();
builder.Services.AddSingleton<IAddressCache, InMemoryAddressCache>();
builder.Services.AddTransient<IAddressRepository, CsvAddressRepository>();
builder.Services.AddTransient<ISearchLocationRepository, CacheSearchLocationRepository>();
builder.Services.AddTransient<ISearchLocationWithinDistance, SearchLocationService>();
builder.Services.AddTransient<IValidator<SearchRequestModel>, SearchRequestModelValidator>();
builder.Services.AddControllers().AddFluentValidation();
;
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Map("/", async ctx =>
{
    ctx.Response.Headers.ContentType = "text/html";
    await ctx.Response.WriteAsync("Please open <a href=\"/swagger\">Swagger</a> to explore API");
});

// Initialize internal application cache
ApplicationCache.Initialize(app);

app.Run();