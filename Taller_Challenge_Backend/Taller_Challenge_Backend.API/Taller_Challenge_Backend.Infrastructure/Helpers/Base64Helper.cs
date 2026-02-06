using System.Text;

namespace Taller_Challenge_Backend.Infrastructure.Helpers
{
    public class Base64Helper
    {
        public static string Encode(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var base64Result = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(base64Result);
        }
    }
}
