using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Loggers;
using ErrH.Tools.SqlHelpers;

namespace ErrH.Tools.CollectionShims
{
    public interface IRepoUpdater<T> 
        : ILogSource, INotifyPropertyChanged, IDisposable
    {

        ISqlDbReader  DbReader      { get; set; }
        //string        ResourceURL   { get; }

        string        JobTitle      { get; }
        string        JobMessage    { get; }
        int           ProgressValue { get; }
        int           ProgressTotal { get; }


        string        GetSqlQuery  (params object[] args);
        ResultRow     ProcessResultBeforeHashing (ResultRow row);

        Task<bool>  Update  (
            CancellationToken token = new CancellationToken(),
            params object[] args
        );
    }
}
