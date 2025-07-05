using CSharpFunctionalExtensions;
using DocuMate.Data.Models;
using DocuMate.Data.Shared;

namespace DocuMate.Interfaces;

public interface IFilesRepository
{
    Task<Result<List<FileData>, Error>> GetAll(CancellationToken cancellationToken = default);

    Task<Result<FileData, Error>> GetById(Guid id, CancellationToken cancellationToken = default);
}