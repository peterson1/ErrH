using ErrH.Tools.CollectionShims;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.XunitTools.Fakes;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;

namespace ErrH.Uploader.ViewModels.Tests.DataAttributes
{
    internal class FoldersTabDataAttribute : AutoDataAttribute
    {
        public FoldersTabDataAttribute(int folders = 3)
        {
            var repo = Fake.Repo<AppFolder>(folders);
            Fixture.Register<IRepository<AppFolder>>(() => repo);
            Fixture.Register<FoldersTabVM>(() => new FoldersTabVM(repo, null, null));
        }
    }
}
