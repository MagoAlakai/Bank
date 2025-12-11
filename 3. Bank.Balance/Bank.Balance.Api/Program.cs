var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BalanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration["BALANCESQLDBCONNECTIONSTRING"]));

builder.Services.AddScoped<IBalanceDbContext, BalanceDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();

