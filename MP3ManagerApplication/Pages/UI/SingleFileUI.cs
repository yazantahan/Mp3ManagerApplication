using System;

namespace MP3ManagerApplication.Pages.UI
{
    class SingleFileUI : IUserInterface
    {
        private int choice;
        private int selected_index;
        private string artist, title, album;
        private bool moveLegal = false;
        public bool isOrganized = false;

        private static SingleFileUI singleFileUI;

        private SingleFileUI()
        {

        }

        public static SingleFileUI start()
        {
            if (singleFileUI == null)
            {
                singleFileUI = new SingleFileUI();
            }

            return singleFileUI;
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

                while (true)
                {
                    if (mp3Engine.getMP3FilesSize() == 0)
                    {
                        Console.WriteLine("There's no mp3 files contained in this directory, Press any key to go back...");
                        Console.ReadKey();
                        break;
                    }

                    mp3Engine.printAllMP3Files();

                    Console.WriteLine("\n\nPlease select one of the .mp3 files you would like to Organize. (Type " + Convert.ToString(mp3Engine.getMP3FilesSize() + 1) + " to go back)");
                    selected_index = Prog.input();

                    if (selected_index == 0)
                    {
                        continue;
                    }

                    if (selected_index == mp3Engine.getMP3FilesSize() + 1)
                    {
                        break;
                    }

                    if (mp3Engine.validateMP3File(selected_index) == MP3Engine.FILE_EXISTS)
                    {
                        Prog.Logger.Info("The .MP3 validated named -> " + mp3Engine.getMP3FileName(selected_index));
                        Console.Clear();

                        while (true)
                        {
                            Console.WriteLine("Would you like to:" +
                                "\n1- Format from filename to .mp3 info" +
                                "\nNote: Make sure before you select this option to rename this .mp3 filename using this format -> ARTIST - TITLE.mp3" +
                                "\n2- Format from .mp3 info to The filename" +
                                "\n3- Edit the .mp3 info" +
                                "\n4- Go back");

                            choice = Prog.input();

                            if (choice == 1)
                            {
                                Console.Clear();

                                Console.WriteLine("Analyzing the file by taking the artist name and title name");

                                artist = mp3Engine.getArtistFromFileName(selected_index);
                                title = mp3Engine.getTitleFromFileName(selected_index);

                                if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Editing the .mp3 info");
                                    
                                    mp3Engine.editMP3Info(artist, title, selected_index);

                                    Console.WriteLine("The .mp3 info has been edited, Would you like to move it to the artist folder? (type Y if you want)");

                                    if (Prog.isYes())
                                    {
                                        Prog.Logger.Info("Moving to the artist folder named -> " + mp3Engine.getMP3FileName(selected_index));
                                        Console.WriteLine("Moving to the artist folder...");
                                        mp3Engine.moveToArtistFolder(artist, title, selected_index);

                                        Console.WriteLine("The File has been moved successfully!");
                                    }

                                    Prog.Logger.Info("The file moved Successfully!");
                                    Console.WriteLine("The file has been organized successfully!");
                                    Console.WriteLine("\nPress any key to continue...");
                                    Console.ReadKey();

                                    isOrganized = true;

                                    Console.Clear();

                                    break;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(artist))
                                    {
                                        Prog.Logger.Error("The artist field is empty in the file named -> " + mp3Engine.getMP3FileName(selected_index));
                                        Prog.setWarningMessage("The artist field is empty, make sure you typed the artist correctly by using this format ARTIST – TITLE.mp3");
                                    }

                                    if (string.IsNullOrEmpty(title))
                                    {
                                        Prog.Logger.Error("The title field is empty in the file named -> " + mp3Engine.getMP3FileName(selected_index));
                                        Prog.setWarningMessage("The title field is empty, make sure you typed the title correctly by using this format ARTIST – TITLE.mp3");
                                    }

                                    Console.ReadKey();
                                }
                            }
                            else if (choice == 2)
                            {
                                Console.Clear();

                                Console.WriteLine("Getting the Artist and the Title Field from .mp3 info");

                                artist = mp3Engine.getArtistField(selected_index);
                                title = mp3Engine.getTitleField(selected_index);

                                if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
                                {
                                    Console.WriteLine("Would you like to move it to the artist folder? (Type Y if you want)");

                                    if (Prog.isYes())
                                    {
                                        Console.WriteLine("Moving to the artist folder");

                                        moveLegal = true;
                                    }

                                    mp3Engine.renameMP3FileName(artist, title, selected_index, moveLegal);

                                    mp3Engine.refreshList();

                                    Console.WriteLine("The file has been organized successfully!");
                                    Console.WriteLine("\nPress any key to continue...");
                                    Console.ReadKey();

                                    Console.Clear();

                                    isOrganized = true;

                                    break;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(artist))
                                    {
                                        Prog.Logger.Error("The artist info is empty in the file named -> " + mp3Engine.getMP3FileName(selected_index));
                                        Prog.setWarningMessage("The artist info from this .mp3 file is empty. if you want to type the artist name, select the option 3");
                                    }

                                    if (string.IsNullOrEmpty(title))
                                    {
                                        Prog.Logger.Error("The title info is empty in the file named -> " + mp3Engine.getMP3FileName(selected_index));
                                        Prog.setWarningMessage("The title info from this .mp3 file is empty. if you want to type the title name, select the option 3");
                                    }

                                    Console.ReadKey();
                                }
                            }
                            else if (choice == 3)
                            {
                                Console.Clear();

                                while (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(album))
                                {
                                    Console.Write("Enter the artist name: ");
                                    artist = Console.ReadLine();

                                    Console.Write("Enter the title name: ");
                                    title = Console.ReadLine();

                                    Console.Write("Enter the album name: ");
                                    album = Console.ReadLine();

                                    Console.Clear();
                                    if (string.IsNullOrEmpty(artist))
                                    {
                                        Prog.setWarningMessage("Make sure you enter info about the artist in order to continue");
                                    }

                                    if (string.IsNullOrEmpty(title))
                                    {
                                        Prog.setWarningMessage("Make sure you enter info about the title in order to continue");
                                    }

                                    if (string.IsNullOrEmpty(album))
                                    {
                                        Prog.setWarningMessage("Make sure you enter info about the album in order to continue");
                                    }
                                }

                                Console.WriteLine("Editing the .mp3 info");
                                mp3Engine.editMP3Info(artist, title, album, selected_index);

                                Console.WriteLine("Edited!");
                                Console.WriteLine("Would you like to rename the filename? (Type Y if you want)");

                                if (Prog.isYes())
                                {
                                    Console.WriteLine("Would you like to move it to the artist folder? (Type Y if you want)");

                                    if (Prog.isYes())
                                    {
                                        Console.WriteLine("Moving to the artist folder...");
                                        moveLegal = true;
                                    }

                                    mp3Engine.renameMP3FileName(artist, title, selected_index, moveLegal);
                                }

                                mp3Engine.refreshList();

                                Console.WriteLine("The File has been organized successfully!");
                                Console.WriteLine("\nPress any key to continue...");
                                Console.ReadKey();

                                Console.Clear();

                                isOrganized = true;

                                break;
                            }
                            else if (choice == 4)
                            {
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Prog.setWarningMessage("Make sure you entered an index in order to continue\n");
                            }
                        }

                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_IS_EMPTY)
                    {
                        Prog.Logger.Error("User didn't enter an index or a number, Error code -> " + MP3Engine.INDEX_IS_EMPTY);
                        Prog.setWarningMessage("Make sure you enter the index in order to continue.");
                        continue;
                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_NOT_INTEGER)
                    {
                        Prog.Logger.Error("User typed a non-integer, Error code -> " + MP3Engine.INDEX_NOT_INTEGER);
                        Prog.setWarningMessage("You need to enter numbers only, check from the index above.");
                        continue;
                    }
                    else if (mp3Engine.validateMP3File(choice) == MP3Engine.INDEX_OUT_IT_RANGE)
                    {
                        Prog.Logger.Error("User Typed a number that it's out of range, Error code -> " + MP3Engine.INDEX_OUT_IT_RANGE);
                        Prog.setWarningMessage("You entered the wrong index, Make sure you entered the corrected index as shown above.");
                        continue;
                    }
                }
            }

            Console.Clear();
        }

        public void Dispose()
        {
            choice = 0;
            artist = title = album = null;
            isOrganized = false;
            moveLegal = false;
            singleFileUI = null;
        }

    }
}
