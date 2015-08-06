using System;
using System.Text;

namespace ErrH.Tools.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Byte array to UTF8 encoding.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToUTF8(this byte[] bytes)
        {
            return UTF8Encoding.UTF8.GetString
                        (bytes, 0, bytes.Length);
        }

        /// <summary>
        /// To lowercase, and removes dashes ("-").
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string AsHash(this byte[] bytes)
        {
            return BitConverter.ToString(bytes)
                               .Replace("-", "")
                               .ToLower();
        }


    }
}
