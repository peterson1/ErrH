using System.Collections.Generic;
using System.Threading.Tasks;
using ErrH.Tools.Extensions;
using ErrH.Tools.Randomizers;

namespace ErrH.WpfTools.ViewModels.Fakes
{
    public class FakeListVM : ListWorkspaceVMBase<FakeViewModel>
    {
        protected override Task<List<FakeViewModel>> CreateVMsList()
        {
            var list = new List<FakeViewModel>();
            var count = new FakeFactory().Int(5, 20);

            for (int i = 1; i <= count; i++)
                list.Add(new FakeViewModel());

            return list.ToTask();
        }
    }
}
