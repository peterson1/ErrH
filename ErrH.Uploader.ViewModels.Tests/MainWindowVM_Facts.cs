using System.Linq;
using ErrH.Uploader.Core.Models;
using ErrH.Uploader.ViewModels.NavigationVMs;
using ErrH.Uploader.ViewModels.Tests.DataAttributes;
using ErrH.XunitTools;
using ErrH.XunitTools.Fakes;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Uploader.ViewModels.Tests
{
    public class MainWindowVM_Facts
    {
        public MainWindowVM_Facts(ITestOutputHelper output)
        {
            MustExtensions.OutputHelper = output;
        }


        [Fact(DisplayName = "State before Refresh()", Skip ="not maintained")]
        public void StateBeforeRefresh()
        {
            var repo = Fake.Repo<AppFolder>(3);
            var sut = new FoldersTabVM(repo, null, null);

            sut.MainList.Count.MustBe(0, "Folder count");
            //todo: re-test all .SelectedItem
            //sut.MainList.SelectedItems.Count().MustBe(0, "Selected items");
            //sut.MainList.SelectedIndex.MustBe(-1, "Selected index");
        }


        [Theory(DisplayName="Refresh() loads all folders", Skip = "not maintained")]
        [InlineData(5)]
        [InlineData(1)]
        [InlineData(0)]
        public void RefreshLoadsAllFolders(int foldersCount)
        {
            var repo = Fake.Repo<AppFolder>(foldersCount);
            var sut = new FoldersTabVM(repo, null, null);

            sut.Refresh();
            sut.MainList.Count.MustBe(foldersCount, "Folders count");
        }


        [Theory(DisplayName = "Initital Refresh() selects first item", Skip = "not maintained"), FoldersTabData]
        public void InitialRefreshSelectsFirstItem(MainWindowVM sut)
        {
            sut.Refresh();
            //sut.MainList[0].IsSelected.MustBe(true, "IsSelected");
            //sut.MainList.SelectedItems.Count().MustBe(1, "Selected items");
        }


        [Theory(DisplayName = "Succeeding Refresh() restores selection", Skip = "not maintained")]
        [FoldersTabData(folders: 10)]
        public void SucceedingRefreshRestoresSelection(FoldersTabVM sut)
        {
            sut.Refresh();
            var f = sut.MainList;

            //f.SelectedIndex = 2;
            sut.Refresh();

            f[2].IsSelected.MustBe(true, "IsSelected");
            //f.SelectedIndex.MustBe(2, "SelectedIndex");
            //f.SelectedItems.Count().MustBe(1, "Selected items");

            //f.SelectedIndex = 5;
            sut.Refresh();

            f[5].IsSelected.MustBe(true, "IsSelected");
            //f.SelectedIndex.MustBe(5, "SelectedIndex");
            //f.SelectedItems.Count().MustBe(1, "Selected items");
        }
    }
}
