using System;
using System.IO;
using ErrH.D7Poster.WPF.DataAccess;
using ErrH.D7Poster.WPF.Models;
using PropertyChanged;
using ServiceStack;

namespace ErrH.D7Poster.WPF.ViewModels
{
    [ImplementPropertyChanged]
    public class TransmittalVM
    {
        private D7Client _d7c = new D7Client();

        public event EventHandler Completed;

        public FileInfo File { get; private set; }
        public int Attempts { get; private set; }


        public void Send(string title, FileInfo file)
        {
            File = file;
            var entry = new LogEntry(title, file);

            _d7c.AttemptFailed += (s, e) => Attempts++;

            _d7c.Posted += (s, e) 
                => Completed?.Invoke(this, EventArgs.Empty);

            _d7c.PersistentPost(entry);
        }

    }
}
