namespace DocuMate.Endpoints;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app);
}