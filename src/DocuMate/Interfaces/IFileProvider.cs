using CSharpFunctionalExtensions;
using DocuMate.Data.Models;
using DocuMate.Data.Shared;

namespace DocuMate.Interfaces;

public interface IFileProvider
{
    Task<Result<string, Error>> GetFile(GetFileData fileData, CancellationToken cancellationToken = default);
}