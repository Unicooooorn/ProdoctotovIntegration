using ProdoctorovIntegration.Application.Authentication;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Infrastructure.Configuration;
using Serilog;

var logger = new LoggerConfiguration().CreateLogger();
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(logger);

builder.Services.AddEfCore(
    builder.Configuration.GetConnectionString("ServiceDataContext") ??
    throw new Exception("Failed to find connection string"));

builder.Services.AddOptions()
    .Configure<AuthenticationOptions>(o => o.Token = builder.Configuration.GetSection("Authentication:AuthenticationToken").Get<string>())
    .AddAuthentication(ApiKeyAuthenticationOptions.AuthenticationScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.AuthenticationScheme, _ => {});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(ApiKeyAuthenticationOptions.AuthenticationScheme, policy =>
        policy.RequireAuthenticatedUser());
});


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