using System;

namespace ErrH.Core.PCL45.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToPesoWords(this decimal value)
        {
            var p1Val = (int)Math.Truncate(value);
            var p2Val = (int)Math.Truncate((value - p1Val) * 100);
            var p1Sfx = (p1Val == 1) ? "peso"    : "pesos";
            var p2Sfx = (p2Val == 1) ? "centavo" : "centavos";
            var words = p1Val.ToWords() + " " + p1Sfx;

            if (p2Val != 0) words += " and " 
                + p2Val.ToWords() + " " + p2Sfx;

            return words;
        }
    }
}
