using ErrH.Tools.MvvmPattern;
using ErrH.Tools.Randomizers;
using PropertyChanged;

namespace ErrH.WpfTools.ViewModels.Fakes
{
    [ImplementPropertyChanged]
    public class FakeViewModel : ListItemVmBase
    {
        public string  FirstName  { get; set; }
        public string  LastName   { get; set; }

        public string FullName 
            => $"{FirstName} {LastName}";


        public FakeViewModel()
        {
            var fke   = new FakeFactory();
            FirstName = fke.Word;
            LastName  = fke.Word;
        }


        public override string DisplayName => FullName;
    }
}
