using Microsoft.OpenApi.Models;
using ModelLayer.Model;
using NLog;
using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Context;
using Microsoft.EntityFrameworkCore;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.


    builder.Services.AddControllers();

    var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
    builder.Services.AddDbContext<GreetingContext>(option => option.UseSqlServer(connectionString));


    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<GreetingContext>();

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
