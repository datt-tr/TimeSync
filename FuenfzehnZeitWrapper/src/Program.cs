using Scalar.AspNetCore;
using FuenfzehnZeitWrapper.Models;
using FuenfzehnZeitWrapper.Interfaces;
using FuenfzehnZeitWrapper.Services;
using FuenfzehnZeitWrapper.Helpers;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddOtlpExporter())
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddOtlpExporter())
      .WithLogging(logging => logging
        .AddOtlpExporter());

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Configure<GlobalVariables>(builder.Configuration.GetSection(GlobalVariables.CollectionName));
builder.Services.AddHttpClient<IFuenfzehnZeitService, FuenfzehnZeitService>();

builder.Services.AddSingleton<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IFuenfzehnZeitHtmlParser, FuenfzehnZeitHtmlParser>();
builder.Services.AddScoped<IFormDataBuilder, FormDataBuilder>();

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
