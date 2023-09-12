using MP3ManagerApplication.Pages.UI;
using NLog;
using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace MP3ManagerApplication
{
    public static class Prog
    {
        private static DeleteUI deleteUI;
        private static DirectoryUI directoryUI;
        private static ExtractionUI extractionUI;
        private static OrganizeUI organizeUI;
        private static PrintListUI printListUI;

        private static int choice = 0;

        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static bool isAsync = false;

        [STAThread]
        public static void Main()
        {
            configLogger();

            setDefaultColor();
            MP3Engine mp3Engine = new MP3Engine();

            directoryUI = DirectoryUI.Start(DirectoryUI.BROWSER_SELECT);
            directoryUI.Run(ref mp3Engine);
            directoryUI.Dispose();

            isAsync = true;

            Thread thread = new Thread(()=>KeepRefresh(ref mp3Engine));
            thread.Start();


            while (choice != 7 && DirectoryUI.isSelected == true)
            {
                Console.WriteLine(
                    "\n           *--------------------------------------------------------------------------*               " +
                    "\n           |                       Welcome to the MP3 Manager                         |               " +
                    "\n           |  Select one of the options you would like and hit \'Enter\'  to continue   |               " +
                    "\n           *--------------------------------------------------------------------------*               " +
                    "\n                                                                                                      " +
                    "\n----------------------------------------------------------------------------------------------------\n" +
                    "\n                                 1- Print all the .mp3 Files.                                         " +
                    "\n                              2- Organize a Single or Multiple Files                                  " +
                    "\n                         3- Extract all The .mp3 files from child Folders                             " +
                    "\n                                     4- Delete a .mp3 file                                            " +
                    "\n                                    5- Change the Directory                                           " +
                    "\n                                       6- About the app                                               " +
                    "\n                                       7- Exit the app.                                               "
                    );

                choice = input();

                switch (choice)
                {
                    case 1:
                        Logger.Info("User pressed number 1");
                        printListUI = PrintListUI.start();
                        printListUI.Run(ref mp3Engine);
                        printListUI.Dispose();
                        break;
                    case 2:
                        Logger.Info("User pressed number 2");
                        organizeUI = OrganizeUI.start();
                        organizeUI.Run(ref mp3Engine);
                        organizeUI.Dispose();
                        break;
                    case 3:
                        Logger.Info("User pressed number 3");
                        extractionUI = ExtractionUI.start();
                        extractionUI.Run(ref mp3Engine);
                        extractionUI.Dispose();
                        break;
                    case 4:
                        Logger.Info("User pressed number 4");
                        deleteUI = DeleteUI.start();
                        deleteUI.Run(ref mp3Engine);
                        deleteUI.Dispose();
                        break;
                    case 5:
                        Logger.Info("User pressed number 5");
                        directoryUI = DirectoryUI.Start(DirectoryUI.BROWSER_CHANGE);
                        directoryUI.Run(ref mp3Engine);
                        directoryUI.Dispose();
                        break;
                    case 6:
                        Logger.Info("User pressed number 6");
                        MessageBox.Show("This Application is used to let you manage the MP3 Files for either a single .mp3 file or multiple files in one shot and organize the main folder you’ve selected where it contains the .mp3 files" +
                            "\nDeveloped by: Yazan Al - Tahan." +
                            "\nIf there’s any feedback, please send me an Email: thn.soperma@gmail.com", "About the app", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Console.Clear();
                        break;
                    case 7:
                        Logger.Info("User pressed number 7");
                        Console.WriteLine("Are you sure you want to exit the Application? (Type Y if you want)");

                        if (!isYes())
                        {
                            choice = 0;
                            Console.Clear();
                            continue;
                        }

                        Logger.Info("User closes the app");
                        break;
                    default:
                        Logger.Info("User pressed some Random number");
                        Console.Clear();
                        setWarningMessage("Please enter the Following options to continue.\n");
                        break;
                }
            }

            if (choice == 7)
            {
                Console.WriteLine(
                    "\n--------------------------------------------------------------------------------------------------------" +
                    "\n------------                        Thank you for using this app.                        ---------------" +
                    "\n------------                  Created and developed by Yazan Al-Tahan                    ---------------" +
                    "\n------------  If there is any feedback please send us an email: thn.soperma@gmail.com    ---------------" +
                    "\n------------                        Press any key to continue...                         ---------------" +
                    "\n--------------------------------------------------------------------------------------------------------");
                Console.ReadKey();
            }


        }

        static void setDefaultColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void setWarningMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(msg);

            setDefaultColor();
        }

        public static void setCautionMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(msg);

            setDefaultColor();
        }

        public static int input()
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                setWarningMessage("Make sure you entered a number in order to continue");
            }

            return num;
        }

        public static bool isYes()
        {
            bool Yes;
            try
            {
                Yes = (Console.ReadKey().Key == ConsoleKey.Y) ? true : false;
            }
            catch (FormatException)
            {
                Yes = false;
            }

            return Yes;
        } 

        public static double calcPercentage(double subTotal, double total)
        {
            if (subTotal > total)
            {
                return 1.0;
            }

            double percentage = (subTotal / total);

            return percentage;
        }

        private static void configLogger()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "Logs\\" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + ".txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        private static void KeepRefresh(ref MP3Engine mp3Engine)
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (isAsync == true)
                {
                    mp3Engine.refreshList();
                }
            }
        }
    }
}
