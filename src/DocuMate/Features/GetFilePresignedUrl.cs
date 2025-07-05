using DocuMate.Data.Models;
using DocuMate.Data.Models.Response;
using DocuMate.Endpoints;
using DocuMate.Interfaces;
using IFileProvider = DocuMate.Interfaces.IFileProvider;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace DocuMate.Features;

public class GetFilePresignedUrl
{
    public class  Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{id:guid}/presigned", Handler);
        }

        private static async Task<IResult> Handler(
            Guid id,
            IFilesRepository repository,
            IFileProvider provider,
            CancellationToken cancellationToken = default)
        {
            var file = await repository.GetById(id, cancellationToken);

            if (file.IsFailure)
                return Results.BadRequest(file.Error.Message);

            var url = await provider.GetFile(
                new GetFileData(file.Value.Id, file.Value.BucketName),
                cancellationToken);

            if (url.IsFailure)
                return Results.BadRequest(file.Error.Message);

            return Results.Ok(new GetFilePresignedIUrlResponse(file.Value.FilePath, url.Value));
        }
    }
}