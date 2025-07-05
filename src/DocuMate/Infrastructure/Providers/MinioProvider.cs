using CSharpFunctionalExtensions;
using DocuMate.Data.Models;
using DocuMate.Data.Shared;
using Minio;
using Minio.DataModel.Args;
using IFileProvider = DocuMate.Interfaces.IFileProvider;

namespace DocuMate.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(
        IMinioClient minioClient,
        ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> GetFile(
        GetFileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExists = await IsBucketExists(fileData.BucketName, cancellationToken);

            if (!bucketExists)
            {
                _logger.LogWarning("Bucket does not exists in minio");

                return Error.Failure("bucket.not.exists", "Bucket does not exists");
            }

            var fileName = fileData.Id + ".md";

            var objectExistsArgs = new StatObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileName);

            await _minioClient.StatObjectAsync(objectExistsArgs, cancellationToken);

            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithObject(fileName)
                .WithExpiry(60 * 60 * 24);

            var result = await _minioClient.PresignedGetObjectAsync(getObjectArgs);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get file in minio");
            
            return Error.Failure("file.get", "Fail to get file in minio");
        }
    }
    
    private async Task<bool> IsBucketExists(string bucketName, CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        
        return await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
    }
}