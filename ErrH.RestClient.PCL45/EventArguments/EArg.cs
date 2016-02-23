using System;

namespace ErrH.RestClient.PCL45.EventArguments
{
    public class EArg<T> : EventArgs
    {
        public T Value { get; set; }

        public static EArg<T> NewArg<Tin>(Tin value) where Tin : T
            => new EArg<T> { Value = value };
    }
}
