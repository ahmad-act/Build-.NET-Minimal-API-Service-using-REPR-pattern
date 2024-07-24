using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using BookInformationService;
using BookInformationService.DatabaseContext;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenApi;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using BookInformationService.BookInformation.List;
using BookInformationService.BookInformation.Get;
using BookInformationService.BookInformation.Create;
using BookInformationService.BookInformation.Update;
using BookInformationService.BookInformation.Delete;
using BookInformationService.BookInformation.Facade.List;
using BookInformationService.BookInformation.Facade.Get;
using BookInformationService.BookInformation.Facade.Create;
using BookInformationService.BookInformation.Facade.Update;
using BookInformationService.BookInformation.Facade.Delete;

var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");


AppSettings? appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

var emailInfo = new EmailSinkOptions
{
    Subject = new MessageTemplateTextFormatter(appSettings.EmailSubject, null),
    Port = appSettings.Port,
    From = appSettings.FromEmail,
    To = new List<string>() { appSettings.ToEmail },
    Host = appSettings.MailServer,
    //EnableSsl = appSettings.EnableSsl,
    Credentials = new NetworkCredential(appSettings.FromEmail, appSettings.EmailPassword)
};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Serilog\\log_.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 1000000, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 24, flushToDiskInterval: TimeSpan.FromSeconds(1))
    //.WriteTo.Email(emailInfo)                           
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
});

builder.Services.AddApiVersioning(option =>
{
    option.DefaultApiVersion = new ApiVersion(1);
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Extensions["Service"] = "BookInformationService";
    };
});

// EF
builder.Services.AddDbContext<SystemDbContext>(options =>
            options.UseSqlite(defaultConnectionString));

// For Dependency Injection (DI)
// List
builder.Services.AddScoped<IListBookInformationDL, ListBookInformationDL>();
builder.Services.AddScoped<IListBookInformationBL, ListBookInformationBL>();
// Get
builder.Services.AddScoped<IGetBookInformationDL, GetBookInformationDL>();
builder.Services.AddScoped<IGetBookInformationBL, GetBookInformationBL>();
// Create
builder.Services.AddScoped<ICreateBookInformationDL, CreateBookInformationDL>();
builder.Services.AddScoped<ICreateBookInformationBL, CreateBookInformationBL>();
// Update
builder.Services.AddScoped<IUpdateBookInformationDL, UpdateBookInformationDL>();
builder.Services.AddScoped<IUpdateBookInformationBL, UpdateBookInformationBL>();
// Delete
builder.Services.AddScoped<IDeleteBookInformationDL, DeleteBookInformationDL>();
builder.Services.AddScoped<IDeleteBookInformationBL, DeleteBookInformationBL>();


var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    dbContext.Database.Migrate();
}

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(1)
    .HasApiVersion(2)
    .ReportApiVersions()
    .Build();

RouteGroupBuilder routeGroupBuilder = app
    .MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

routeGroupBuilder.MapListBookInformationEndpoint();
routeGroupBuilder.MapGetBookInformationEndpoint();
routeGroupBuilder.MapCreateBookInformationEndpoint();
routeGroupBuilder.MapUpdateBookInformationEndpoint();
routeGroupBuilder.MapDeleteBookInformationEndpoint();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature is not null)
        {
            Console.WriteLine($"Error: {contextFeature.Error}");
            Log.Fatal(contextFeature.Error, "Unhandled exception occurred.");

            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            });
        }
    });
});

app.Run();