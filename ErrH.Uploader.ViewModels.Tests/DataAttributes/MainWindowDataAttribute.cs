using ErrH.Tools.CollectionShims;
using ErrH.Tools.FileSynchronization;
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
            var repo = Fake.Repo<SyncableFolderInfo>(folders);
            Fixture.Register<IRepository<SyncableFolderInfo>>(() => repo);
            Fixture.Register<FoldersTabVM>(() => new FoldersTabVM(repo, null));
        }
    }
}
