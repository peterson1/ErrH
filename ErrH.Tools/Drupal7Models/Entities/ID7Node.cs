using System.ComponentModel;

namespace ErrH.Tools.Drupal7Models.Entities
{
    public interface ID7Node : INotifyPropertyChanged
    {
        int nid        { get; set; }
        int uid        { get; set; }

        string title   { get; set; }
        string type    { get; set; }
    }
}
