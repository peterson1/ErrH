using System;
using System.Threading.Tasks;
using PropertyChanged;

namespace ErrH.Core.PCL45.Inputs
{
    // https://msdn.microsoft.com/en-us/magazine/dn630647.aspx
    [ImplementPropertyChanged]
    public class ClearyAsyncCmdPCL<TResult> : ClearyAsyncCmdBase
    {
        private readonly Func<Task<TResult>>  _command;
        private readonly Func<object, Task<TResult>>  _cmdWithParam;


        public NotifyTaskCompletion<TResult> Execution { get; private set; }


        public override string ErrorMessage => Execution?.ErrorMessage;
        public override string ErrorDetails => Execution?.ErrorDetails;


        public ClearyAsyncCmdPCL(Func<Task<TResult>> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
        {
            _command       = command;
            IdleLabel      = idleLabel;
            ExecutingLabel = executingLabel;
            FinishedLabel  = finishedLabel;
            CurrentLabel   = IdleLabel;
        }

        public ClearyAsyncCmdPCL(Func<object, Task<TResult>> cmdWithParam, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
        {
            _cmdWithParam  = cmdWithParam;
            IdleLabel      = idleLabel;
            ExecutingLabel = executingLabel;
            FinishedLabel  = finishedLabel;
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

        public override string ToString() => CurrentLabel;
    }

    public static class ClearyAsyncCmdPCL
    {
        public static ClearyAsyncCmdPCL<object> Create(Func<Task> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdPCL<object>(async ()
                => { await command(); return null; }, idleLabel, executingLabel, finishedLabel);

        public static ClearyAsyncCmdPCL<TResult> Create<TResult>(Func<Task<TResult>> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdPCL<TResult>(command, idleLabel, executingLabel, finishedLabel);

        public static ClearyAsyncCmdPCL<TResult> Create<TResult>(Func<object, Task<TResult>> cmdWithParamm, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdPCL<TResult>(cmdWithParamm, idleLabel, executingLabel, finishedLabel);
    }
}
