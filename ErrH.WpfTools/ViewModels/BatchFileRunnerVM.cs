using System.Threading;
using System.Threading.Tasks;
using ErrH.WinTools.ProcessTools;
using ErrH.WpfTools.Commands;

namespace ErrH.WpfTools.ViewModels
{
    public class BatchFileRunnerVM : WorkspaceVmBase
    {
        private BatchFileShim _batShim;

        public IAsyncCommand  RunAgainCmd { get; }
        public LogScrollerVM  LogScroller { get; }
        


        public BatchFileRunnerVM(LogScrollerVM logScroller)
        {
            LogScroller = logScroller.ListenTo(this);
            RunAgainCmd = AsyncCommand.Create(tkn => RunAgain(tkn));
            _batShim    = ForwardLogs(new BatchFileShim());
        }


        private Task RunAgain(CancellationToken tkn)
            => new Task(() => _batShim.RunAgain());


        public void Run(string batchFilePath)
        {
            _batShim.Run(batchFilePath);
        }
    }
}
