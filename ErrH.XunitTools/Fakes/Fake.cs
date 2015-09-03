using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrH.XunitTools.Fakes
{
    public class Fake
    {
        public static FakeListRepo<T> Repo<T>(int itemCount)
            => new FakeListRepo<T>(itemCount);
    }
}
