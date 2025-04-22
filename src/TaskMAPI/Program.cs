using Serilog;
using TaskMAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Add Serilog

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

// Replace the default logging with Serilog
builder.Host.UseSerilog();


//Add Cognito Authentication Services
//TODO: Move IAM configurations to Env variable
builder.Services.CognitoServices(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

//Add Swagger
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();

