var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TransferDbContext>(options =>
    options.UseSqlServer(builder.Configuration["TRANSFERSQLDBCONNECTIONSTRING"]));

builder.Services.AddScoped<ITransferDbContext, TransferDbContext>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

builder.Services.AddHostedService<ServiceBusRecieveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
