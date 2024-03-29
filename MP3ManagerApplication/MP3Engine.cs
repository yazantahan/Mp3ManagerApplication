﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MP3ManagerApplication
{
    public class MP3Engine
    {
        /// <summary>
        /// This MP3Engine response is called when the selected file is exists
        /// </summary>
        public const int FILE_EXISTS = 301;

        /// <summary>
        /// This MP3Engine respont is called when the current input is not an integer
        /// </summary>
        public const int INDEX_NOT_INTEGER = 302;

        /// <summary>
        /// This MP3Engine response is called when the current input is out of range (depends on the mp3 File list size)
        /// </summary>
        public const int INDEX_OUT_IT_RANGE = 303;

        /// <summary>
        /// This MP3Engine response is called when the current is null or empty
        /// </summary>
        public const int INDEX_IS_EMPTY = 304;

        public const int INDEX_VALID = 401;

        private string mainDirectoryPath;
        private static MP3Engine mp3Engine;

        private List<string> mp3Files;
        private FolderCollector fc;
        private FileManager fm;

        private List<string> initNewMP3Files()
        {
            var subMP3Files = new List<string>();
            return subMP3Files;
        }

        private List<string> initChildFolders()
        {
            var folders = Directory.GetDirectories(mainDirectoryPath);
            Array.Sort(folders);

            var listFolders = new List<string>();

            foreach (var value in folders)
            {
                listFolders.Add(value);
            }

            folders = null;

            return listFolders;
        }

        private MP3Engine(string directoryPath)
        {
            mp3Files = new List<string>();
            mainDirectoryPath = directoryPath;

            var arr = Directory.GetFiles(directoryPath, "*.mp3");

            Array.Sort(arr);

            foreach (var value in arr)
            {
                mp3Files.Add(value);
            }

            arr = null;

            fc = FolderCollector.init(directoryPath);
            fm = new FileManager();
        }

        public MP3Engine()
        {

        }

        public static MP3Engine setDirectoryPath(string directoryPath)
        {
            if (mp3Engine == null)
            {
                mp3Engine = new MP3Engine(directoryPath);
            }

            return mp3Engine;
        }

        /// <summary>
        /// it gets the file name of the .mp3 file
        /// </summary>
        /// <param name="index"> The selected index from the list of .mp3 files </param>
        /// <returns>Returns INDEX_OUT_OF_RANGE or number 4 if it's the selected index is out of range, and it Returns .mp3 file name if it's validated</returns>
        public string getMP3FileName(int index)
        {
            try
            {
                return Path.GetFileName(mp3Files.ElementAt(index - 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                return INDEX_OUT_IT_RANGE.ToString();
            }
        }

        /// <summary>
        /// it gets the artist info from inside the .mp3 file
        /// </summary>
        /// <param name="index"> The selected index from the list of .mp3 file</param>
        /// <returns>Returns empty if the index is out of the range, and it return the artist if it's valid</returns>
        public string getArtistField(int index)
        {
            try
            {
                return TagLib.File.Create(mp3Files.ElementAt(index - 1)).Tag.Artists[0];
            }
            catch (ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// it gets the title info from inside the .mp3 file
        /// </summary>
        /// <param name="index"> The selected index from the list of .mp3 file</param>
        /// <returns>Returns empty if the index is out of the range, and it return the title if it's valid</returns>
        public string getTitleField(int index)
        {
            try
            {
                return TagLib.File.Create(mp3Files.ElementAt(index - 1)).Tag.Title;
            }
            catch (ArgumentOutOfRangeException)
            {
                return "";
            }
        }

        /// <summary>
        /// It gets the artist from the mp3 filename using an algorithm to take the artist using this format -> ARTIST - TITLE.mp3
        /// </summary>
        /// <param name="index"> the index that's from the .mp3 files list</param>
        /// <returns> returns null if there's no artist in the .mp3 filename, and it returns string if it's processed and there's artist depending on the filename's format</returns>
        public string getArtistFromFileName(int index)
        {
            string filename = Path.GetFileName(mp3Files.ElementAt(index - 1));

            string artistName = "";

            Queue<char> queue_artistName = new Queue<char>();
            for (int currentCharIndex = 0; currentCharIndex < filename.Length; currentCharIndex++)
            {
                if (filename.Substring(currentCharIndex, 2) == " -" || filename[currentCharIndex] == '-')
                {
                    break;
                }
                else if (filename.Substring(currentCharIndex) == ".mp3")
                {
                    return "";
                }
                else
                {
                    queue_artistName.Enqueue(filename[currentCharIndex]);
                }
            }

            while (queue_artistName.Count != 0)
            {
                artistName += queue_artistName.Dequeue();
            }

            return artistName;
        }

        /// <summary>
        /// It gets the title from the mp3 filename using an algorithm to taking the title using this format -> ARTIST - TITLE.mp3
        /// </summary>
        /// <param name="index">The index that's from the .mp3 files list</param>
        /// <returns>return null if there's no title in the .mp3 filename, and it return string if it's processed and there's title depending on the filename's format</returns>
        public string getTitleFromFileName(int index)
        {
            string filename = Path.GetFileName(mp3Files.ElementAt(index - 1));

            string titleName = "";

            Queue<char> queue_titleName = new Queue<char>();
            for (int currentCharTitle = 0; currentCharTitle < filename.Length; currentCharTitle++)
            {
                int pointer = currentCharTitle;
                if (filename[pointer] == '-')
                {
                    pointer++;
                    while (true)
                    {
                        if (filename[pointer] != ' ')
                        {
                            for (int j = pointer; j < filename.Length; j++)
                            {
                                pointer = j;
                                if (filename.Substring(pointer).Equals(".mp3"))
                                {
                                    break;
                                }
                                else
                                {
                                    queue_titleName.Enqueue(filename[j]);
                                }
                            }

                            if (queue_titleName.Count == 0)
                            {
                                return null;
                            }
                            else
                            {

                                while (queue_titleName.Count != 0)
                                {
                                    titleName += queue_titleName.Dequeue();
                                }
                                return titleName;
                            }
                        }
                        else
                        {
                            pointer++;
                        }
                    }
                }
                else if (filename.Substring(pointer).Equals(".mp3"))
                {
                    break;
                }
            }

            return titleName;
        }

        /// <summary>
        /// It Gets the following MP3Engine responses During this validation function
        /// </summary>
        /// <param name="index">The index that's from the .mp3 files list</param>
        /// <returns> it returns the following:<br/>
        /// 1- FILE_EXISTS -> means that the file is exists<br/>
        /// 2- INDEX_NOT_INTEGER -> means that the index is not an integer<br/>
        /// 3- INDEX_OUT_OF_RANGE -> means that the index is out of range depending on the mp3 list files size<br/>
        /// 4- INDEX_IS_EMPTY -> means that the index is empty (depends if the index parameter is 0 or null)<br/>
        /// </returns>
        public int validateMP3File(Object index)
        {
            int number;

            try
            {
                number = Convert.ToInt32(index);
            }
            catch
            {
                return INDEX_NOT_INTEGER;
            }

            if (number == 0)
            {
                return INDEX_IS_EMPTY;
            }

            number--;

            if (number < 0 || number > mp3Files.Count)
            {
                return INDEX_OUT_IT_RANGE;
            }

            try
            {
                mp3Files.ElementAt(number);
            }
            catch (ArgumentOutOfRangeException)
            {
                return INDEX_OUT_IT_RANGE;
            }

            return FILE_EXISTS;
        }

        /// <summary>
        /// This function used when we want to change the directory path
        /// </summary>
        /// <param name="directoryPath">The directory path that we want to change</param>
        public void changeDirectoryPath(string directoryPath)
        {
            if (directoryPath != mainDirectoryPath)
            {
                mainDirectoryPath = directoryPath;
            }
            refreshList();
        }

        /// <summary>
        /// This Function is used to print the entire MP3 Files with it's index
        /// </summary>
        public void printAllMP3Files()
        {
            for (int i = 0; i < mp3Files.Count; i++)
            {
                int count = i + 1;
                Console.WriteLine(count + "- " + Path.GetFileName(mp3Files[i]));
            }
        }

        /// <summary>
        /// This Function is used to edit the .mp3 information file with the album
        /// </summary>
        /// <param name="artist">The name of the artist that needs to be edited</param>
        /// <param name="title">The name of the title that needs to be edited</param>
        /// <param name="album">The name of the album that needs to be edited</param>
        /// <param name="index">The selected .mp3 file using an integer or number</param>
        public void editMP3Info(string artist, string title, string album, int index)
        {
            editMP3(artist, title, album, index - 1);
        }

        /// <summary>
        /// This Function is used to edit the .mp3 file's information
        /// </summary>
        /// <param name="artist">The name of the artist that needs to be edited</param>
        /// <param name="title">The name of the title that needs to be edited</param>
        /// <param name="index">The selected .mp3 file using an integer or number</param>
        public void editMP3Info(string artist, string title, int index)
        {
            editMP3(artist, title, "", index - 1);
        }

        /// <summary>
        /// This Function is used to rename the .mp3 Filename using this format ARTIST - TITLE.mp3
        /// </summary>
        /// <param name="artist">The artist that need to input the format</param>
        /// <param name="title">The title that need to input the format</param>
        /// <param name="index">The index for the selected MP3 File</param>
        /// <param name="moveLegal">It's used if we want to move the MP3 File to the artist folder</param>
        public void renameMP3FileName(string artist, string title, int index, bool moveLegal)
        {
            //Step 1: Removing the symbols, So that it can works with no errors that comes from the file explorer
            artist = removeSymbols(artist);
            title = removeSymbols(title);

            //Step 2: We will get the .mp3 file path by taking the index and defining string for that
            string filePath = mp3Files.ElementAt(index - 1);

            //Step 3: Without loss of the File, We will use the if-else statement so in that way there's no more .mp3 files loss
            if (moveLegal == true)
            {
                moveToArtistFolder(artist, title, filePath);
            }
            else
            {
                fm.RenameFile(filePath, artist + " - " + title + ".mp3");
            }
        }

        /// <summary>
        /// This Function is used to move the current mp3 file to the Artist folder
        /// </summary>
        /// <param name="artist">The name of the artist</param>
        /// <param name="title">The name of the title</param>
        /// <param name="filePath">The filepath of the located MP3 File</param>
        private void moveToArtistFolder(string artist, string title, string filePath)
        {
            string artistPath = mainDirectoryPath + "\\" + artist;

            if (!Directory.Exists(artistPath))
            {
                Directory.CreateDirectory(artistPath);
            }

            fm.RenameAndMoveFile(filePath, artistPath + "\\" + artist + " - " + title + ".mp3");
        }

        /// <summary>
        /// This Function is used to move the current mp3 file to Artist folder using an index
        /// </summary>
        /// <param name="artist">The name of the artist</param>
        /// <param name="title">The name of the title</param>
        /// <param name="index">The index of the selected MP3 File</param>
        public void moveToArtistFolder(string artist, string title, int index)
        {
            string filePath = mp3Files.ElementAt(index - 1);

            moveToArtistFolder(artist, title, filePath);
        }

        /// <summary>
        /// This Function is used to extract all the .MP3 files from the subfolders (depending on the main directory file we set it early)
        /// </summary>
        public void extractMP3Files()
        {
            /*Initialize the list of a selected .mp3 files*/
            var newMP3Files = initNewMP3Files();

            Console.WriteLine("Checking for subfolders (1/3)");
            Thread.Sleep(1000);

            /*Declare a list of child folders*/
            List<string> listChildFolders = new List<string>();

            /*1st Step: Starts the process by entering every each subfolder to the list of the Discovered and collected child folders*/
            listChildFolders = fc.collectChildFolders();

            Console.WriteLine("Checked!");
            Thread.Sleep(50);
            Console.WriteLine("Checking for .mp3 files (2/3)");
            Thread.Sleep(1000);

            /*2nd Step: Selects the .mp3 files from the discovered child folders and adds them on the list
             *Randomly selects one of the child folders
             */
            foreach (var childFolder in listChildFolders)
            {
                //If the current folder contains an .MP3 files
                if (Directory.GetFiles(childFolder, "*.mp3").Length != 0)
                {
                    //While there's a .mp3 files
                    //Adds them to the list of the selected .mp3 files
                    foreach (var mp3File in Directory.GetFiles(childFolder))
                    {
                        newMP3Files.Add(mp3File);
                    }
                }
            }

            Console.WriteLine("Checked!");
            Thread.Sleep(500);
            Console.WriteLine("Extracting all The .mp3 files (3/3)" +
                "\nProcessing to extract all the .mp3 files");
            Thread.Sleep(1000);

            Console.Clear();
            /*3rd Step: Starts the process for this list by moving all the .mp3 files to the main directory
             *Moves all the .mp3 files to the main directory
             */
            foreach (var mp3File in newMP3Files)
            {
                Console.SetCursorPosition(0, 0);
                string mp3Filename = Path.GetFileName(mp3File);
                Console.WriteLine("Moving the filename named -> " + mp3Filename);

                fm.MoveFile(mp3File, mainDirectoryPath);
                Thread.Sleep(50);
            }

            Console.WriteLine("All the .mp3 files has ben extracted successfully!" +
                                "\nWould you like to delete all the subfolders? (Type Y if you want)");

            /*
             * If the User Pressed Y button, then Delete all the child folders
             */
            if (Prog.isYes())
            {
                Console.WriteLine("Deleting the subfolders");
                Thread.Sleep(500);

                foreach (var folder in listChildFolders)
                {
                    fm.DeleteFile(folder);
                }

                Console.WriteLine("All the folders has been deleted!");
            }

            listChildFolders = null;
            newMP3Files = null;

            refreshList();
        }

        /// <summary>
        /// This Function is used to Delete the .mp3 file
        /// </summary>
        /// <param name="index">The index of the selected .mp3 file</param>
        public void deleteMP3File(int index)
        {
            fm.DeleteFile(mp3Files.ElementAt(index - 1));
        }

        /// <summary>
        /// This Function is used to get the numbers of .mp3 file in this main directory
        /// </summary>
        /// <returns>Returns 0 if there's no .mp3 file in the main directory</returns>
        public int getMP3FilesSize()
        {
            int length;
            try
            {
                length = mp3Files.Count;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
            return mp3Files.Count;
        }

        /// <summary>
        /// This Function is used to refresh the list of .mp3 files
        /// </summary>
        public void refreshList()
        {
            mp3Files = new List<string>();
            var arr = Directory.GetFiles(mainDirectoryPath, "*.mp3");

            foreach (var index in arr)
            {
                mp3Files.Add(index);
            }

            arr = null;

            fc.setDirectory(mainDirectoryPath);
        }

        /// <summary>
        /// This Function is used to check if there's a child folders in this main directory
        /// </summary>
        /// <returns>Returns false if there's no child folders in this main directory</returns>
        public bool checkFolders()
        {
            return fc.checkSubFolders();
        }

        /// <summary>
        /// This function is used to Play a specific .MP3 File using Index
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="FileNotFoundException">Thrown if the File is not found (depending by the index)</exception>
        public void PlayMP3File(int index)
        {
            if (!File.Exists(mp3Files.ElementAt(index - 1)))
            {
                throw new FileNotFoundException("File not found");
            }

            File.OpenRead(mp3Files.ElementAt(index - 1));
        }

        /// <summary>
        /// This function is used to get the informations about this mp3 file using the index
        /// </summary>
        /// 
        /// <param name="index">used to get a specific .MP3 File</param>
        ///
        /// <returns>Returns an information about his .MP3 File</returns>
        /// 
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="TagLib.CorruptFileException"></exception>
        /// <exception cref="TagLib.UnsupportedFormatException"></exception>
        public string GetMP3FileDetails(int index)
        {
            if (validateMP3File(index) == FILE_EXISTS)
            {
                var mp3File = TagLib.File.Create(mp3Files.ElementAt(index - 1));
                string MP3Details = "";

                MP3Details += "MP3 Filename --> " + Path.GetFileName(mp3Files.ElementAt(index - 1)) + '\n';
                MP3Details += "-------------------------------------------------------------------------\n";
                MP3Details += "Title: " + mp3File.Tag.Title + "\n";
                MP3Details += "Artist: " + mp3File.Tag.Artists[0] + "\n";
                MP3Details += "Album: " + mp3File.Tag.Album + "\n";

                if (mp3File.Tag.Year == 0)
                {
                    MP3Details += "Year: " + "N/A" + "\n";
                }
                else
                {
                    MP3Details += "Year: " + Convert.ToString(mp3File.Tag.Year) + "\n";
                }

                if (string.IsNullOrEmpty(mp3File.Tag.Genres[0]) || string.IsNullOrWhiteSpace(mp3File.Tag.Genres[0]))
                {
                    MP3Details += "Genre: " + "N/A" + "\n";
                }
                else
                {
                    MP3Details += "Genre: " + mp3File.Tag.Genres[0] + "\n";
                }

                return MP3Details;
            } else if (validateMP3File(index) == INDEX_OUT_IT_RANGE)
            {
                throw new IndexOutOfRangeException();
            } else if (validateMP3File(index) == INDEX_IS_EMPTY)
            {
                throw new FormatException();
            }

            return null;
        }

        /// <summary>
        /// This function is used to get a sorted List of artists in a specific directory
        /// </summary>
        /// <returns>Returns a list of distincted Artists with their index, and null if there's no artists in the specific directory</returns>
        public string getMP3ArtistFiles()
        {
            if (mp3Files.Count > 0)
            {
                List<string> List_MP3Artists = new List<string>();

                foreach (var MP3File in mp3Files)
                {
                    var MP3Info = TagLib.File.Create(MP3File);
                    List_MP3Artists.Add(MP3Info.Tag.Artists[0]);
                    MP3Info = null;
                }

                List_MP3Artists.Sort();

                List<string> list_MP3Artists_noDupes = DistinctList(List_MP3Artists);

                string MP3Artists = "";

                for (int mp3FileList_index = 0; mp3FileList_index < list_MP3Artists_noDupes.Count; mp3FileList_index++)
                {
                    int pointer = mp3FileList_index + 1;
                    MP3Artists += Convert.ToString(pointer) + "- " + list_MP3Artists_noDupes[mp3FileList_index] + (pointer == list_MP3Artists_noDupes.Count? "" : "\n");
                }

                return MP3Artists;
            }

            return null;
        }

        /// <summary>
        /// This function is used to get a sorted List of Albums in a specific directory
        /// </summary>
        /// <returns>Returns a list of distincted Albums with their index, and null if there's no Albums in the specific directory</returns>
        public string getMP3AlbumFiles()
        {
            if (mp3Files.Count > 0)
            {
                List<string> List_MP3Albums = new List<string>();

                foreach(var MP3File in mp3Files)
                {
                    var MP3Info = TagLib.File.Create(MP3File);
                    List_MP3Albums.Add(MP3Info.Tag.Album + "  <--  Artist: " + MP3Info.Tag.Artists[0]);
                    MP3Info = null;
                }

                List_MP3Albums.Sort();

                List<string> list_MP3Albums_noDupes = DistinctList(List_MP3Albums);

                string MP3Albums = "";

                for (int mp3FileList_index = 0; mp3FileList_index < list_MP3Albums_noDupes.Count; mp3FileList_index++)
                {
                    int pointer = mp3FileList_index + 1;
                    MP3Albums += Convert.ToString(pointer) + "- " + list_MP3Albums_noDupes[mp3FileList_index] + (pointer == list_MP3Albums_noDupes.Count ? "" : "\n");
                }

                return MP3Albums;
            }

            return null;
        }

        /// <summary>
        /// This function is used to get sorted List of Years from Oldest to newest (include Unknown Year which can returned as N/A) in a specific directory
        /// </summary>
        /// <returns>Returns a list of distincted Years with their index, and null if there's no MP3 Files in the specific directory</returns>
        public string getMP3YearFiles()
        {
            if (mp3Files.Count > 0)
            {
                List<string> List_MP3Years = new List<string>();

                foreach (var MP3File in mp3Files)
                {
                    var MP3Info = TagLib.File.Create(MP3File);
                    if (MP3Info.Tag.Year == 0)
                    {
                        List_MP3Years.Add(Convert.ToString(0));
                    } else
                    {
                        List_MP3Years.Add(Convert.ToString(MP3Info.Tag.Year));
                    }
                    MP3Info = null;
                }

                List_MP3Years.Sort();

                List<string> list_MP3Years_noDupes = DistinctList(List_MP3Years);

                string MP3Years = "";
                
                for (int mp3FileList_index = 0; mp3FileList_index < list_MP3Years_noDupes.Count; mp3FileList_index++)
                {
                    int pointer = mp3FileList_index + 1;
                    MP3Years += Convert.ToString(pointer) + "- " + list_MP3Years_noDupes[mp3FileList_index] + (pointer == list_MP3Years_noDupes.Count? "" : "\n");
                }

                return MP3Years;
            }

            return null;
        }

        /// <summary>
        /// This function is used to Validate a selected Artist (depending on the index) 
        /// </summary>
        /// <param name="index">the index of specific Artist</param>
        /// <returns>
        /// It returns the following:       <br/>
        ///     1- INDEX_VALID ->           <br/>
        ///     2- INDEX_NOT_INTEGER ->     <br/>
        ///     3- INDEX_IS_EMPTY ->        <br/>
        ///     4- INDEX_OUT_OF_RANGE ->    <br/>
        /// </returns>
        public int ValidateArtist(Object index)
        {
            int number = 0;

            try
            {
                number = (int) index;
            } catch
            {
                return INDEX_NOT_INTEGER;
            }

            if (number == 0)
            {
                return INDEX_IS_EMPTY;
            } else
            {
                Dictionary<int, string> mp3Artists = initArtistList();

                try
                {
                    if (mp3Artists.ContainsKey(number))
                    {
                        return INDEX_VALID;
                    }
                } catch (ArgumentOutOfRangeException)
                {
                    return INDEX_OUT_IT_RANGE;
                } 
            }

            return 0;
        }

        public List<string> DistinctList(List<string> list)
        {
            HashSet<string> noDupes = list.ToHashSet(StringComparer.OrdinalIgnoreCase);
            return noDupes.ToList();
        }

        public int ValidateAlbum(Object index)
        {
            int number = 0;

            try
            {
                number = (int) index;
            } catch
            {
                return INDEX_NOT_INTEGER;
            }

            if (number == 0)
            {
                return INDEX_IS_EMPTY;
            } else
            {
                Dictionary<int, string> mp3Albums = initAlbumList();

                try
                {
                    if (mp3Albums.ContainsKey(number))
                    {
                        return INDEX_VALID;
                    }
                } catch (ArgumentOutOfRangeException)
                {
                    return INDEX_OUT_IT_RANGE;
                }
            }

            return 0;
        }

        public int ValidateYear(Object index)
        {
            int number = 0;

            try
            {
                number = (int)index;
            } catch (InvalidCastException)
            {
                return INDEX_NOT_INTEGER;
            }

            if (number == 0)
            {
                return INDEX_IS_EMPTY;
            } else
            {
                Dictionary<int, string> MP3Years = initYearList();

                try
                {
                    if (MP3Years.ContainsKey(number))
                    {
                        return INDEX_VALID;
                    }
                } catch (ArgumentOutOfRangeException)
                {
                    return INDEX_OUT_IT_RANGE;
                }
            }

            return 0;
        }

        private Dictionary<int, string> initArtistList()
        {
            List<string> mp3Artists = new List<string>();

            foreach (var mp3File in mp3Files)
            {
                var mp3 = TagLib.File.Create(mp3File);
                mp3Artists.Add(mp3.Tag.Artists[0]);
                mp3 = null;
            }

            mp3Artists.Sort();

            List<string> mp3Artists_noDupe = DistinctList(mp3Artists);
            mp3Artists = null;

            int i = 0;
            Dictionary<int, string> hashMP3Artists = new Dictionary<int, string>();

            foreach (var mp3Artist in mp3Artists_noDupe)
            {
                i++;
                hashMP3Artists.Add(i, mp3Artist);
            }

            return hashMP3Artists;
        }

        private Dictionary<int, string> initAlbumList()
        {
            List<string> MP3AlbumList = new List<string>();

            foreach (var MP3File in mp3Files)
            {
                var mp3 = TagLib.File.Create(MP3File);
                MP3AlbumList.Add(mp3.Tag.Album);
                mp3 = null;
            }

            MP3AlbumList.Sort();

            List<string> MP3AlbumList_noDupes = DistinctList(MP3AlbumList);

            int i = 0;
            Dictionary<int, string> HashAlbumList = new Dictionary<int, string>();

            foreach (var MP3Album in MP3AlbumList_noDupes)
            {
                i++;
                HashAlbumList.Add(i, MP3Album);
            }

            return HashAlbumList;
        }

        private Dictionary<int, string> initYearList()
        {
            List<string> MP3YearsList = new List<string>();

            foreach (var MP3File in mp3Files)
            {
                var mp3 = TagLib.File.Create(MP3File);
                if (mp3.Tag.Year == 0)
                {
                    MP3YearsList.Add("N/A");
                } else
                {
                    MP3YearsList.Add(Convert.ToString(mp3.Tag.Year));
                }
            }

            MP3YearsList.Sort();

            List<string> MP3YearsList_noDupes = DistinctList(MP3YearsList);
            MP3YearsList = null;

            int i = 0;
            Dictionary<int, string> HashYearsList = new Dictionary<int, string>();

            foreach(var MP3Year in MP3YearsList_noDupes)
            {
                i++;
                HashYearsList.Add(i, MP3Year);
            }

            return HashYearsList;
        }

        /// <summary>
        /// if we want to rename the .mp3 file without errors that's comes from the File Explorer due of the limited characters that's it's illegal to put them.
        /// This Function is used to remove some symbols that's illegal to put them to the filename
        /// </summary>
        /// <param name="text">A sample text we want to check if there's an illegal symbols we want to remove them</param>
        /// <returns>Return an edited text with removed illegal symbols</returns>
        private string removeSymbols(string text)
        {
            Queue<char> queue_text = new Queue<char>();

            for (int i = 0; i < text.Length; i++)
            {
                if (symbolsCheck(text[i]))
                {
                    continue;
                }

                queue_text.Enqueue(text[i]);
            }

            text = "";

            while (queue_text.Count != 0)
            {
                text += queue_text.Dequeue();
            }

            return text;
        }

        /// <summary>
        /// for each letter in the text variable, we want to check if this character is an illegal symbol
        /// </summary>
        /// <param name="sym">Sample character</param>
        /// <returns>Returns true if it's a part of illegal symbols</returns>
        private bool symbolsCheck(char sym)
        {
            return sym == '?' || sym == '/' || sym == '\\' || sym == '<' || sym == '>' || sym == '|' || sym == '*' || sym == '\"'
                || sym == ':' || sym == '\n';
        }

        /// <summary>
        /// This Function is used to Edit the .mp3 file (The album value is Optional to edit it)
        /// </summary>
        /// <param name="artist">The value of the artist</param>
        /// <param name="title">The value of the title</param>
        /// <param name="album">The value of the album (Optional)</param>
        /// <param name="index">The selected .mp3 file</param>
        private void editMP3(string artist, string title, string album, int index)
        {
            var mp3 = TagLib.File.Create(mp3Files.ElementAt(index));

            mp3.Tag.Artists[0] = artist;
            mp3.Tag.Title = title;
            if (!string.IsNullOrEmpty(album))
            {
                mp3.Tag.Album = album;
            }

            mp3.Save();
        }
    }
}