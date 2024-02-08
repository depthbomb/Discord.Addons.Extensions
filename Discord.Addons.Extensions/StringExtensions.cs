using System.Text;
using System.Text.RegularExpressions;

namespace Discord.Addons.Extensions;

public static class StringExtensions
{
    private static readonly Regex CodeBlockBackticksPattern = new("`{3}", RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown heading with the section <paramref name="level"/>.
    /// </summary>
    /// <param name="input">The string to format into a markdown heading.</param>
    /// <param name="level">The section level of the heading</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the section <paramref name="level"/> is not <c>1</c>, <c>2</c>, or <c>3</c>.</exception>
    public static string ToHeading(this string input, int level = 1)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(level, 3, nameof(level));
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 1, nameof(level));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(level, nameof(level));

        var prefix = level switch
        {
            1 => "#",
            2 => "##",
            3 => "###",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return new StringBuilder()
               .Append(prefix)
               .Append(' ')
               .Append(input)
               .ToString();
    }

    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown heading with a section level of <c>1</c> (<c>h1</c>).
    /// </summary>
    /// <param name="input">The string to format into a markdown heading with a section level of <c>1</c> (<c>h1</c>).</param>
    public static string ToH1(this string input) => input.ToHeading();
    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown heading with a section level of <c>2</c> (<c>h2</c>).
    /// </summary>
    /// <param name="input">The string to format into a markdown heading with a section level of <c>2</c> (<c>h2</c>).</param>
    public static string ToH2(this string input) => input.ToHeading(2);
    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown heading with a section level of <c>3</c> (<c>h3</c>).
    /// </summary>
    /// <param name="input">The string to format into a markdown heading with a section level of <c>3</c> (<c>h3</c>).</param>
    public static string ToH3(this string input) => input.ToHeading(3);

    /// <summary>
    /// Formats the <paramref name="input"/> into spoiler text.
    /// </summary>
    /// <param name="input">The string to format into spoiler text.</param>
    public static string ToSpoiler(this string input) => WrapString(input, "||");

    /// <summary>
    /// Italicizes the <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The string to italicize.</param>
    /// <param name="useUnderscores">Whether to use <c>_</c> instead of <c>*</c> in the resulting markdown.</param>
    /// <param name="bold">Whether to embolden the resulting markdown.</param>
    public static string ToItalic(this string input, bool useUnderscores = true, bool bold = false)
    {
        var formatted = useUnderscores ? WrapString(input, '_') : WrapString(input, '*');
        if (bold)
        {
            formatted = WrapString(formatted, useUnderscores ? "**" : "*");
        }

        return formatted;
    }

    /// <summary>
    /// Emboldens the <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The string to embolden.</param>
    /// <param name="italic">Whether to italicize the resulting markdown.</param>
    public static string ToBold(this string input, bool italic = false)
    {
        var formatted = WrapString(input, "**");
        if (italic)
        {
            formatted = WrapString(formatted, '*');
        }

        return formatted;
    }

    /// <summary>
    /// Underlines the <paramref name="input"/>.
    /// </summary>
    /// <param name="input">The string to underline.</param>
    /// <param name="bold">Whether to embolden the resulting markdown.</param>
    /// <param name="italic">Whether to italicize the resulting markdown.</param>
    public static string ToUnderline(this string input, bool bold = false, bool italic = false)
    {
        var formatted = WrapString(input, "__");
        if (bold)
        {
            formatted = WrapString(formatted, "**");
        }
        
        if (italic)
        {
            formatted = WrapString(formatted, '*');
        }

        return formatted;
    }

    /// <summary>
    /// Strikes through the specified <paramref name="input"/> string.
    /// </summary>
    /// <param name="input">The string to be struck through.</param>
    public static string ToStrikethrough(this string input) => WrapString(input, "~~");

