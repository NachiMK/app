using System.Text;
using CommandLine;

namespace RestoreSQLBackups
{

    /// <summary>
    /// Command line Options for Restore backups
    /// </summary>    
   class CmdOptions
    {
        [Option('i', Required = false, HelpText = "Input file to read configurations from.")]
        public string InputFile { get; set; }

        [Option('u', Required = false, HelpText = "Runs the User interface.")]
        public bool UseUI { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("Restore SQL Backups");
            usage.AppendLine("To use a configuration file:");
            usage.AppendLine(" \t RestoreSQLBackups -i test.xml");
            usage.AppendLine("or to use the User Interface:");
            usage.AppendLine("\t RestoreSQLBackups -u true");
            return usage.ToString();
        }
    }

}
