using challengeFiap.Infrastruture;
using challengeFiap.Infrastruture.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting web host");


    var builder = WebApplication.CreateBuilder(args);

    //Parte de Banco de dados

    var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

    //Serviços externos

    //Parte de HealthCheck

    builder.Services.AddHealthChecks()
        .AddCheck("self", () => Microsoft
        .Extensions
        .Diagnostics
        .HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live" })
        .AddDbContextCheck<AppDbContext>("oracle", tags: new[] { "ready" })
        .AddUrlGroup(
        new Uri("https://jsonplaceholder.typicode.com/posts/1"),
        name: "jsonplaceholder",
        tags: new[] { "ready" });

    //Parte de Banco de dados

    builder.Services.AddDbContext<AppDbContext>(
        options =>
        options.UseOracle(connectionString));

    //Parte de Controllers

    builder.Services.AddControllers();

    //PARA VER O TEST FUNCIONAR
    builder.Services.AddAuthentication("ApiKey")
        .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler >

    //Parte de Swagger

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    }
    );

    //tracing e metrica
    builder.Services.AddInfrastructure();

    //Parte de Serialização

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge CYLVO");
            options.RoutePrefix = "swagger";
        });
    }


    app.UseHttpsRedirection();

    app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("live")
    });

    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

