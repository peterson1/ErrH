using System;

namespace HashLib
{
    internal interface ICrypto : IHash, IBlockHash
    {
    }

    internal interface ICryptoBuildIn : ICrypto
    {
    }

    internal interface ICryptoNotBuildIn : ICrypto
    {
    }

    internal interface IHMAC : IWithKey, ICrypto
    {
    }

    internal interface IHMACBuildIn : IHMAC, ICryptoBuildIn
    {
    }

    internal interface IHMACNotBuildIn : IHMAC, ICryptoNotBuildIn
    {
    }

    internal interface IHasHMACBuildIn : ICrypto
    {
        //System.Security.Cryptography.HMAC GetBuildHMAC();
    }

    internal interface IHash32 : IHash
    {
    }

    internal interface IHash128 : IHash
    {
    }

    internal interface IHash64 : IHash
    {
    }

    internal interface IWithKey : IHash
    {
        byte[] Key
        {
            get;
            set;
        }

        int? KeyLength
        {
            get;
        }
    }

    internal interface IHashWithKey : IHash, IWithKey
    {
    }

    internal interface IFastHash32
    {
        int ComputeByteFast(byte a_data);
        int ComputeCharFast(char a_data);
        int ComputeShortFast(short a_data);
        int ComputeUShortFast(ushort a_data);
        int ComputeIntFast(int a_data);
        int ComputeUIntFast(uint a_data);
        int ComputeLongFast(long a_data);
        int ComputeULongFast(ulong a_data);
        int ComputeFloatFast(float a_data);
        int ComputeDoubleFast(double a_data);
        int ComputeStringFast(string a_data);
        int ComputeBytesFast(byte[] a_data);
        int ComputeCharsFast(char[] a_data);
        int ComputeShortsFast(short[] a_data);
        int ComputeUShortsFast(ushort[] a_data);
        int ComputeIntsFast(int[] a_data);
        int ComputeUIntsFast(uint[] a_data);
        int ComputeLongsFast(long[] a_data);
        int ComputeULongsFast(ulong[] a_data);
        int ComputeDoublesFast(double[] a_data);
        int ComputeFloatsFast(float[] a_data);
    }

    internal interface IBlockHash
    {
    }

    internal interface INonBlockHash
    {
    }

    internal interface IChecksum
    {
    }
}
