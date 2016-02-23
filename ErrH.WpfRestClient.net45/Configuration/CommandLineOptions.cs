using CommandLine;
using CommandLine.Text;

namespace ErrH.WpfRestClient.net45.Configuration
{
    public abstract class CommandLineOptions
    {
        [Option('h', "start-hidden",
            Required = false, DefaultValue = false,
            HelpText = "Start hidden.")]
        public bool StartHidden { get; set; }


        [Option('n', "no-updates",
            Required = false, DefaultValue = false,
            HelpText = "No checking for updates.")]
        public bool NoUpdates { get; set; }


        protected abstract string AppTitle { get; }


        [HelpOption]
        public string GetUsage()
        {
            var h = new HelpText
            {
                Heading = new HeadingInfo(AppTitle),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            h.AddOptions(this);
            return h;
        }
    }
}
