using System.Linq;

namespace ProjectManager.Domain.Utils.Expressions.Internal
{
    internal static class Utils
    {
        private static readonly char[] WhiteSpaces = { ' ', '\t', '\n', '\r' };

        public static bool IsWhiteSpace(this char @char)
        {
            return WhiteSpaces.Contains(@char);
        }
    }
}
