using CollaboCraft.Api.Extensions;
using CollaboCraft.Api.Hubs;
using CollaboCraft.Api.Middlewares;
using CollaboCraft.DataAccess.Extensions;
using CollaboCraft.Models.Auth;
using CollaboCraft.Models.Auth.Interfaces;
using CollaboCraft.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.MigrateDatabase(builder.Configuration);
builder.Services.AddDapper();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<IAuthSettings, AuthSettings>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/CollaboCraft/swagger.json", "CollaboCraft API v1"));
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHub<DocumentHub>("/documenthub");

app.Run();
