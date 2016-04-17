namespace ErrH.Core.PCL45.Extensions
{
    public static class StringExtensions
    {


        /// <summary>
        /// Returns true if string is null or whitespace.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsBlank(this string text)
        {
            if (text == null) return true;
            return string.IsNullOrWhiteSpace(text);
        }



        public static string Between(this string fullText,
                    string firstString, string lastString,
                    bool seekLastStringFromEnd = false)
        {
            if (fullText.IsBlank()) return string.Empty;

            int pos1 = fullText.IndexOf(firstString) + firstString.Length;
            if (pos1 == -1) return fullText;

            int pos2 = seekLastStringFromEnd ?
                fullText.LastIndexOf(lastString)
                : fullText.IndexOf(lastString, pos1);
            if (pos2 == -1 || pos2 <= pos1) return fullText;

            return fullText.Substring(pos1, pos2 - pos1);
        }


        public static bool SameAs(this string text1, string text2, bool caseSensitive = false)
        {
            if (text1.IsBlank() && text2.IsBlank()) return true;
            if (text1.IsBlank() && !text2.IsBlank()) return false;
            if (!text1.IsBlank() && text2.IsBlank()) return false;

            if (caseSensitive)
                return text1.Trim() == text2.Trim();
            else
                return text1.Trim().ToLower()
                    == text2.Trim().ToLower();
        }

    }
}
