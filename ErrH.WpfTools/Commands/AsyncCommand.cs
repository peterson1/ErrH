using System;
using System.Threading.Tasks;
using ErrH.WpfTools.TaskShims;

namespace ErrH.WpfTools.Commands
{
    //[ImplementPropertyChanged]
    public class AsyncCommand<TResult> : AsyncCommandBase//, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;


        private readonly Func<Task<TResult>> _command;


        public TalkyTask<TResult> Execution { get; private set; }


        public AsyncCommand(Func<Task<TResult>> command)
        {
            _command = command;
        }


        public override bool CanExecute(object parameter) 
            => true;


        public override Task ExecuteAsync(object parameter)
        {
            Execution = new TalkyTask<TResult>(_command());
            return Execution.TaskCompletion;
        }
    }


    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
            => new AsyncCommand<object>(async () 
                => { await command(); return null; });

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
            => new AsyncCommand<TResult>(command);
    }
}
