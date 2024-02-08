# Discord.Addons.Extensions

I was tired of copying and pasting common extensions I use between [Discord.Net](https://github.com/discord-net/Discord.Net) projects so I extracted them into a shared library.

This is a still a work-in-progress but features are ready to use.

---

### StringExtensions

- `<string>.ToHeading(int? level)`
- `<string>.ToH1()`
- `<string>.ToH2()`
- `<string>.ToH3()`
- `<string>.ToSpoiler()`
- `<string>.ToItalic(bool useUnderscores, bool bold)`
- `<string>.ToBold(bool italic)`
- `<string>.ToUnderline(bool bold, bool italic)`
- `<string>.ToStrikethrough()`
- `<string>.HideLinkEmbed()`
- `<string>.ToQuote()`
- `<string>.ToBlockQuote()`
- `<string>.ToHyperlink(string url, string? title)`
- `<string>.ToHyperlink(Uri url, string? title)`
- `<string>.ToInlineCode()`
- `<string>.ToCodeBlock(string? language)`
- `<string>.IsValidDiscordToken(TokenType? type)`
- `<string>.TryValidateDiscordToken(out string? result, TokenType? type)`

### LongExtensions

- `<ulong>.ToUserMention(bool? ping)`
- `<ulong>.ToChannelMention()`
- `<ulong>.ToRoleMention()`

### EnumerableExtensions

- `<string[]>.ToUnorderedList(bool useAsterisks)`
- `<string[]>.ToOrderedList()`

### DateTimeExtensions/DateTimeOffsetExtensions

- `<date>.ToTimestampTag(TimestampTagStyles style)`
- `<date>.ToTimestampTag(string style)`

### DiscordSocketClientExtensions

- `<client>.GetCategoryChannelAsync(ulong channelId)`
- `<client>.GetTextChannelAsync(ulong channelId)`
- `<client>.GetVoiceChannelAsync(ulong channelId)`
- `<client>.GetForumChannelAsync(ulong channelId)`
- `<client>.GetThreadChannelAsync(ulong channelId)`
- `<client>.GetOrCreateWebhookAsync(ulong channelId, string name, string? auditLogReason)`
- `<client>.CreateWebhookAsync(ulong channelId, string name, string? auditLogReason)`
- `<client>.GetWebhookAsync(ulong channelId, string name)`
