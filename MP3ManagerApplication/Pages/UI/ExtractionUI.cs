using System;

namespace MP3ManagerApplication.Pages.UI
{
    class ExtractionUI : IUserInterface
    {
        private static ExtractionUI extractionUI;

        private ExtractionUI()
        {

        }

        public static ExtractionUI start()
        {
            if (extractionUI == null)
            {
                extractionUI = new ExtractionUI();
            }

            return extractionUI;
        }

        public void Run(ref MP3Engine mp3Engine)
        {
            Console.Clear();

            if (mp3Engine.checkFolders())
            {
                Console.WriteLine("Are you sure you want to extract all the .mp3 files from the subfolders? (Type Y if you want)");

                if (Prog.isYes())
                {
                    Prog.isAsync = false;
                    Prog.Logger.Info("Starting to extract .mp3 files");
                    mp3Engine.extractMP3Files();

                    Console.WriteLine("All the .mp3 files has been extracted successfully!\nPress any key to continue.");
                    Console.ReadKey();
                    Console.WriteLine();
                    Prog.isAsync = true;
                }
            }
            else
            {
                Prog.Logger.Warn("No folders are available.");
                Console.WriteLine("You\'re unable to use this feature because there\'s no folders in this selected directory.\nPress any key to go back.");
                Console.ReadKey();
            }

            Console.Clear();
        }

        public void Dispose()
        {
            extractionUI = null;
        }
    }
}
