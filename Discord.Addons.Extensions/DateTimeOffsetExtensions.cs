namespace Discord.Addons.Extensions;

public static class DateTimeOffsetExtensions
{
        /// <summary>
    /// Formats a <see cref="DateTimeOffset"/> to a Discord timestamp tag string in the provided <paramref name="style"/>.
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <param name="style">The style of timestamp tag (Short Date/Time by default; see example)</param>
    /// <example>
    ///     <code language="cs">
    ///         var date = new DateTimeOffset(...);
    ///         date.ToTimeStampTag(TimestampTagStyles.ShortTime);     // 16:20
    ///         date.ToTimeStampTag(TimestampTagStyles.LongTime);      // 16:20:30
    ///         date.ToTimeStampTag(TimestampTagStyles.ShortDate);     // 20/04/2021
    ///         date.ToTimeStampTag(TimestampTagStyles.LongDate);      // 20 April 2021
    ///         date.ToTimeStampTag(TimestampTagStyles.ShortDateTime); // 20 April 2021 16:20 (default)
    ///         date.ToTimeStampTag(TimestampTagStyles.LongDateTime);  // Tuesday, 20 April 2021 16:20
    ///         date.ToTimestampTag(TimestampTagStyles.Relative);      // 2 months ago
    ///     </code>
    /// </example>
    public static string ToTimestampTag(this DateTimeOffset date, TimestampTagStyles style = TimestampTagStyles.ShortDateTime)
        => new TimestampTag(date, style).ToString();

    /// <summary>
    /// Formats a <see cref="DateTimeOffset"/> to a Discord timestamp tag string in the provided <paramref name="styleString"/>.
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <param name="styleString">The style string of timestamp tag (Short Date/Time by default; see example)</param>
    /// <example>
    ///     <code language="cs">
    ///         var date = new DateTimeOffset(...);
    ///         date.ToTimeStampTag("t"); // 16:20
    ///         date.ToTimeStampTag("T"); // 16:20:30
    ///         date.ToTimeStampTag("d"); // 20/04/2021
    ///         date.ToTimeStampTag("D"); // 20 April 2021
    ///         date.ToTimeStampTag("f"); // 20 April 2021 16:20 (default)
    ///         date.ToTimeStampTag("F"); // Tuesday, 20 April 2021 16:20
    ///         date.ToTimestampTag("R"); // 2 months ago
    ///     </code>
    /// </example>
    public static string ToTimestampTag(this DateTimeOffset date, string styleString = "f")
    {
        var style = styleString switch
        {
            "t" => TimestampTagStyles.ShortTime,
            "T" => TimestampTagStyles.LongTime,
            "d" => TimestampTagStyles.ShortDate,
            "D" => TimestampTagStyles.LongDate,
            "f" => TimestampTagStyles.ShortDateTime,
            "F" => TimestampTagStyles.LongDateTime,
            "R" => TimestampTagStyles.Relative,
            _   => throw new ArgumentOutOfRangeException(nameof(styleString))
        };

        return date.ToTimestampTag(style);
    }
}
