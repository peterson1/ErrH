using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ErrH.D7Poster.WPF.Configuration;
using ErrH.D7Poster.WPF.Models;
using ErrH.Tools.Extensions;
using ErrH.Wpf.net45.Extensions;
using PropertyChanged;

namespace ErrH.D7Poster.WPF.ViewModels
{
    [ImplementPropertyChanged]
    public class TargetFolderVM
    {
        private const int MAX_ARCHIVE = 30;
        private SynchronizationContext _ui;

        public ObservableCollection<FileInfo>      Pending { get; } = new ObservableCollection<FileInfo>();
        public ObservableCollection<TransmittalVM> OnGoing { get; } = new ObservableCollection<TransmittalVM>();
        public ObservableCollection<string>        Archive { get; } = new ObservableCollection<string>();

        public FileSystemWatcher   Watcher { get; } = new FileSystemWatcher();
        public SettingsCfg.Target  Target  { get; }

        //public string Label { get; }

        //public string FolderPath => Folder.FullName;



        public TargetFolderVM(SettingsCfg.Target target)
        {
            Target = target;
            _ui = SynchronizationContext.Current;

            Pending.CollectionChanged += Pending_CollectionChanged;

            ProcessExistingFiles();

            WatchForNewFiles();
        }


        private void ProcessExistingFiles()
        {
            var dir = new DirectoryInfo(Target.Source);

            foreach (var file in dir.GetFiles(Target.Filter))
            {
                if (!Pending.Has(x => x.Name == file.Name))
                    _ui.Send(x => Pending.Add(file), null);
            }
        }


        private void WatchForNewFiles()
        {
            Watcher.Path         = Target.Source;
            Watcher.Filter       = Target.Filter;
            Watcher.NotifyFilter = NotifyFilters.LastWrite;
            Watcher.Created     += OnContentsChanged;
            Watcher.Changed     += OnContentsChanged;

            Watcher.EnableRaisingEvents = true;
        }


        private void OnContentsChanged(object sender, FileSystemEventArgs e)
        {
            var file = new FileInfo(e.FullPath);

            if (!Pending.Has(x => x.Name == file.Name))
                _ui.Send(x => Pending.Add(file), null);

            ProcessExistingFiles();
        }


        private async void Pending_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;

            foreach (FileInfo file in e.NewItems)
            {
                while (IsCurrentMinute(file))
                    await Task.Delay(1000 * 15);

                var trans = new TransmittalVM();
                OnGoing.Add(trans);
                trans.Completed += (s, a) => MoveToArchive(trans);
                trans.Send(Target.Title, file);
            }
        }

        private bool IsCurrentMinute(FileInfo file)
        {
            var str = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            var now = DateTime.Parse(str);
            var log = LogEntry.ParseDate(file.Name);
            var same = now == log;
            return same;
        }

        private void MoveToArchive(TransmittalVM trans)
        {
            var file  = trans.File;
            file.MoveTo(ArchivePath(file));

            Archive.Add(file.Name);
            if (Archive.Count > MAX_ARCHIVE) Archive.RemoveAt(0);

            OnGoing.Remove(trans);
            Pending.Remove(file);
        }


        private string ArchivePath(FileInfo file)
        {
            var dir = Target.Archive;
            if (dir.IsBlank())
                dir = file.Directory.CreateSubdirectory("archive").FullName;

            Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, file.Name);
            return FileInfoEx.IncrementFileName(path);
        }
    }

}
