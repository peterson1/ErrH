using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Autofac;
using ErrH.WpfTools.ViewModels;
using ErrH.XunitTools;
using Xunit.Abstractions;

namespace ErrH.WpfTools.Tests.ViewModels.MainWindowVmBase_Theories
{
    public class MainWindowVmBase_Facts
    {
        public MainWindowVmBase_Facts(ITestOutputHelper output)
        {
            MustExtensions.OutputHelper = output;
        }



        [Fact(DisplayName= "Initial state")]
        public void InitialState()
        {
            var sut = new Mock<MainWindowVmBase>().Object;
            sut.NaviTabs.Count.MustBe(0);
            sut.MainTabs.Count.MustBe(0);
            sut.OtherTabs.Count.MustBe(0);
            sut.UserSession.IsLoggedIn.MustBe(false);
        }
    }
}
