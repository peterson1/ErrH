using System;
using System.Threading.Tasks;
using PropertyChanged;

namespace ErrH.Core.PCL45.Inputs
{
    [ImplementPropertyChanged]
    public class ClearyAsyncCmdPCL<TResult> : ClearyAsyncCmdBase
    {
        private readonly Func<Task<TResult>>  _command;
        private readonly Func<object, Task<TResult>>  _cmdWithParam;

        public NotifyTaskCompletion<TResult> Execution { get; private set; }

        public ClearyAsyncCmdPCL(Func<Task<TResult>> command, string idleLabel = null, string executingLabel = null)
        {
            _command       = command;
            IdleLabel      = idleLabel;
            ExecutingLabel = executingLabel;
            CurrentLabel   = IdleLabel;
        }

        public ClearyAsyncCmdPCL(Func<object, Task<TResult>> cmdWithParam, string idleLabel = null, string executingLabel = null)
        {
            _cmdWithParam  = cmdWithParam;
            IdleLabel      = idleLabel;
            ExecutingLabel = executingLabel;
            CurrentLabel   = IdleLabel;
        }


        public override bool CanExecute(object parameter)
        {
            if (!IsEnabled) return false;
            return Execution == null || Execution.IsCompleted;
        }


        public override async Task ExecuteAsync(object parameter)
        {
            if (_cmdWithParam != null)
                Execution = new NotifyTaskCompletion<TResult>(_cmdWithParam(parameter));
            else
                Execution = new NotifyTaskCompletion<TResult>(_command());

            RaiseCanExecuteChanged();
            await Execution.Completion;
            RaiseCanExecuteChanged();
        }
    }
}
