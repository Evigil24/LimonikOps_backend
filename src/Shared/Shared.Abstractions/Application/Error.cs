namespace LimonikOne.Shared.Abstractions.Application;

public record Error(string Code, string Message)
{
    public static Error Validation(string code, string message) => new(code, message);

    public static Error NotFound(string code, string message) => new(code, message);

    public static Error Conflict(string code, string message) => new(code, message);
}
