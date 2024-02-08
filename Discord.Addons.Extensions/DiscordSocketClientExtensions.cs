using Discord.Webhook;
using Discord.WebSocket;
using Discord.Addons.Extensions.Exceptions;

namespace Discord.Addons.Extensions;

public static class DiscordSocketClientExtensions
{
    private static readonly IReadOnlyList<string> BlacklistedWebhookNameSubstrings = ["clyde", "discord"];
    
    /// <summary>
    /// Attempts to retrieve a <see cref="SocketCategoryChannel"/> from the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The snowflake ID of the channel.</param>
    /// <returns>The <see cref="SocketCategoryChannel"/> if it exists; <c>null</c> otherwise.</returns>
    public static async Task<SocketCategoryChannel?> GetCategoryChannelAsync(this DiscordSocketClient client, ulong channelId) 
        => await client.GetChannelAsync(channelId) as SocketCategoryChannel;
    
    /// <summary>
    /// Attempts to retrieve a <see cref="SocketTextChannel"/> from the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The snowflake ID of the channel.</param>
    /// <returns>The <see cref="SocketTextChannel"/> if it exists; <c>null</c> otherwise.</returns>
    public static async Task<SocketTextChannel?> GetTextChannelAsync(this DiscordSocketClient client, ulong channelId) 
        => await client.GetChannelAsync(channelId) as SocketTextChannel;
    
    /// <summary>
    /// Attempts to retrieve a <see cref="SocketVoiceChannel"/> from the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The snowflake ID of the channel.</param>
    /// <returns>The <see cref="SocketVoiceChannel"/> if it exists; <c>null</c> otherwise.</returns>
    public static async Task<SocketVoiceChannel?> GetVoiceChannelAsync(this DiscordSocketClient client, ulong channelId) 
        => await client.GetChannelAsync(channelId) as SocketVoiceChannel;
    
    /// <summary>
    /// Attempts to retrieve a <see cref="SocketForumChannel"/> from the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The snowflake ID of the channel</param>
    /// <returns>The <see cref="SocketForumChannel"/> if it exists; <c>null</c> otherwise</returns>
    public static async Task<SocketForumChannel?> GetForumChannelAsync(this DiscordSocketClient client, ulong channelId) 
        => await client.GetChannelAsync(channelId) as SocketForumChannel;
    
    /// <summary>
    /// Attempts to retrieve a <see cref="SocketThreadChannel"/> from the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The snowflake ID of the channel.</param>
    /// <returns>The <see cref="SocketThreadChannel"/> if it exists; <c>null</c> otherwise.</returns>
    public static async Task<SocketThreadChannel?> GetThreadChannelAsync(this DiscordSocketClient client, ulong channelId) 
        => await client.GetChannelAsync(channelId) as SocketThreadChannel;

    /// <summary>
    /// Retrieves or creates a <see cref="DiscordWebhookClient"/> for the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The text channel ID to retrieve the webhook of or create the webhook in.</param>
    /// <param name="name">The name of the webhook to retrieve or create with.</param>
    /// <param name="auditLogReason">The guild audit log reason posted when creating a webhook.</param>
    /// <returns>A <see cref="DiscordWebhookClient"/> that was retrieved for or created in the channel.</returns>
    /// <exception cref="Exception">Thrown if the channel is not found or is not a text channel, or if the client has already created a webhook with the <paramref name="name"/> in the channel.</exception>
    /// <exception cref="WebhookExistsException">Thrown if a webhook by the client with the desired name already exists in the channel.</exception>
    /// <exception cref="InvalidWebhookNameException">Thrown if the desired webhook name contains blacklisted substrings (see https://discord.com/developers/docs/resources/webhook#create-webhook).<br/>The occurrence of this exception in this method is an edge case.</exception>
    public static async Task<DiscordWebhookClient> GetOrCreateWebhookAsync(
        this DiscordSocketClient client,
        ulong                    channelId,
        string                   name,
        string?                  auditLogReason = null
    )
    {
        var existingWebhook = await client.GetWebhookAsync(channelId, name);
        if (existingWebhook == null)
        {
            return await client.CreateWebhookAsync(channelId, name, auditLogReason);
        }

        return existingWebhook;
    }

    /// <summary>
    /// Creates a <see cref="DiscordWebhookClient"/> for the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The text channel ID to create the webhook in.</param>
    /// <param name="name">The name of the webhook to create.</param>
    /// <param name="auditLogReason">The guild audit log reason for creating the webhook.</param>
    /// <returns>A <see cref="DiscordWebhookClient"/> for the channel.</returns>
    /// <exception cref="Exception">Thrown if the channel is not found or is not a text channel.</exception>
    /// <exception cref="WebhookExistsException">Thrown if a webhook by the client with the desired name already exists in the channel.</exception>
    /// <exception cref="InvalidWebhookNameException">Thrown if the desired webhook name contains blacklisted substrings (see https://discord.com/developers/docs/resources/webhook#create-webhook).</exception>
    public static async Task<DiscordWebhookClient> CreateWebhookAsync(
        this DiscordSocketClient client,
        ulong                    channelId,
        string                   name,
        string?                  auditLogReason = null
    )
    {
        AssertWebhookNameValid(name);

        var channel = await client.GetTextChannelAsync(channelId);
        if (channel == null)
        {
            throw new Exception($"Could not retrieve text channel by ID {channelId}");
        }

        var channelWebhooks = await channel.GetWebhooksAsync();
        var existingWebhook = channelWebhooks.FirstOrDefault(x => x.Name == name && x.Creator == client.CurrentUser);
        if (existingWebhook != null)
        {
            throw new WebhookExistsException($"Webhook \"{name}\" already exists for channel ID {channelId}");
        }

        var options = new RequestOptions();
        if (auditLogReason != null)
        {
            options.AuditLogReason = auditLogReason;
        }

        var newWebhook = await channel.CreateWebhookAsync(name, options: options);

        return new DiscordWebhookClient(newWebhook.Id, newWebhook.Token);
    }

    /// <summary>
    /// Retrieves a <see cref="DiscordWebhookClient"/> for the provided <paramref name="channelId"/>.
    /// </summary>
    /// <param name="client"><see cref="DiscordSocketClient"/></param>
    /// <param name="channelId">The text channel ID to retrieve the webhook of.</param>
    /// <param name="name">The name of the webhook to retrieve.</param>
    /// <returns>A <see cref="DiscordWebhookClient"/> if the webhook exists; otherwise, <c>null</c>.</returns>
    /// <exception cref="Exception">Thrown if the channel is not found or is not a text channel.</exception>
    public static async Task<DiscordWebhookClient?> GetWebhookAsync(
        this DiscordSocketClient client,
        ulong                    channelId,
        string                   name
    )
    {
        var channel = await client.GetTextChannelAsync(channelId);
        if (channel == null)
        {
            throw new Exception($"Could not retrieve text channel by ID {channelId}");
        }
        
        var webhooks     = await channel.GetWebhooksAsync();
        var foundWebhook = webhooks.FirstOrDefault(x => x.Name == name && x.Creator == client.CurrentUser);
        if (foundWebhook == null)
        {
            return null;
        }

        return new DiscordWebhookClient(foundWebhook.Id, foundWebhook.Token);
    }

    private static void AssertWebhookNameValid(string name)
    {
        if (BlacklistedWebhookNameSubstrings.Any(x => name.Contains(x, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidWebhookNameException();
        }
    }
}
