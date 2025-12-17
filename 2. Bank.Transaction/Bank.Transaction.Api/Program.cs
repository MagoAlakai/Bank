using Bank.Transaction.Api.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration["TRANSACTIONSQLDBCONNECTIONSTRING"]));

builder.Services.AddScoped<ITransactionDbContext,TransactionDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IProcessService, ProcessService>();

builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();

builder.Services.AddHostedService<ServiceBusRecieveService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessHandler).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
