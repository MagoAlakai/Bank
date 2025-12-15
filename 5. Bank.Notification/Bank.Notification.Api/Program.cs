var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<INotificationDbContext, NotificationDbContext>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<ISendGridEmailService, SendGridEmailService>();

builder.Services.AddHostedService<ServiceBusRecieveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();

