using CSharpFunctionalExtensions;
using DocuMate.Data.Models;
using DocuMate.Data.Shared;
using DocuMate.Interfaces;
using MongoDB.Driver;

namespace DocuMate.Infrastructure.MongoDataAccess;

public class FilesRepository : IFilesRepository
{
    private readonly ILogger<FilesRepository> _logger;
    private readonly FileMongoDbContext _dbContext;
    
    public FilesRepository(
        ILogger<FilesRepository> logger,
        FileMongoDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<Result<List<FileData>, Error>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var files = await _dbContext.Files.Find(FilterDefinition<FileData>.Empty).ToListAsync(cancellationToken);

            return files;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get files from mongo");

            return Error.Failure("get.files.mongo", "Fail to get files from mongo");
        }
    }

    public async Task<Result<FileData, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var file = await _dbContext.Files.Find(f => f.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (file is null)
                return Error.NotFound("record.not.found", "File not found");

            return file;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to get file from mongo");

            return Error.Failure("get.file.mongo", "Fail to get file from mongo");
        }
    }
}