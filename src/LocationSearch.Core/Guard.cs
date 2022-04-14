using System.Diagnostics.CodeAnalysis;

namespace LocationSearch.Core;

public static class Guard
{
    public static void NotNull([NotNull] object? obj, string name)
    {
        if (ReferenceEquals(obj, null))
        {
            throw new ArgumentNullException($"Cannot be null: ${name}");
        }
    }

    public static void NotNegative(int num, string name)
    {
        if (num < 0)
        {
            throw new ArgumentException($"Cannot be null: ${name}");
        }
    }
    
    public static void NotNegative(double num, string name)
    {
        if (num < 0)
        {
            throw new ArgumentException($"Cannot be null: ${name}");
        }
    }
}