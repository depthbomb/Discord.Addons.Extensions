namespace Discord.Addons.Extensions.Exceptions;

public class WebhookExistsException : Exception
{
    public WebhookExistsException() { }
    public WebhookExistsException(string message) : base(message) { }
    public WebhookExistsException(string message, Exception inner) : base(message, inner) { }
}
