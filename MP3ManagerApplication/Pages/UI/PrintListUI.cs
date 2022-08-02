using System;

namespace MP3ManagerApplication.Pages.UI
{
    class PrintListUI : IUserInterface
    {

        private static PrintListUI printListUI;
        private PrintListUI()
        {

        }

        public static PrintListUI start()
        {
            if (printListUI == null)
            {
                printListUI = new PrintListUI();
            }

            return printListUI;
        }

        public void Run(ref MP3Engine mp3Engine)
        {
            Console.Clear();

            if (mp3Engine.getMP3FilesSize() != 0)
            {
                mp3Engine.printAllMP3Files();
                Console.WriteLine("\n--------------------------------------\n\nPress any key to go back.");
            }
            else
            {
                Console.WriteLine("This file is empty, Press any key to go back.");
            }

            Console.ReadKey();
            Console.Clear();
        }

        public void Dispose()
        {
            printListUI = null;
        }
    }
}
