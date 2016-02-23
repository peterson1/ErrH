using System.Text;

namespace ErrH.RestClient.PCL45.Extensions
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


        /// <summary>
        /// From: HashLib 2.1 (Dec 29, 2013) Stable
        /// http://hashlib.codeplex.com/
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1(this string text)
        {
            var algo = new HashLib.Crypto.SHA1();
            var res = algo.ComputeString(text, Encoding.UTF8);
            return res.ToString().ToLower();
        }

    }
}
