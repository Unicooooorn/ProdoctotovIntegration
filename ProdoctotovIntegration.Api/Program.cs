using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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

var authenticationOptions =
    builder.Configuration.GetSection(AuthenticationOptions.Position).Get<AuthenticationOptions>() ??
    throw new Exception("Failed to find Authentication options");

builder.Services.AddOptions()
    .Configure<AuthenticationOptions>(o => o.Token = authenticationOptions.Token)
    .AddAuthentication(ApiKeyAuthenticationOptions.AuthenticationScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.AuthenticationScheme, _ => {});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(GetScheduleRequest))!));

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(ApiKeyAuthenticationOptions.AuthenticationScheme, policy =>
        policy.RequireAuthenticatedUser());
});

builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
builder.Services
    .AddApiVersioning(opt =>
    {
        opt.ReportApiVersions = true;
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        opt.AssumeDefaultVersionWhenUnspecified = true;
    });

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo{Title = "ProdoctorovIntegration.Api", Version = "v1"});

    opt.AddSecurityDefinition(authenticationOptions.Token,
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