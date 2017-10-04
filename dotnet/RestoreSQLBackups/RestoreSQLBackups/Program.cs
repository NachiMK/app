using System;
using RestoreSQLBackupLibrary;
using System.Windows.Forms;
using System.Collections.Generic;

namespace RestoreSQLBackups
{
    /// <summary>
    /// Startup class for SQL Restore 
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            var cmdOptions = new CmdOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, cmdOptions))
            {
                if (string.IsNullOrEmpty(cmdOptions.InputFile) &&
                    !cmdOptions.UseUI)
                {
                    PrintUsage(cmdOptions);
                }
                else
                {
                    if (!string.IsNullOrEmpty(cmdOptions.InputFile))
                        RunTool(cmdOptions); 
                    else
                        ShowUI();
                }
            }
            else
            {
                PrintUsage(cmdOptions);
            }
        }

        private static void ShowUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RestoreProdToDBTestForm());
        }

        private static void RunTool(CmdOptions cmdOptions)
        {
            if (!string.IsNullOrEmpty(cmdOptions.InputFile))
            {
                if (!System.IO.File.Exists(cmdOptions.InputFile))
                    Console.WriteLine("Input file {0} doesn't exists.", cmdOptions.InputFile);
                else
                {
                    DBRestoreParam dBRestoreParam = DBRestoreParam.NewDBRestoreParam(cmdOptions.InputFile);
                    List<string> strList = DBRestoreHelper.RestoreCleanupShrinkDB(dBRestoreParam);
                    foreach(string str in strList)
                        Console.WriteLine(str);
                }
            }
        }

        private static void PrintUsage(CmdOptions cmdOptions)
        {
            Console.WriteLine(cmdOptions.GetUsage());
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
