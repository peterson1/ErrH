namespace ErrH.Tools.Extensions
{
    public static class DecimalExtensions
    {
        public static string AsPeso
            (this decimal value, string prefix = "₱ ")
        {
            return value.ToString(prefix + "#,#0.#0");
        }
    }
}
