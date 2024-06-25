using ConfigManager.Core.DependencyInjection;
using ConfigManager.WebApi;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
InstanceFactory.Configure(builder.Services.BuildServiceProvider());
var app = builder.Build();

startup.Configure(app);