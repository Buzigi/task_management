using Serilog;
using TaskMAPI.Services;
using TaskMAPI.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Add Serilog
builder.Services.SerilogServices(builder.Configuration);

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

if (builder.Configuration.GetValue<bool>("SwaggerEnabled"))
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();

