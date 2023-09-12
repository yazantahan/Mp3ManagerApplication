using MP3ManagerApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace MP3ManagerApplication.Tests
{
    [TestClass()]
    public class MP3EngineTests
    {
        [TestMethod()]
        public void DistinctListTest()
        {
            MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs");
            List<string> list = new List<string>();

            list.Add("Dagoba");
            list.Add("Dagoba");
            list.Add("Dagoba");
            list.Add("As I lay dying");
            list.Add("As I lay dying");
            list.Add("As I lay dying");
            list.Add("As I lay dying");
            list.Add("Lord of the Lost");
            list.Add("Lord of the Lost");

            List<string> distinctedlist = mp3Engine.DistinctList(list);

            foreach (var element in distinctedlist)
            {
                Console.WriteLine(element);
            }

            Assert.AreEqual(3, distinctedlist.Count);

            list = new List<string>();

            list.Add("Dagoba");
            list.Add("As I lay dying");
            list.Add("Lord of the Lost");

            distinctedlist = mp3Engine.DistinctList(list);

            foreach (var element in distinctedlist)
            {
                Console.WriteLine(element);
            }

            Assert.AreEqual(3, distinctedlist.Count);
        }

        [TestMethod()]
        public void getMP3ArtistFilesTest()
        {
            MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs");

            string mp3ListArtists;

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dark Tranquillity");

            mp3ListArtists = mp3Engine.getMP3ArtistFiles();

            Console.WriteLine(mp3ListArtists);
            if (!string.IsNullOrEmpty(mp3ListArtists))
            {
                Assert.AreEqual(1, mp3ListArtists.Split('\n').Length);
            }
        }

        [TestMethod()]
        public void getMP3AlbumFilesTest()
        {
            MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Celtic Frost");

            string mp3ListAlbums = mp3Engine.getMP3AlbumFiles();

            Console.WriteLine(mp3ListAlbums);
            Assert.AreEqual(5, mp3ListAlbums.Split('\n').Length);

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dark Tranquillity");

            mp3ListAlbums = mp3Engine.getMP3AlbumFiles();

            Console.WriteLine(mp3ListAlbums);
            Assert.AreEqual(4, mp3ListAlbums.Split('\n').Length);
        }

        [TestMethod()]
        public void GetMP3FileDetailsTest()
        {
            MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Celtic Frost");

            string musicDetails = mp3Engine.GetMP3FileDetails(2);

            if (!string.IsNullOrEmpty(musicDetails))
            {
                Console.WriteLine(musicDetails);
            }
            else
            {
                Console.WriteLine("N/A");
            }

            Assert.IsFalse(string.IsNullOrEmpty(musicDetails));

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Annihilator");

            musicDetails = mp3Engine.GetMP3FileDetails(2);

            if (!string.IsNullOrEmpty(musicDetails))
            {
                Console.WriteLine(musicDetails);
            }
            else
            {
                Console.WriteLine("N/A");
            }

            Assert.IsFalse(string.IsNullOrEmpty(musicDetails));
        }

        [TestMethod()]
        public void getMP3YearFilesTest()
        {
            MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead Sun");

            string MP3ListYear = mp3Engine.getMP3YearFiles();

            if (!string.IsNullOrEmpty(MP3ListYear))
            {
                Console.WriteLine(MP3ListYear);
            }
            else
            {
                Console.WriteLine("N/A");
            }

            Assert.AreEqual(2, MP3ListYear.Split('\n').Length);
        }

        //[TestMethod()]
        //public void initYearListTest()
        //{
        //    MP3Engine mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Annihilator");
        //
        //    Dictionary<int, string> mp3Year_List = mp3Engine.initYearList();
        //
        //    foreach (var mp3Year in mp3Year_List)
        //    {
        //        Console.WriteLine(mp3Year.Key + "- " + mp3Year.Value);
        //    }
        //
        //    Assert.AreEqual(5, mp3Year_List.Count);
        //}
    }
}