using System.IO;

#nullable enable
namespace Kuva.Accounts.Business.Logging
{
    internal static class Extensions
    {
        internal static void Render(this TextWriter output, string propertyName, object value, string? delimiter = null)
        {
            output.Write(propertyName);
            output.Write(": ");
            output.Write(value);
            if (!string.IsNullOrEmpty(delimiter))
                output.Write(delimiter);
        }
    }
}
