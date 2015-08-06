using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ErrH.Tools.CollectionShims
{
    /// <summary>
    /// Inherits from ReadOnlyCollection, and adds member: "Parent".
    /// </summary>
    /// <typeparam name="TChild"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class ChildList<TChild, TParent> : ReadOnlyCollection<TChild>
    {
        /// <summary>
        /// Parent of all items in the collection.
        /// </summary>
        public TParent Parent { get; set; }


        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="list"></param>
        public ChildList(IList<TChild> list) : base(list) { }
    }
}
