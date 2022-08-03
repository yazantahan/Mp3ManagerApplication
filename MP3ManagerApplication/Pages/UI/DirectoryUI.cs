using System;
using System.Windows.Forms;

namespace MP3ManagerApplication.Pages.UI
{
    class DirectoryUI : IUserInterface
    {
        public const int BROWSER_SELECT = 1;
        public const int BROWSER_CHANGE = 2;

        public static bool isSelected = false;

        private static DirectoryUI directoryUI;

        private int choice;

        private FolderBrowserDialog fb;

        private DirectoryUI(int choice)
        {
            this.choice = choice;

            fb = new FolderBrowserDialog();
        }

        public static DirectoryUI Start(int choice)
        {
            if (directoryUI == null)
            {
                directoryUI = new DirectoryUI(choice);
            }

            return directoryUI;
        }

        public void Run(ref MP3Engine mp3Engine)
        {
            Console.Clear();
            fb = new FolderBrowserDialog();
            DialogResult dr;

            if (choice == BROWSER_SELECT)
            {
                Console.WriteLine("Welcome to the MP3 Manager.\nplease enter the directory you would like to organize the MP3 Files to continue.");
                while (true)
                {
                    dr = fb.ShowDialog();
                    fb.ShowNewFolderButton = false;

                    if (dr == DialogResult.OK)
                    {
                        isSelected = true;
                        mp3Engine = MP3Engine.setDirectoryPath(fb.SelectedPath);
                        break;
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else if (choice == BROWSER_CHANGE)
            {
                dr = fb.ShowDialog();
                fb.ShowNewFolderButton = false;

                if (dr == DialogResult.OK)
                {
                    mp3Engine.changeDirectoryPath(fb.SelectedPath);
                }
            }
            else
            {
                throw new IndexOutOfRangeException("You entered a wrong Index.");
            }
            Console.Clear();
        }

        public void Dispose()
        {
            directoryUI = null;
            fb = null;
            choice = 0;
        }
    }
}
