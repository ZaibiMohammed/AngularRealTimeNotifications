using Microsoft.AspNetCore.SignalR;
using NotificationsApi.Hubs;
using NotificationsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Add SignalR
builder.Services.AddSignalR();

// Add services
builder.Services.AddSingleton<INotificationService, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
