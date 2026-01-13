using Microsoft.Extensions.Hosting;

namespace IpService.Host
{
    internal sealed class DefaultStartup
    {
        private readonly IHostEnvironment _env;

        public DefaultStartup(IHostEnvironment env)
        {
            _env = env;
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    if (services == null) throw new ArgumentNullException(nameof(services));

        //    var mvcBuilder = services
        //        .AddProblemDetails()
        //        .AddControllers()
        //        .AddApplicationPart(Assembly.GetEntryAssembly()!)
        //        .AddControllersAsServices()
        //        .AddJsonOptions(json =>
        //        {
        //            json.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //            json.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        //            json.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        //            json.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        //            json.JsonSerializerOptions.WriteIndented = false;
        //        })
        //        .AddMvcOptions(o =>
        //        {
        //            o.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
        //        });

        //    services.AddCorrelationId(o =>
        //    {
        //        o.IncludeInResponse = true;
        //        o.AddToLoggingScope = true;
        //        o.LoggingScopeKey = CorrelationId;
        //    });

        //    SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions.ServiceCollectionExtensions
        //        .AddFluentValidationAutoValidation(services);

        //    SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions.ServiceCollectionExtensions
        //        .AddFluentValidationAutoValidation(services);

        //    //services.AddHttpLogging(opt => opt.LoggingFields = opt.LoggingFields | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody);

        //    services.AddDefaultCorrelationId(o =>
        //    {
        //        o.IncludeInResponse = true;
        //        o.UpdateTraceIdentifier = true;
        //    })
        //        .AddMemoryCache()
        //        .AddHttpContextAccessor()
        //        .AddCors()
        //        .TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();


        //    foreach (var mvcBuilderAction in hostConfiguration.MvcBuilderActions)
        //    {
        //        mvcBuilderAction(mvcBuilder);
        //    }

        //    services.AddSwaggerGen(c =>
        //    {
        //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //        {
        //            In = ParameterLocation.Header,
        //            Description = "Enter Access-Token:",
        //            Name = "Access-Token",
        //            Type = SecuritySchemeType.ApiKey,
        //        });

        //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //        {
        //            {
        //                new OpenApiSecurityScheme
        //                {
        //                    Reference = new OpenApiReference
        //                    {
        //                        Type = ReferenceType.SecurityScheme,
        //                        Id = "Bearer"
        //                    }
        //                },
        //                new string[] { }
        //            }
        //        });
        //    });

        //    services.AddExceptionHandler<AllExceptionHandler>();
        //}

        //public void Configure(
        //    IApplicationBuilder app,
        //    IApiVersionDescriptionProvider apiVersionProvider)
        //{
        //    app.UseExceptionHandler();
        //    app.UseCorrelationId();

        //    //app.UseHttpLogging();

        //    foreach (var applicationBuilderAction in hostConfiguration.ApplicationBuilderActions)
        //    {
        //        applicationBuilderAction(app);
        //    }

        //    app.UseCors(builder =>
        //    {
        //        builder.SetIsOriginAllowed(_ => true);
        //        builder.AllowAnyHeader();
        //        builder.AllowAnyMethod();
        //        builder.AllowCredentials();
        //    });

        //    if (!_env.IsProduction())
        //    {
        //        app.UseSwagger(o =>
        //        {
        //            o.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        //            {
        //                swaggerDoc.Servers = new List<OpenApiServer> { new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
        //            });
        //        });

        //        app.UseSwaggerUI(c =>
        //        {
        //            foreach (var apiVersion in apiVersionProvider.ApiVersionDescriptions.OrderBy(version =>
        //                         version.ToString()))
        //            {
        //                c.SwaggerEndpoint(
        //                    $"{apiVersion.GroupName}/swagger.json",
        //                    $"Api {apiVersion.GroupName} for {Assembly.GetEntryAssembly()!.GetName().Name}"
        //                );
        //            }

        //            c.EnableFilter();
        //        });
        //    }

        //    app.UseRouting();

        //    app.UseEndpoints(endpoints => endpoints.MapControllers());

        //    var logger = app.ApplicationServices.GetRequiredService<ILogger<DefaultStartup>>();

        //    TaskScheduler.UnobservedTaskException += (_, e) =>
        //    {
        //        logger.LogCritical(e.Exception, "UnobservedTaskException");
        //    };

        //    AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        //    {
        //        logger.LogCritical((Exception)e.ExceptionObject, "CurrentDomain.UnhandledException");
        //    };
        //}
    }
}
