using System.Collections.Generic;
using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Tools.Drupal7Models.DTOs
{
    //todo: use generic Und<T> instead
    public class D7File_List
    {
        public List<D7File> und { get; set; }
    }
}
