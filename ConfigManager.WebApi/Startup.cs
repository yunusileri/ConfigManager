using ConfigManager.Business.Abstract;
using ConfigManager.Business.Concrete;
using ConfigManager.Business.Extension;
using ConfigManager.Core.Cache;
using ConfigManager.Core.Config;
using ConfigManager.Core.MessageBroker;
using ConfigManager.DataAccess.Abstract;
using ConfigManager.DataAccess.Concrete;
using ConfigManager.DataAccess.Config;
using ConfigManager.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ConfigManager.WebApi;

public class Startup
{
    public static IConfiguration Configuration { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void Configure(WebApplication app)
    {
        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseCors(cors =>
        {
            cors.AllowAnyOrigin();
            cors.AllowAnyMethod();
            cors.AllowAnyHeader();
        });

        


        app.MapControllers();
        AutoMigrate.Migrate();
        SeedData.Sync();
        app.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.None;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var connectionStrings = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>() ??
                                throw new Exception("Config missing key ConnectionStrings");

        var redisSettings = Configuration.GetSection("Redis").Get<RedisSettings>() ??
                            throw new Exception("Config missing key RedisSettings");

        var rabbitSettings = Configuration.GetSection("Rabbit").Get<RabbitSettings>() ??
                             throw new Exception("Config missing key RabbitSettings");


        services.AddSingleton(connectionStrings);

        services.AddSingleton<ICacheService>(new RedisService($"{redisSettings.Host}:{redisSettings.Port}"));

        var rabbitConnStr = RabbitConnStrGenerator.GenerateConnStr(rabbitSettings);
        var rabbitClient = new RabbitMqClient(rabbitConnStr);
        services.AddSingleton<IMessageBrokerClient>(rabbitClient);

        services.AddScoped<IConfigValueRepository, ConfigValueRepository>();
        services.AddScoped<IConfigValueService, ConfigValueService>();
    }
}