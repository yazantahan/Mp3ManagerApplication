using System;
using System.Collections.Generic;
using System.Threading;

namespace MP3ManagerApplication.Pages.UI
{
    class MultipleFilesUI : IUserInterface
    {
        private int choice;
        private bool moveLegal;
        private List<string> nonFormattable;
        public bool isOrganized = false;

        private static MultipleFilesUI multipleFilesUI;

        private MultipleFilesUI()
        {
            moveLegal = false;
            nonFormattable = new List<string>();
        }

        public static MultipleFilesUI start()
        {
            if (multipleFilesUI == null)
            {
                multipleFilesUI = new MultipleFilesUI();
            }

            return multipleFilesUI;
        }

        [Obsolete]
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
                Console.WriteLine(
                    "Would you like to:" +
                    "\n1- Format from the .mp3 filename to the .mp3 info" +
                    "\n2- Format from the .mp3 info to the .mp3 filename" +
                    "\n3- Go back");

                while (true)
                {
                    choice = Prog.input();

                    if (choice == 1)
                    {
                        Console.WriteLine("Would you like to Move the .mp3 files to the Artist folders? (Type Y if you want)");

                        if (Prog.isYes())
                            moveLegal = true;

                        Prog.isAsync = false;

                        Console.Clear();
                        for (int i = 1; i <= mp3Engine.getMP3FilesSize(); i++)
                        {
                            string artist = mp3Engine.getArtistFromFileName(i);
                            string title = mp3Engine.getTitleFromFileName(i);

                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("\rProcessing the file named: " + mp3Engine.getMP3FileName(i));
                            Console.Write(String.Format("\n\rTotal files completed: {0:P2}", Prog.calcPercentage(i - 1, mp3Engine.getMP3FilesSize())));

                            if (nonFormattable.Count != 0)
                            {
                                Prog.setCautionMessage("\n\rNon-formattable files total: " + Convert.ToString(nonFormattable.Count));
                            }

                            if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
                            {
                                mp3Engine.editMP3Info(artist, title, i);

                                if (moveLegal)
                                    mp3Engine.moveToArtistFolder(artist, title, i);
                            }
                            else
                            {
                                nonFormattable.Add(mp3Engine.getMP3FileName(i));
                            }
                            Thread.Sleep(50);
                        }
                        Prog.isAsync = true;

                        mp3Engine.refreshList();

                        Console.Clear();

                        Console.WriteLine("All the .mp3 files has been Organized successfully!");

                        if (nonFormattable.Count != 0)
                        {
                            Prog.setCautionMessage("\n\nHere are some .mp3 files that are un-formattable: \n" + getNonFormattableFiles());
                        }

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();

                        Console.Clear();

                        isOrganized = true;
                        break;
                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine("Would you like to Move the .mp3 files to the Artist folders? (Type Y if you want)");

                        if (Prog.isYes())
                            moveLegal = true;

                        Prog.isAsync = false;

                        Console.Clear();
                        for (int i = 1; i <= mp3Engine.getMP3FilesSize(); i++)
                        {
                            string artist = mp3Engine.getArtistField(i);
                            string title = mp3Engine.getTitleField(i);

                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("\rProcessing the file named: " + mp3Engine.getMP3FileName(i));
                            Console.Write(String.Format("\n\rTotal files completed: {0:P2}", Prog.calcPercentage(i - 1, mp3Engine.getMP3FilesSize())));

                            if (nonFormattable.Count != 0)
                            {
                                Prog.setCautionMessage("\n\rNon-formattable files total: " + Convert.ToString(nonFormattable.Count));
                            }

                            if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
                            {
                                mp3Engine.renameMP3FileName(artist, title, i, moveLegal);
                            }
                            else
                            {
                                nonFormattable.Add(mp3Engine.getMP3FileName(i));
                            }

                            Thread.Sleep(50);
                        }
                        Prog.isAsync = true;

                        mp3Engine.refreshList();

                        Console.Clear();

                        Console.WriteLine("All the .mp3 files has been Organized successfully!");

                        if (nonFormattable.Count != 0)
                        {
                            Prog.setCautionMessage("\n\nHere are some .mp3 files that are un-formattable: \n" + getNonFormattableFiles());
                        }

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();

                        Console.Clear();

                        isOrganized = true;
                        break;
                    }
                    else if (choice == 3)
                    {
                        break;
                    }
                }
            }

            Console.Clear();
        }

        public void Dispose()
        {
            choice = 0;
            nonFormattable = null;
            isOrganized = false;

            multipleFilesUI = null;
        }

        private string getNonFormattableFiles()
        {
            string text = "";
            for (int i = 0; i < nonFormattable.Count; i++)
            {
                int pointer = i + 1;

                text += '\n' + Convert.ToString(pointer) + "- " + nonFormattable[i];
            }

            return text;
        }
    }
}
