namespace SafeVault.Utils;

public class ValidationHelpers
{
    public static bool IsValidInput(string input, List<char>? allowedSpecialCharacters = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        var validCharacters = allowedSpecialCharacters ?? new List<char>();

        return input.All(c => char.IsLetterOrDigit(c) || validCharacters.Contains(c));
    }

    public static bool IsValidSqlInjectionInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return true;
        }

        if (
            input.ToLower().Contains("admin' or '1'='1".ToLower()) ||
            input.ToLower().Contains("select") || input.ToLower().Contains("insert") ||
            input.ToLower().Contains("update") || input.ToLower().Contains("delete") ||
            input.ToLower().Contains("drop") || input.ToLower().Contains("truncate")
        )
        {
            return false;
        }

        return true;
    }

    public static bool IsValidXssInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return true;
        }

        if ((input.ToLower().Contains("<script")) || (input.ToLower().Contains("<iframe")))
        {
            return false;
        }

        return true;
    }
}