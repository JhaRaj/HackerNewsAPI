using HackerNews.Common.Cache;
using HackerNews.Common.Http;
using HackerNews.Service;
using HackerNews.WebApi.Middleware;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<ICache, Cache>();
builder.Services.AddSingleton<IRestClient, RestClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseErrorHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
