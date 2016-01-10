namespace ErrH.Tools.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal PercentOf
            (this decimal numerator, decimal denominator)
            => denominator == 0 ? 0
            : (numerator / denominator) * 100M;


        public static string AsPeso (this decimal value, string prefix = "₱ ")
            => value.ToString(prefix + "#,#0.#0");

        public static string WithComma (this decimal value, string format = "#,#0.#0")
            => value.ToString(format);
    }
}
