using System.Text;
using JetBrains.Annotations;

namespace CookieUtils.Base
{
    [PublicAPI]
    public static class MiscUtils
    {
        [Pure]
        public static string ToDisplayString(string internalName)
        {
            internalName = internalName.Trim();

            if (internalName.StartsWith("_"))
                internalName = internalName[1..];
            if (internalName.StartsWith("m_") || internalName.StartsWith("k_"))
                internalName = internalName[2..];

            StringBuilder sb = new();
            sb.Append(char.ToUpper(internalName[0]));
            for (int i = 1; i < internalName.Length; i++)
            {
                char c = internalName[i];
                if (IsSeparatorChar(c))
                {
                    sb.Append(' ');
                }

                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        [Pure]
        public static bool IsSeparatorChar(char c)
        {
            if (c == '_')
                return true;
            return char.IsUpper(c);
        }
    }
}
