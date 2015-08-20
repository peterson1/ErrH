using System;

namespace ErrH.Tools.ScalarEventArgs
{
    public class EArg<T> : EventArgs
    {
        public T Value { get; set; }

        public static EArg<T> NewArg<Tin>(Tin value) where Tin : T
            => new ScalarEventArgs.EArg<T> { Value = value };
    }
}
