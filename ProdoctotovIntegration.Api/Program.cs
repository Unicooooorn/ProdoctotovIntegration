using ProdoctorovIntegration.Infrastructure.Configuration;
using Serilog;

var logger = new LoggerConfiguration().CreateLogger();
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(logger);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if(app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.ApplyPendingMigrations();

app.UsePathBase(builder.Configuration["PathBase"]);

app.UseRouting();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.RunAsync();