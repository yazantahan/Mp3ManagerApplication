using System;

namespace MP3ManagerApplication.Pages.UI
{
    class DeleteUI : IUserInterface
    {
        int choice;

        private static DeleteUI deleteUI;

        private DeleteUI()
        {

        }

        public static DeleteUI start()
        {
            if (deleteUI == null)
            {
                deleteUI = new DeleteUI();
            }

            return deleteUI;
        }
        public void Run(ref MP3Engine mp3Engine)
        {
            Console.Clear();

            if (mp3Engine.getMP3FilesSize() == 0)
            {
                Console.WriteLine("There's no mp3 files contained in this directory, Press any key to go back...");
                Console.ReadKey();
            }
            else
            {
                mp3Engine.printAllMP3Files();

                Console.WriteLine("\n\n---------------------------------\n" +
                                    "Select one of the .mp3 file you would like to delete? (Type " + Convert.ToString(mp3Engine.getMP3FilesSize() + 1) + " to go back)");

                while (true)
                {
                    choice = Prog.input();
                    if (choice == mp3Engine.getMP3FilesSize() + 1)
                    {
                        break;
                    }

                    if (mp3Engine.validateMP3File(choice) == MP3Engine.FILE_EXISTS)
                    {
                        Prog.Logger.Info("MP3 File validated named -> " + mp3Engine.getMP3FileName(choice));
                        Console.WriteLine("Are you sure you want to delete this file?\n\n--> " + mp3Engine.getMP3FileName(choice) + "\n\n(Type Y if you want)");

                        if (Prog.isYes())
                        {
                            Prog.Logger.Info("Deleting the mp3 filename named -> " + mp3Engine.getMP3FileName(choice));
                            Console.WriteLine("Deleting the selected .mp3 File");

                            mp3Engine.deleteMP3File(choice);
                            mp3Engine.refreshList();

                            Prog.Logger.Info("Deleted the filename named -> " + mp3Engine.getMP3FileName(choice));
                            Console.WriteLine("The mp3 file has been deleted successfully! Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_IS_EMPTY)
                    {
                        Prog.Logger.Error("User didn't enter an index or a number, Error code -> " + MP3Engine.INDEX_IS_EMPTY);
                        Prog.setWarningMessage("\nMake sure you entered the index in order to continue.");
                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_NOT_INTEGER)
                    {
                        Prog.Logger.Error("User typed a non-integer, Error code -> " + MP3Engine.INDEX_NOT_INTEGER);
                        Prog.setWarningMessage("\nYou need to enter numbers only, check from the index above.");
                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_OUT_IT_RANGE)
                    {
                        Prog.Logger.Error("User Typed a number that it's out of range, Error code -> " + MP3Engine.INDEX_OUT_IT_RANGE);
                        Prog.setWarningMessage("\nMake sure you enter an index in order to continue.");
                    }
                }
            }

            Console.Clear();
        }

        public void Dispose()
        {
            choice = 0;
            deleteUI = null;
        }
    }
}
