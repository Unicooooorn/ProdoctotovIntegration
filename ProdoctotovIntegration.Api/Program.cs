using Serilog;

var logger = new LoggerConfiguration().CreateLogger();
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(logger);