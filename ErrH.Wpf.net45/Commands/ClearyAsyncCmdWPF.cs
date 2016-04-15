using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ErrH.Core.PCL45.Inputs;
using PropertyChanged;

namespace ErrH.Wpf.net45.Commands
{
    public static class ClearyAsyncCmdWPF
    {
        public static ClearyAsyncCmdWPF<object> Create(Func<Task> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdWPF<object>(async () 
                => { await command(); return null; }, idleLabel, executingLabel, finishedLabel);

        public static ClearyAsyncCmdWPF<TResult> Create<TResult>(Func<Task<TResult>> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdWPF<TResult>(command, idleLabel, executingLabel, finishedLabel);

        public static ClearyAsyncCmdWPF<TResult> Create<TResult>(Func<object, Task<TResult>> cmdWithParamm, string idleLabel = null, string executingLabel = null, string finishedLabel = null)
            => new ClearyAsyncCmdWPF<TResult>(cmdWithParamm, idleLabel, executingLabel, finishedLabel);
    }



    [ImplementPropertyChanged]
    public class ClearyAsyncCmdWPF<TResult> : ClearyAsyncCmdPCL<TResult>
    {
        public ClearyAsyncCmdWPF(Func<Task<TResult>> command, string idleLabel = null, string executingLabel = null, string finishedLabel = null) : base(command, idleLabel, executingLabel, finishedLabel)
        {
        }

        public ClearyAsyncCmdWPF(Func<object, Task<TResult>> cmdWithParam, string idleLabel = null, string executingLabel = null, string finishedLabel = null) : base(cmdWithParam, idleLabel, executingLabel, finishedLabel)
        {
        }

        protected override void AddHandlerToCanExecuteChanged(EventHandler handlr)
            => CommandManager.RequerySuggested += handlr;

        protected override void RemoveHandlerFromCanExecuteChanged(EventHandler handlr)
            => CommandManager.RequerySuggested -= handlr;

        protected override void RaiseCanExecuteChanged()
            => CommandManager.InvalidateRequerySuggested();
    }
}
