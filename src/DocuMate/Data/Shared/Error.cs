namespace DocuMate.Data.Shared;

public class Error
{
    public Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }
    
    
    public string Code { get; }
    
    public string Message { get; }
    
    public ErrorType Type { get; }
    
    public string? InvalidField { get; }
    
    public static Error Failure(string code, string message) =>
        new (code, message, ErrorType.Failure);
    
    public static Error NotFound(string code, string message) =>
        new (code, message, ErrorType.NotFound);
}

public enum ErrorType
{
    NotFound,
    Failure,
}