using CommandLine;
using CommandLine.Text;

namespace ErrH.D7Poster.WPF.Configuration
{
    class CmdOptions
    {
        internal static CmdOptions CmdOpt = new CmdOptions();


        [Option('h', "stealth-mode", 
            Required = false, DefaultValue = false,
            HelpText = "Stealth mode.")]
        public bool StealthMode { get; set; }



        [HelpOption]
        public string GetUsage()
        {
            var h = new HelpText
            {
                Heading = new HeadingInfo("D7 Poster"),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            h.AddOptions(this);
            return h;
        }
    }
}
