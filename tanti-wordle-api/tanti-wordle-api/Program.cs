var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();


var app = builder.Build();
UseSwagger();
app.MapHealthChecks("/healthz");

app.Run();

void UseSwagger()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            opt.RoutePrefix = "";
        });
    }
}