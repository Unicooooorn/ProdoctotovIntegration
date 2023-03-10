using System.Reflection;
using MediatR;
using Microsoft.OpenApi.Models;
using ProdoctorovIntegration.Application.Authentication;
using ProdoctorovIntegration.Application.Options.Authentication;
using ProdoctorovIntegration.Application.Requests.Schedule;
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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(GetScheduleRequest))!));

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(ApiKeyAuthenticationOptions.AuthenticationScheme, policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Enter here your Token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "JWT",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddApiVersioning();

var app = builder.Build();

if(app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.ApplyPendingMigrations();

app.UsePathBase(builder.Configuration["PathBase"]);

app.UseRouting();
app.MapControllers();

app.UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


await app.RunAsync();