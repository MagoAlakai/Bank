var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<INotificationDbContext, NotificationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();

