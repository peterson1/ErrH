using System.Collections.Generic;

namespace ErrH.Tools.CollectionShims
{
    public interface ILastResult<T>
    {
        IEnumerable<T> LastResult { get; }
    }
}
