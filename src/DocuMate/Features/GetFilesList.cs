using DocuMate.Endpoints;
using DocuMate.Interfaces;
using FileInfo = DocuMate.Data.Models.Response.FileInfo;

namespace DocuMate.Features;

public static class GetFilesList
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files", Handler);
        }
    }

    private static async Task<IResult> Handler(
        IFilesRepository repository,
        CancellationToken cancellationToken = default)
    {
        var files = await repository.GetAll(cancellationToken);

        if (files.IsFailure)
            return Results.BadRequest(files.Error.Message);

        var filesByProjects = files.Value
            .GroupBy(file => file.BucketName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(f => new FileInfo(f.Id, f.FilePath)));

        return Results.Ok(filesByProjects);
    }
}