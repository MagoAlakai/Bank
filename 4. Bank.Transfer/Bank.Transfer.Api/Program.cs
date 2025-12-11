var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TransferDbContext>(options =>
    options.UseSqlServer(builder.Configuration["TRANSFERSQLDBCONNECTIONSTRING"]));

builder.Services.AddScoped<ITransferDbContext, TransferDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
