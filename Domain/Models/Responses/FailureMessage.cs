namespace Domain.Models.Responses;

public class FailureMessage
{
    public FailureMessage() { }

    public FailureMessage(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; set; }
    public string Message { get; set; }
}