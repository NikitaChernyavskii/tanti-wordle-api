using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json.Serialization;
using Wordle.Api.Infrastructure;
using Wordle.Api.Infrastructure.ExceptionHandlers;
using Wordle.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    })
    .AddControllers(options =>
    {
        options.OutputFormatters.RemoveType<StringOutputFormatter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(opt =>
{
    opt.LowercaseUrls = true;
});

builder.Services.AddWordleServices();
builder.Services.AddAutoMapper(typeof(Program));

AddHealthChecksServices();
AddExceptinHandlingServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseCors();

app.MapControllers();
app.MapHealthChecks("/healthz");

app.Run();

void AddHealthChecksServices()
{
    builder.Services.AddHealthChecks()
        .AddCheck<HealthCheckWithDependencies>("HealthCheckWithDependencies");
}

void AddExceptinHandlingServices()
{
    // register excepttion handlers in right order! DefaultExceptionHandler should be last
    builder.Services.AddExceptionHandler<ValidationFailedExceptionHandler>();
    builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
    builder.Services.AddProblemDetails();
}
