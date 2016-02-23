using System;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using PropertyChanged;

namespace ErrH.Wpf.net45.Commands
{
    [ImplementPropertyChanged]
    public abstract class StatusCommandBase : ResultingCommand<string>
    {
        //private static Logger _logr = LogManager.GetCurrentClassLogger();

        public bool    IsBusy       { get; private set; }
        public string  Status       { get; set; }
        public string  StatusBefore { get; set; }
        public string  StatusAfter  { get; set; }


        public StatusCommandBase(Func<object, Task<string>> action, 
            Predicate<object> canExecute = null)
            : base(action)
        {
            if (canExecute == null)
                _canExecute = _ => !IsBusy;
            else
                _canExecute = _ => !IsBusy && canExecute.Invoke(_);
        }


        protected override void BeforeExecute(object parameter)
        {
            IsBusy = true;
            if (!StatusBefore.IsBlank())
            {
                LogInfo(StatusBefore);
                Status = StatusBefore;
            }
            base.BeforeExecute(parameter);
        }


        protected override void AfterExecute(object parameter)
        {
            if (!StatusAfter.IsBlank())
            {
                LogInfo(StatusAfter);
                //Status = $"{StatusAfter}  {Result}";
                Status = string.Format(StatusAfter, Result);
            }
            base.AfterExecute(parameter);
            IsBusy = false;
        }


        protected override void OnError(Exception ex, string caption = null)
        {
            LogError(caption + L.f + ex.Details());
            Status = ex.Message;
            base.OnError(ex, caption);
        }


        protected abstract void LogInfo(string message);
        protected abstract void LogError(string message);
    }
}
