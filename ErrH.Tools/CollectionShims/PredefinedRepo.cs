using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.CollectionShims
{
    public class PredefinedRepo<T> : ListRepoBase<T>
    {
        private List<T> _predefList;

        public PredefinedRepo(IEnumerable<T> list) 
            : base(list.ToList())
        {
            _predefList = list.ToList();
        }

        protected override Func<T, object> 
            GetKey => x => null;

        protected override List<T> 
            LoadList(object[] args) => _predefList;


        public override Task<bool> LoadAsync(CancellationToken tkn, params object[] args)
        {
            _list = _predefList;
            return true.ToTask();
        }
    }
}
