using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrH.Uploader.Core.Models;
using ErrH.XunitTools;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;

namespace ErrH.Uploader.Core.Tests.Models.RemoteVsLocalFile_Theories
{
    public class Event_Facts
    {
        public Event_Facts(ITestOutputHelper output)
        {
            MustExtensions.OutputHelper = output;
        }


        [Theory(DisplayName="Parent echoes child event"), AutoData]
        public void ParentEchoesChildEvent(RemoteVsLocalFile sut)
        {
            var fired = false;
            var propN = "";


            sut.PropertyChanged += (s, e) =>
            {
                fired = true;
                propN = e.PropertyName;
            };

            sut.Remote = new AppFileInfo();
            sut.Remote.Size = 1234;

            fired.MustBe(true, "flag");
            propN.MustBe(nameof(sut.Remote.Size), "property name");
        }
    }
}
