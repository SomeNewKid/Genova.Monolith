namespace Genova.ContentService.Fields;

public sealed class UrlValue
{
    /// <summary>
    /// The final, validated URL (may be absolute, relative, root-relative, or protocol-relative).
    /// </summary>
    public string Value { get; }

    private UrlValue(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new UrlValue from the given string, trimming whitespace and validating structure.
    /// Acceptable forms:
    ///  - Relative (e.g. "path", "../somewhere")
    ///  - Root-relative (e.g. "/path")
    ///  - Protocol-relative (e.g. "//example.com/path")
    ///  - Absolute (e.g. "https://example.com/path")
    /// Does NOT validate the domain or the protocol scheme. Disallows whitespace, double '://',
    /// multiple '?' or '#', or other obviously invalid structures.
    /// </summary>
    /// <exception cref="ArgumentException">If the given URL is empty or structurally invalid.</exception>
    public static UrlValue Create(string? raw)
    {
        // 1) Trim and reject empty
        var trimmed = (raw ?? string.Empty).Trim();
        if (trimmed.Length == 0)
            throw new ArgumentException("URL cannot be empty.");

        // 2) No internal whitespace (newlines, tabs, spaces). 
        //    We already Trim() at ends, but disallow interior whitespace.
        if (HasAnyWhitespace(trimmed))
            throw new ArgumentException("URL cannot contain whitespace.");

        // 3) Disallow leading "://"
        if (trimmed.StartsWith("://", StringComparison.Ordinal))
            throw new ArgumentException("URL must not start with '://'. A protocol or '//' is required, or use a relative path.");

        // 4) Check for multiple '://'
        if (CountOccurrences(trimmed, "://") > 1)
            throw new ArgumentException("URL cannot contain more than one '://'. Example of invalid: 'http://some://thing'.");

        // 5) Check for multiple '?' or '#'
        if (CountOccurrences(trimmed, "?") > 1)
            throw new ArgumentException("URL cannot contain more than one '?'.");
        if (CountOccurrences(trimmed, "#") > 1)
            throw new ArgumentException("URL cannot contain more than one '#'.");

        // 6) Check for invalid characters beyond typical URL-safe sets.
        //    For simplicity, let's define a minimal set of allowed ASCII ranges plus common punctuation.
        //    This is not an exhaustive RFC 3986 check, but enough to catch obviously invalid chars (like <, >, etc.).
        //    Adjust as needed for your real URL rules.
        if (!AllCharactersAllowed(trimmed))
            throw new ArgumentException("URL contains invalid characters.");

        return new UrlValue(trimmed);
    }

    /// <summary>
    /// Minimal method to detect typical "invalid" whitespace inside a string.
    /// </summary>
    private static bool HasAnyWhitespace(string input)
    {
        foreach (char ch in input)
        {
            if (char.IsWhiteSpace(ch))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Counts how many times a substring occurs in the given string.
    /// </summary>
    private static int CountOccurrences(string text, string substring)
    {
        if (substring.Length == 0) return 0;

        int count = 0;
        int pos = text.IndexOf(substring, StringComparison.Ordinal);
        while (pos >= 0)
        {
            count++;
            pos = text.IndexOf(substring, pos + substring.Length, StringComparison.Ordinal);
        }
        return count;
    }

    /// <summary>
    /// Ensures every character is within a typical set of URL-safe characters.
    /// This is simplified; real-world rules can be more nuanced.
    /// </summary>
    private static bool AllCharactersAllowed(string url)
    {
        // We'll allow alphanumeric, plus a range of punctuation typically seen in URLs.
        // This includes unreserved / reserved characters from RFC 3986:
        //   - ALPHA, DIGIT
        //   - "-", ".", "_", "~"
        //   - ":", "/", "?", "#", "[", "]", "@"
        //   - "!", "$", "&", "'", "(", ")", "*", "+", ",", ";", "="
        // ... plus "%" for escaped sequences.
        // We'll disallow quotes, angle brackets, whitespace, backslash, etc.

        foreach (char ch in url)
        {
            // Basic categories
            if (char.IsLetterOrDigit(ch))
                continue;

            switch (ch)
            {
                // Allowed punctuation from RFC 3986 (some of these might appear rarely,
                // but are still valid in certain contexts).
                case '-':
                case '.':
                case '_':
                case '~':
                case ':':
                case '/':
                case '?':
                case '#':
                case '[':
                case ']':
                case '@':
                case '!':
                case '$':
                case '&':
                case '\'':
                case '(':
                case ')':
                case '*':
                case '+':
                case ',':
                case ';':
                case '=':
                case '%':
                    // Possibly expand if you allow "{" or "}" or other rare URL chars.
                    break;
                default:
                    return false;
            }
        }
        return true;
    }
}
