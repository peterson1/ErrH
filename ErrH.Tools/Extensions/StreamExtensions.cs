using System.IO;

namespace ErrH.Tools.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Computes SHA1 hash, closes the stream, then returns lower-cased checksum.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Lower-cased hash string</returns>
        public static string SHA1(this Stream stream)
        {
            var algo = new HashLib.Crypto.SHA1();
            using (var openStream = stream)
            {
                var res = algo.ComputeStream(openStream);
                return res.ToString().ToLower();
            }

        }
    }
}
