var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();
builder.Services.AddSingleton<IProcessService, ProcessService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
ApigatewayEndPoint.GetwayEndpoint(app);

app.Run();

