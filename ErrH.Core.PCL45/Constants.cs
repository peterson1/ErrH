using System;

namespace ErrH.Core.PCL45
{
    public struct L
    {
        public static string f => Environment.NewLine;
        public static string F => L.f + L.f;
    }


    public enum CalendarMonth
    {
        January   =  1,
        February  =  2,
        March     =  3,
        April     =  4,
        May       =  5,
        June      =  6,
        July      =  7,
        August    =  8,
        September =  9,
        October   = 10,
        November  = 11,
        December  = 12
    }
}
