using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PropertyChanged;

namespace ErrH.WpfTools.TaskShims
{
    [ImplementPropertyChanged]
    public class TalkyTask<TResult> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public Task<TResult>  Task           { get; }
        public Task           TaskCompletion { get; }


        public TaskStatus          Status         => Task.Status;
        public AggregateException  Exception      => Task.Exception;
        public Exception           InnerException => Exception?.InnerException;
        public string              ErrorMessage   => InnerException?.Message;


        public bool  IsCompleted    => Task.IsCompleted;
        public bool  IsNotCompleted => !Task.IsCompleted;
        public bool  IsCanceled     => Task.IsCanceled;
        public bool  IsFaulted      => Task.IsFaulted;


        public bool IsSuccessfullyCompleted
            => Task.Status == TaskStatus.RanToCompletion;

        public TResult Result 
            =>  (Task.Status == TaskStatus.RanToCompletion) ?
                    Task.Result : default(TResult);
        



        public TalkyTask(Task<TResult> task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }



        private async Task WatchTaskAsync(Task task)
        {
            try   { await task; }
            catch { }

            RaisePropertyChanged(nameof(Status));
            RaisePropertyChanged(nameof(IsCompleted));
            RaisePropertyChanged(nameof(IsNotCompleted));


            if (task.IsCanceled)
            {
                RaisePropertyChanged(nameof(IsCanceled));
            }
            else if (task.IsFaulted)
            {
                RaisePropertyChanged(nameof(IsFaulted));
                RaisePropertyChanged(nameof(Exception));
                RaisePropertyChanged(nameof(InnerException));
                RaisePropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                RaisePropertyChanged(nameof(IsSuccessfullyCompleted));
                RaisePropertyChanged(nameof(Result));
            }
        }



        private void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
