using System;

namespace ErrH.Core.PCL45.Extensions
{
    public static class StringExtensions
    {

        public static string StripLineBreaks(this string text, string replacementText = " ")
            => text.IsBlank() ? text 
            : text.Replace("\r", replacementText)
                  .Replace("\n", replacementText);



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



        public static bool IsNumeric(this string text)
        {
            if (text.IsBlank()) return false;
            text = text.Trim();
            text = text.TrimStart('-');
            text = text.Trim();

            var dots = text.CountOccurence('.');
            if (dots > 1) return false;
            if (dots == 1) text = text.Replace(".", "");

            foreach (char c in text.ToCharArray())
                if (!char.IsDigit(c)) return false;

            return true;
        }



        /// <summary>
        /// Counts occurences of a character in a string.
        /// </summary>
        /// <param name="fullText"></param>
        /// <param name="findThis">character to look for</param>
        /// <returns></returns>
        public static int CountOccurence(this string fullText, char findThis)
        {
            int count = 0;
            var chars = fullText.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
                if (chars[i] == findThis) count++;

            return count;
        }


        public static int ToInt(this string text)
        {
            int val; var ok = int.TryParse(text, out val);
            if (ok) return val;
            throw new FormatException($"Non-convertible to Int32: “{text}”.");
        }
    }
}
