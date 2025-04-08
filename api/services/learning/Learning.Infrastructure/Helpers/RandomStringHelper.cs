using System.Text;

namespace Learning.Infrastructure.Helpers;

public static class RandomStringHelper
{
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GenerateRandomString(byte lenght)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < lenght; ++i)
        {
            sb.Append(Chars[Random.Shared.Next(Chars.Length)]);
        }

        return sb.ToString();
    }
}
