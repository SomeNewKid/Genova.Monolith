using System.Net.Mail;

namespace Genova.ContentService.Fields;

public sealed class EmailValue
{
    public string Value { get; }

    private EmailValue(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new EmailValue from the given string, trimming whitespace and validating structure.
    /// Requirements:
    /// - Non-empty after trimming.
    /// - Exactly one "@" character.
    /// - Local-part and domain-part each must be non-empty.
    /// - Disallow whitespace and some obviously invalid characters.
    /// This is a simplified approach; real email rules can be more complex.
    /// </summary>
    /// <exception cref="ArgumentException">If input is invalid.</exception>
    public static EmailValue Create(string? raw)
    {
        var trimmed = (raw ?? string.Empty).Trim();
        if (trimmed.Length == 0)
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        // Disallow any interior whitespace
        if (HasAnyWhitespace(trimmed))
        {
            throw new ArgumentException("Email cannot contain spaces or control characters.");
        }

        // Must contain exactly one '@'
        var atCount = CountOccurrences(trimmed, "@");
        if (atCount != 1)
        {
            throw new ArgumentException("Email must contain exactly one '@' character.");
        }

        // Split local-part and domain-part
        var parts = trimmed.Split('@');
        var local = parts[0];
        var domain = parts[1];

        if (local.Length == 0)
        {
            throw new ArgumentException("Local-part (before @) cannot be empty.");
        }
        if (domain.Length == 0)
        {
            throw new ArgumentException("Domain-part (after @) cannot be empty.");
        }

        // Check for invalid characters
        if (!AllCharactersAllowed(local) || !AllCharactersAllowed(domain))
        {
            throw new ArgumentException("Email contains invalid characters.");
        }

        try
        {
            // 2) Parse with MailAddress to ensure basic email format
            var mail = new MailAddress(trimmed);

            // 3) We'll store only the address portion (mail.Address)
            // so any display names like "John Doe <john@example.com>" are discarded.
            return new EmailValue(mail.Address);
        }
        catch (FormatException)
        {
            throw new ArgumentException($"'{raw}' is not a valid email address.", nameof(raw));
        }
    }

    private static bool HasAnyWhitespace(string input)
    {
        foreach (char ch in input)
        {
            if (char.IsWhiteSpace(ch))
            {
                return true;
            }
        }
        return false;
    }

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
    /// Minimal method to detect obviously invalid characters.
    /// This is not a complete RFC 5322 check; it covers a basic set.
    /// If you need more advanced checks (like domain punycode, TLD checks, etc.), 
    /// consider a more robust approach or external library.
    /// </summary>
    private static bool AllCharactersAllowed(string segment)
    {
        // We'll allow letters, digits, and a set of punctuation often permitted in email local parts / domains.
        // Real-world rules can be more relaxed or more strict. 
        // We'll disallow angle brackets, commas, semicolons, quotes, parentheses, etc.
        foreach (char ch in segment)
        {
            if (char.IsLetterOrDigit(ch))
                continue;

            switch (ch)
            {
                case '.':
                case '-':
                case '_':
                case '+':
                    // Domain part might contain additional characters like:
                    //   e.g. domain may have multiple subdomains with . 
                    // local part may have ! # $ % & ' * / ? ^ ` { | } ~ etc. 
                    // This example is simplified.
                    break;
                default:
                    return false;
            }
        }
        return true;
    }
}
