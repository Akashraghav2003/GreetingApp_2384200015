using Microsoft.OpenApi.Models;
using ModelLayer.Model;
using NLog;
using NLog.Web;
using System.Reflection;


var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.


    builder.Services.AddControllers();
    builder.Services.AddSingleton<DictionaryForMethod>();


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "HelloGreetingApplication",
            Version = "v1",
            Description = "API for Greeting App",
            Contact = new OpenApiContact
            {
                Name = "Akash singh",
                Email = "akash.si8273@gmail.com",

            }
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    });
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    var app = builder.Build();

    // Configure the HTTP request pipeline.


    app.Use(async (context, next) =>
    {
        logger.Info($"Request: {context.Request.Method} {context.Request.Path}");
        await next.Invoke();
        logger.Info($"Response: {context.Response.StatusCode}");
    });
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.Error("Error occurred " + ex);
}
finally
{
    NLog.LogManager.Shutdown();
}
