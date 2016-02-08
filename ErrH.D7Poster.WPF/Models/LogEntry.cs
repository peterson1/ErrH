using System;
using System.IO;
using ErrH.D7Poster.WPF.DTOs;
using ErrH.Tools.Drupal7Models.DTOs;

namespace ErrH.D7Poster.WPF.Models
{
    public class LogEntry : D7NodeMap<LogEntryDTO>
    {
        public string    Title      { get; }
        public string    Message    { get; }
        public DateTime  EventTime  { get; }



        public LogEntry(string title, FileInfo file)
        {
            Title     = title;
            Message   = File.ReadAllText(file.FullName);
            EventTime = ParseDate(file.Name);
        }


        public static DateTime ParseDate(string fileName)
        {
            var nme = fileName.Replace(".log", "");
            var ss  = nme.Split('_');
            var dte = ss[0];
            var tme = ss[1].Replace("-", ":");
            return DateTime.Parse($"{dte} {tme}");
        }
    }
}
