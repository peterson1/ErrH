using System;

namespace ErrH.Tools.ScalarEventArgs
{
    public class EventArg
    {
        public static PathEventArg Path(string value) { return new PathEventArg(value); }
        public static UrlEventArg Url(string value) { return new UrlEventArg(value); }
        public static EnableEventArg Enable(bool value) { return new EnableEventArg(value); }
    }


    public class PathEventArg : EventArgs
    {
        public readonly string Path;
        public PathEventArg(string value) { this.Path = value; }
        public override int GetHashCode() { return this.Path.GetHashCode(); }
    }


    public class UrlEventArg : EventArgs
    {
        public readonly string Url;
        public UrlEventArg(string value) { this.Url = value; }
        public override int GetHashCode() { return this.Url.GetHashCode(); }
    }


    public class EnableEventArg : EventArgs
    {
        public readonly bool Enabled;
        public EnableEventArg(bool value) { this.Enabled = value; }
        public override int GetHashCode() { return this.Enabled.GetHashCode(); }
    }

}
