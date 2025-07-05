using DocuMate;
using DocuMate.Endpoints;
using DocuMate.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDocuMateServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5177")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();
