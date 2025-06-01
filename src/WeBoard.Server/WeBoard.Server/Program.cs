using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WeBoard.Server.Configuration;
using WeBoard.Server.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddDbContext<WeBoardContext>(options =>
    options.UseMongoDB(mongoDbSettings!.DefaultConnection, mongoDbSettings.DefaultDatabaseName));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024 * 1024; // 1MB
});
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();
app.MapHub<BoardHub>("/boardHub");
app.UseCors(policy => policy
    .AllowAnyOrigin()     
    .AllowAnyHeader()
    .AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference("scalar");
    app.MapOpenApi();
}


app.MapControllers();
app.Run();
