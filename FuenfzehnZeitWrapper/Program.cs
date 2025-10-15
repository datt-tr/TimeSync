using Scalar.AspNetCore;
using FuenfzehnZeitWrapper.Models;
using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Services;
using FuenfzehnZeitWrapper.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<GlobalVariables>(builder.Configuration.GetSection(GlobalVariables.CollectionName));
builder.Services.AddHttpClient<IFuenfzehnZeitService, FuenfzehnZeitService>();

builder.Services.AddSingleton<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IFuenfzehnZeitHtmlParser, FuenfzehnZeitHtmlParser>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
