using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ErrH.Tools.Randomizers
{
    public static class ThreadSafe
    {

        // from http://stackoverflow.com/a/1262619/3973863
        [ThreadStatic]
        private static Random _localRandom;
        public static Random LocalRandom
        {
            get
            {
                return _localRandom ??
                    (_localRandom = new Random(unchecked(Environment.TickCount * 31
                                    + Thread.CurrentThread.ManagedThreadId)));

            }
        }


        [ThreadStatic]
        private static FakeFactory _fake;
        public static FakeFactory Fake
        {
            get
            {
                return _fake ?? (_fake = new FakeFactory());
            }
        }
    }
}
