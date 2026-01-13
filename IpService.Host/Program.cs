using Autofac;
using Autofac.Extensions.DependencyInjection;
using IpService.Contracts;
using IpService.Dal;
using IpService.Dal.Ef;
using IpService.Host;
using IpService.Service;
using IpService.Service.Consumers;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)))
    .Configure<KafkaSection>(builder.Configuration.GetSection(nameof(KafkaSection)))
    .Configure<KafkaConfig<UserIpEventMessage>>(builder.Configuration.GetSection($"KafkaConfigs:UserIp"))
    .AddDbContext<UserIpContext>((prov, o) =>
    {
        var connectionStings = prov.GetRequiredService<IOptions<ConnectionStrings>>().Value;
        var connectionString = connectionStings.Default;
        o.UseNpgsql(connectionString);
    })
    .AddProblemDetails()
    .AddControllers()
    .AddApplicationPart(Assembly.GetEntryAssembly()!)
    .AddControllersAsServices()
    .AddJsonOptions(json =>
    {
        json.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        json.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        json.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        json.JsonSerializerOptions.WriteIndented = false;
    })
    .AddMvcOptions(o =>
    {
        o.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
    });

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureServices(services =>
    {
        services.AddHostedService<BackgroundConsumer<string, UserIpEventMessage>>();
    })
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();

        var configuration = MediatRConfigurationBuilder
            .Create(string.Empty, applicationAssembly)
            .Build();
        containerBuilder.RegisterMediatR(configuration);

        containerBuilder.RegisterModule<HostModule>();
    });

SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions.ServiceCollectionExtensions
    .AddFluentValidationAutoValidation(builder.Services);

SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions.ServiceCollectionExtensions
    .AddFluentValidationAutoValidation(builder.Services);

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter Access-Token:",
        Name = "Access-Token",
        Type = SecuritySchemeType.ApiKey,
    });

    c.AddSecurityRequirement(document =>
    {
        OpenApiSecuritySchemeReference? schemeRef = new("Bearer");
        OpenApiSecurityRequirement? requirement = new()
        {
            [schemeRef] = []
        };
        return requirement;
    });
});

builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);
builder.Services.AddExceptionHandler<AllExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(builderCors =>
{
    builderCors.SetIsOriginAllowed(_ => true);
    builderCors.AllowAnyHeader();
    builderCors.AllowAnyMethod();
    builderCors.AllowCredentials();
});

if (!app.Environment.IsProduction())
{
    app.UseSwagger(o =>
    {
        o.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
        });
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseRouting();

app.MapControllers();

// 5. Run the application
await app.RunWithMigrationAsync();

public partial class Program { }