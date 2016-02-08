using System;
using ErrH.D7Poster.WPF.Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.Drupal7Models.FieldAttributes;
using ErrH.Tools.Drupal7Models.Fields;
using ErrH.Tools.Drupal7Models.FieldValues;
using ErrH.Tools.Extensions;

namespace ErrH.D7Poster.WPF.DTOs
{
    [D7NodeDto("log_entry", typeof(LogEntryD7Fields))]
    public class LogEntryDTO : D7NodeBase
    {
        [D7NodeTitle]
        public string Title { get; set; }


        [D7ValueField(nameof(LogEntryD7Fields.field_message))]
        public string Message { get; set; }


        [D7ValueField(nameof(LogEntryD7Fields.field_event_time))]
        public DateTime EventTime { get; set; }
    }


    class LogEntryD7Fields : D7NodeBase
    {
        public FieldUnd<UndValue> field_message    { get; set; }
        public FieldUnd<UndValue> field_event_time { get; set; }
    }
}
