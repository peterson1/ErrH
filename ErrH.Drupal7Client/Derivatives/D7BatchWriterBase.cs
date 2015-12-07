using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using PropertyChanged;

namespace ErrH.Drupal7Client.Derivatives
{
    [ImplementPropertyChanged]
    public abstract class D7BatchWriterBase<T> : D7WriterBase<T>
        where T : class, ID7Node, new()
    {

        protected int _pageSize = 10;


        //public override async Task<bool> SaveChanges(CancellationToken tkn = default(CancellationToken))
        //{
        //    InitializeProgressState();
        //    OneChangeCommitted += (s, e) => ProgressValue++;

        //    if (!await AddNewItems(tkn)) return false;
        //}



        //private async Task<bool> AddNewItems(CancellationToken tkn)
        //{
        //    D7NodeBase dto;
        //    var nodes = new List<T>();

        //    foreach (var item in _newUnsavedItems)
        //    {
        //        try { dto = D7FieldMapper.Map(item); }
        //        catch (Exception ex) { return LogError("D7FieldMapper.Map", ex); }

        //        if (dto == null) return Error_n("Failed to map to D7 fields.", "");

        //    }

        //}
    }
}
