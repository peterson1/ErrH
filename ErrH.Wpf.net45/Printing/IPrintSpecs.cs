using System.Windows;

namespace ErrH.Wpf.net45.Printing
{
    public interface IPrintSpecs
    {
        string   HeaderLeftText    { get; }
        string   HeaderCenterText  { get; }
        string   HeaderRightText   { get; }
        string   FooterCenterText  { get; }
        string   PrintJobTitle     { get; }

        ResourceDictionary Resources { get; }
    }
}
