using DocuMate.Data.Options;
using DocuMate.Infrastructure.MongoDataAccess;
using DocuMate.Infrastructure.Providers;
using DocuMate.Interfaces;
using Minio;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

namespace DocuMate;

public static class DependencyInjection
{
    public static IServiceCollection AddDocuMateServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddLogging(configuration)
            .AddMinio(configuration)
            .AddMongoDb(configuration)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddLogging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(configuration.GetConnectionString("Seq") ?? throw new ArgumentException("Seq"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing configuration minio");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
    
    private static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));

        services.AddScoped<FileMongoDbContext>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFilesRepository, FilesRepository>();

        return services;
    }
}