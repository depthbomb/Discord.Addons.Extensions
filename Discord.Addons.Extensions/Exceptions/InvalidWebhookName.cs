namespace Discord.Addons.Extensions.Exceptions;

public class InvalidWebhookNameException : Exception
{
    public InvalidWebhookNameException() { }
    public InvalidWebhookNameException(string message) : base(message) { }
    public InvalidWebhookNameException(string message, Exception inner) : base(message, inner) { }
}