    /// <summary>
    /// Formats the <paramref name="input"/> URL to not show an embed.
    /// </summary>
    /// <param name="input">The string URL to hide the embed of.</param>
    public static string HideLinkEmbed(this string input) => new StringBuilder().Append('<')
                                                                                .Append(input)
                                                                                .Append('>')
                                                                                .ToString();

    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown quote.
    /// </summary>
    /// <param name="input">The string to format into a markdown quote.</param>
    public static string ToQuote(this string input) => new StringBuilder().Append("> ")
                                                                          .Append(input)
                                                                          .ToString();

    /// <summary>
    /// Formats the <paramref name="input"/> into a markdown block quote.
    /// </summary>
    /// <param name="input">The string to format into a markdown block quote.</param>
    public static string ToBlockQuote(this string input) => new StringBuilder().Append(">>> ")
                                                                               .Append(input)
                                                                               .ToString();

    /// <summary>
    /// Formats the <paramref name="input"/> URL into a markdown hyperlink (masked link).
    /// </summary>
    /// <param name="input">The string content of the markdown hyperlink.</param>
    /// <param name="url">The URL of the markdown hyperlink.</param>
    /// <param name="title">The title of the markdown hyperlink.</param>
    public static string ToHyperlink(this string input, string url, string? title = null)
    {
        var sb = new StringBuilder().Append('[')
                                    .Append(input)
                                    .Append(']')
                                    .Append(url);

        if (title != null)
        {
            sb.Append(' ')
              .Append('"')
              .Append(title)
              .Append('"');
        }

        return sb.Append(')')
                 .ToString();
    }

    /// <summary>
    /// Formats the <paramref name="input"/> URL into a markdown hyperlink (masked link).
    /// </summary>
    /// <param name="input">The string content of the markdown hyperlink.</param>
    /// <param name="url">The URL of the markdown hyperlink.</param>
    /// <param name="title">The title of the markdown hyperlink.</param>
    public static string ToHyperlink(this string input, Uri url, string? title = null) 
        => input.ToHyperlink(url.ToString(), title);

    /// <summary>
    /// Formats the <paramref name="input"/> to markdown inline code.
    /// </summary>
    /// <param name="input">The string to format into a markdown inline code string.</param>
    public static string ToInlineCode(this string input) => WrapString(input, '`');

    /// <summary>
    ///     Formats the <paramref name="input"/> to a markdown code block with the provided <paramref name="language"/>.
    /// </summary>
    /// <param name="input">The string to format into a markdown code block.</param>
    /// <param name="language">The language of the code block (<c>md</c> by default).</param>
    public static string ToCodeBlock(this string input, string language = "md") => new StringBuilder().Append("```")
                                                                                                      .AppendLine(input)
                                                                                                      .Append("```")
                                                                                                      .ToString();

    /// <summary>
    ///     Whether the <paramref name="input"/> is a valid Discord token based on the provided
    ///     <paramref name="type"/>.
    /// </summary>
    /// <param name="input">The string to validate.</param>
    /// <param name="type">The type of token to validate (bot by default).</param>
    public static bool IsValidDiscordToken(this string input, TokenType type = TokenType.Bot)
    {
        try
        {
            TokenUtils.ValidateToken(type, input);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to check whether the <paramref name="input"/> is a valid Discord token based on the provided
    /// <paramref name="type"/>. The return value indicates whether the <paramref name="result"/> is valid.
    /// </summary>
    /// <param name="input">The string to validate.</param>
    /// <param name="result">When this method returns, contains the string value contained in <paramref name="input"/>, if the validation succeeded, or <c>null</c> if the validation failed. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <param name="type">The type of token to validate (bot by default)</param>
    public static bool TryValidateDiscordToken(this string input, out string? result, TokenType type = TokenType.Bot)
    {
        result = input;

        if (input.IsValidDiscordToken(type))
        {
            return true;
        }

        result = null;

        return false;
    }

    private static string WrapString(string input, string wrapping) => new StringBuilder().Append(wrapping)
                                                                                          .Append(input)
                                                                                          .Append(wrapping)
                                                                                          .ToString();
    
    private static string WrapString(string input, char wrapping) => new StringBuilder().Append(wrapping)
                                                                                        .Append(input)
                                                                                        .Append(wrapping)
                                                                                        .ToString();
}
