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
        


        public BatchFileRunnerVM(LogScrollerVM logScroller, BatchFileShim batchFileShim)
        {
            DisplayName = "Batch File Runner";
            RunAgainCmd = AsyncCommand.Create(tkn => RunAgain(tkn));
            _batShim    = ForwardLogs(batchFileShim);
            LogScroller = logScroller.ListenTo(this);
        }


        private async Task RunAgain(CancellationToken tkn)
        {
            await Task.Delay(1);
            _batShim.RunAgain();
        }


        public void Run(string batchFilePath)
        {
            _batShim.Run(batchFilePath);
        }
    }
}
