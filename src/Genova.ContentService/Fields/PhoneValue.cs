namespace Genova.ContentService.Fields;

public sealed class PhoneValue
{
    /// <summary>
    /// The final, normalized phone string.
    /// It may contain only digits, or a leading '+' followed by digits.
    /// </summary>
    public string Value { get; }

    private PhoneValue(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a PhoneValue from the given string by:
    ///  1) Removing all whitespace, parentheses '()', and hyphens '-'
    ///  2) Checking if the first character is '+'. If so, the rest must be digits.
    ///  3) Otherwise, all characters must be digits only.
    ///  4) Counting digits, which must be between 6 and 20 inclusive.
    /// </summary>
    /// <param name="raw">The raw phone number input (may include spaces, parentheses, hyphens, etc.).</param>
    /// <returns>A valid PhoneValue with the normalized phone string.</returns>
    /// <exception cref="ArgumentException">If the phone is invalid or has fewer than 6 or more than 20 digits.</exception>
    public static PhoneValue Create(string? raw)
    {
        // 1) Normalize by removing whitespace, parentheses, and hyphens.
        var normalized = RemoveUnwantedCharacters(raw ?? string.Empty);

        if (normalized.Length == 0)
        {
            throw new ArgumentException("Phone number is empty after removing whitespace/parentheses/hyphens.");
        }

        // 2) Check if the first character is '+'
        bool startsWithPlus = normalized[0] == '+';

        // We'll count the digits as we verify characters.
        int digitCount = 0;

        // Validate each character:
        // - If startsWithPlus is true, the first char is '+', so subsequent must be digits.
        // - If startsWithPlus is false, all chars must be digits.
        for (int i = 0; i < normalized.Length; i++)
        {
            char c = normalized[i];

            if (i == 0 && startsWithPlus)
            {
                // The first character is '+', that's allowed, skip digitCount increment.
                continue;
            }

            if (!char.IsDigit(c))
            {
                throw new ArgumentException(
                    $"Phone number contains invalid character '{c}' at index {i}. " +
                    "Only digits are allowed after an optional leading '+'.");
            }

            // This character is a digit
            digitCount++;
        }

        // 3) Now check digitCount range (6..20)
        //    If the string starts with '+', digitCount excludes that plus sign, so we rely on actual digits count.
        if (digitCount < 6 || digitCount > 20)
        {
            throw new ArgumentException(
                $"Phone number must have 6..20 digits. Found {digitCount} digits in '{normalized}'.");
        }

        return new PhoneValue(normalized);
    }

    /// <summary>
    /// Strips out whitespace, parentheses, and hyphens from the input string.
    /// Then returns the result.
    /// </summary>
    private static string RemoveUnwantedCharacters(string input)
    {
        var chars = input.Trim().ToCharArray();
        var sb = new System.Text.StringBuilder(chars.Length);

        foreach (char c in chars)
        {
            if (char.IsWhiteSpace(c))
                continue;
            if (c == '(' || c == ')' || c == '-')
                continue;

            sb.Append(c);
        }
        return sb.ToString();
    }
}
