using System.Text;

namespace Discord.Addons.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Formats the <paramref name="enumerable"/> into a markdown unordered list.
    /// </summary>
    /// <param name="enumerable">The string enumerable to format into a markdown unordered list.</param>
    /// <param name="useAsterisks">Whether to use <c>*</c> instead of <c>-</c> as the prefix for each list item.</param>
    public static string ToUnorderedList(this IEnumerable<string> enumerable, bool useAsterisks = false) 
        => ToListCore(enumerable, useAsterisks ? '*' : '-');
    
    /// <summary>
    /// Formats the <paramref name="enumerable"/> into a markdown ordered list.
    /// </summary>
    /// <param name="enumerable">The string enumerable to format into a markdown ordered list.</param>
    public static string ToOrderedList(this IEnumerable<string> enumerable) 
        => ToListCore(enumerable, "1.");

    private static string ToListCore(this IEnumerable<string> enumerable, string prefix = "-")
    {
        var sb = new StringBuilder();
        foreach (var item in enumerable)
        {
            if (string.IsNullOrEmpty(item))
            {
                continue;
            }

            var listItem = new StringBuilder().Append(prefix)
                                              .Append(' ')
                                              .Append(item)
                                              .ToString();

            sb.AppendLine(listItem);
        }

        return sb.ToString();
    }

    private static string ToListCore(this IEnumerable<string> enumerable, char prefix = '-')
        => ToListCore(enumerable, prefix.ToString());
}
