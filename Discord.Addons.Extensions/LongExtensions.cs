using System.Text;

namespace Discord.Addons.Extensions;

public static class LongExtensions
{
    /// <summary>
    /// Formats a ulong <paramref name="id"/> into a user mention.
    /// </summary>
    /// <param name="id">The ID to format into a user mention.</param>
    /// <param name="ping">Whether the resulting mention should ping the target user.</param>
    public static string ToUserMention(this ulong id, bool ping = true)
    {
        var sb = new StringBuilder().Append('<')
                                    .Append('@');

        if (ping)
        {
            sb.Append('!');
        }

        return sb.Append(id)
                 .Append('>')
                 .ToString();
    }
    
    /// <summary>
    /// Formats a ulong <paramref name="id"/> into a channel mention.
    /// </summary>
    /// <param name="id">The ID to format into a channel mention.</param>
    public static string ToChannelMention(this ulong id) => new StringBuilder().Append('<')
                                                                               .Append('#')
                                                                               .Append(id)
                                                                               .Append('>')
                                                                               .ToString();
    
    /// <summary>
    /// Formats a ulong <paramref name="id"/> into a role mention.
    /// </summary>
    /// <param name="id">The ID to format into a role mention.</param>
    public static string ToRoleMention(this ulong id) => new StringBuilder().Append('<')
                                                                            .Append('@')
                                                                            .Append('&')
                                                                            .Append(id)
                                                                            .Append('>')
                                                                            .ToString();
}
