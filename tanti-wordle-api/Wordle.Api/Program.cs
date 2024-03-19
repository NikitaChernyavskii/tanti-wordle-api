using Wordle.Api.Infrastructure;
using Wordle.Api.Infrastructure.ExceptionHandlers;
using Wordle.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.AddWordleServices();
}
