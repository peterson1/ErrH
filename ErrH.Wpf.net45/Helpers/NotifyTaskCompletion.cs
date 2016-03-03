using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ErrH.Wpf.net45.Helpers
{
    // https://msdn.microsoft.com/en-us/magazine/dn630647.aspx
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task       = task;
            Completion = WatchTaskAsync(task);
        }

        public Task<TResult>       Task                    { get; private set; }
        public Task                Completion              { get; private set; }
        public TResult             Result                  => Task.Status == TaskStatus.RanToCompletion ? Task.Result : default(TResult);
        public TaskStatus          Status                  => Task.Status;
        public bool                IsCompleted             => Task.IsCompleted;
        public bool                IsNotCompleted          => !Task.IsCompleted;
        public bool                IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool                IsCanceled              => Task.IsCanceled;
        public bool                IsFaulted               => Task.IsFaulted;
        public AggregateException  Exception               => Task.Exception;
        public Exception           InnerException          => Exception?.InnerException;
        public string              ErrorMessage            => InnerException?.Message;
        public string              ResultOrErrorText       => IsFaulted ? ErrorMessage : Result?.ToString();

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task WatchTaskAsync(Task task)
        {
            try { await task; } catch { }

            var propertyChanged = PropertyChanged;
            if (propertyChanged == null) return;

            propertyChanged(this, Arg(nameof(Status)));
            propertyChanged(this, Arg(nameof(IsCompleted)));
            propertyChanged(this, Arg(nameof(IsNotCompleted)));

            if (task.IsCanceled)
            {
                propertyChanged(this, Arg(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, Arg(nameof(IsFaulted)));
                propertyChanged(this, Arg(nameof(Exception)));
                propertyChanged(this, Arg(nameof(InnerException)));
                propertyChanged(this, Arg(nameof(ErrorMessage)));
                propertyChanged(this, Arg(nameof(ResultOrErrorText)));
            }
            else
            {
                propertyChanged(this, Arg(nameof(IsSuccessfullyCompleted)));
                propertyChanged(this, Arg(nameof(Result)));
                propertyChanged(this, Arg(nameof(ResultOrErrorText)));
            }
        }

        private PropertyChangedEventArgs Arg(string propertyName) 
            => new PropertyChangedEventArgs(propertyName);
    }
}
