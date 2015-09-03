using ErrH.Tools.Randomizers;

namespace ErrH.WpfTools.ViewModels.Fakes
{
    public class FakeMainWindowVM : MainWindowVMBase
    {
        public FakeMainWindowVM()
        {
            var count = new FakeFactory().Int(5, 10);
            for (int i = 1; i <= count; i++)
                Workspaces.Add(new FakeListVM());

            //CompletelyLoaded += (s, e) => { Refresh(); };
        }
    }
}
