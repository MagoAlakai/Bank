var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BalanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration["BALANCESQLDBCONNECTIONSTRING"]));

builder.Services.AddScoped<IBalanceDbContext, BalanceDbContext>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

builder.Services.AddHostedService<ServiceBusRecieveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();

