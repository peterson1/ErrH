using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;

namespace ErrH.WpfTools.ViewModels.Fakes
{
    public class FakeMainWindowVM : MainWindowVMBase
    {
        protected override Task<List<WorkspaceViewModelBase>> CreateVMsList()
        {
            var list = new List<WorkspaceViewModelBase>();
            var count = new FakeFactory().Int(5, 10);
            for (int i = 1; i <= count; i++)
                list.Add(new FakeListVM());

            return list.ToTask();
        }
    }
}
