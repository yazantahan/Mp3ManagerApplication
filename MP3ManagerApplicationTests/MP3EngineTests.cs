using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MP3ManagerApplication.Tests
{
    [TestClass()]
    public class MP3EngineTests
    {
        MP3Engine mp3Engine;
        [TestMethod()]
        public void getMP3FilesSizeTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dark Tranquillity");
            Assert.AreEqual(15, mp3Engine.getMP3FilesSize());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead Sun");
            Assert.AreEqual(9, mp3Engine.getMP3FilesSize());

            mp3Engine.changeDirectoryPath(@"D:\Music\Jesper Kyd");
            Assert.AreEqual(1, mp3Engine.getMP3FilesSize());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual(10, mp3Engine.getMP3FilesSize());
        }

        [TestMethod()]
        public void validateMP3FileTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual(MP3Engine.FILE_EXISTS, mp3Engine.validateMP3File("1"));
            Assert.AreNotEqual(MP3Engine.INDEX_OUT_IT_RANGE, mp3Engine.validateMP3File(6));
            Assert.AreEqual(MP3Engine.INDEX_OUT_IT_RANGE, mp3Engine.validateMP3File(11));
            Assert.AreEqual(MP3Engine.INDEX_NOT_INTEGER, mp3Engine.validateMP3File(""));
            Assert.AreEqual(MP3Engine.INDEX_IS_EMPTY, mp3Engine.validateMP3File("0"));
            Assert.AreEqual(MP3Engine.INDEX_NOT_INTEGER, mp3Engine.validateMP3File("sijdvnidvn"));
        }

        [TestMethod()]
        public void getArtistFieldTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual("dead by april", mp3Engine.getArtistField(1).ToLower());

            mp3Engine.changeDirectoryPath(@"D:\Music\ISIS");
            Assert.AreEqual("isis", mp3Engine.getArtistField(1).ToLower());
        }

        [TestMethod()]
        public void getTitleFieldTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual("beautiful nightmare", mp3Engine.getTitleField(0).ToLower());
            Assert.AreEqual("Perfect The Way You Are".ToLower(), mp3Engine.getTitleField(7).ToLower());

            mp3Engine.changeDirectoryPath(@"D:\Music\ISIS");
            Assert.AreEqual("carry", mp3Engine.getTitleField(0).ToLower());
        }

        [TestMethod()]
        public void getArtistFromFileNameTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual("Dead by april".ToLower(), mp3Engine.getArtistFromFileName(1).ToLower());
            Assert.AreEqual("Dead by april".ToLower(), mp3Engine.getArtistFromFileName(10).ToLower());

            mp3Engine.changeDirectoryPath(@"D:\Music\ISIS");
            Assert.AreEqual("ISIS".ToLower(), mp3Engine.getArtistFromFileName(1).ToLower());

            mp3Engine.changeDirectoryPath(@"D:\Music\Ad Infinitum");
            Assert.AreEqual("Ad Infinitum".ToLower(), mp3Engine.getArtistFromFileName(1).ToLower());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs");
            Assert.AreEqual("dead by april".ToLower(), mp3Engine.getArtistFromFileName(1).ToLower());
            Assert.AreEqual("", mp3Engine.getArtistFromFileName(2).ToLower());
        }

        [TestMethod()]
        public void getTitleFromFileNameTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            Assert.AreEqual("beautiful nightmare".ToLower(), mp3Engine.getTitleFromFileName(0).ToLower());
            Assert.AreEqual("erased".ToLower(), mp3Engine.getTitleFromFileName(9).ToLower());

            mp3Engine.changeDirectoryPath(@"D:\Music\ISIS");
            Assert.AreEqual("carry".ToLower(), mp3Engine.getTitleFromFileName(0).ToLower());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs");
            Assert.AreEqual("I Can't Breathe".ToLower(), mp3Engine.getTitleFromFileName(0).ToLower());
            Assert.AreEqual("", mp3Engine.getTitleFromFileName(1).ToLower());
        }

        [TestMethod()]
        public void getFileNamePath()
        {
            string artist, title;
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Dead By April");
            artist = mp3Engine.getArtistField(6);
            title = mp3Engine.getTitleField(6);

            Assert.AreEqual(MP3Engine.FILE_EXISTS, mp3Engine.validateMP3File(6));
            Assert.AreEqual("Dead By april".ToLower(), artist.ToLower());
            Assert.AreEqual("losing you".ToLower(), title.ToLower());

            artist = mp3Engine.getArtistField(11);
            title = mp3Engine.getTitleField(11);

            Assert.AreEqual(MP3Engine.INDEX_OUT_IT_RANGE, mp3Engine.validateMP3File(11));
            Assert.AreEqual("".ToLower(), artist.ToLower());
            Assert.AreEqual("".ToLower(), title.ToLower());
        }

        [TestMethod()]
        public void renameMP3FileNameTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\MP3FilesTest");

            mp3Engine.renameMP3FileName("Dead By April", "I Can't breathe", 1, true);
            mp3Engine.renameMP3FileName("Dead sun", "Dead sun", 2, true);

            mp3Engine.refreshList();
            Assert.AreEqual(0, mp3Engine.getMP3FilesSize());
        }

        [TestMethod()]
        public void extractMP3FilesTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\MP3FilesTest");
            mp3Engine.extractMP3Files();

            Assert.AreEqual(5, mp3Engine.getMP3FilesSize());
        }

        [TestMethod()]
        public void extractAndOrganizeTest()
        {
            mp3Engine = MP3Engine.setDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs");
            mp3Engine.extractMP3Files();

            Assert.AreEqual(21, mp3Engine.getMP3FilesSize());

            var nonFormattable = new List<string>();
            for (int i = 1; i <= mp3Engine.getMP3FilesSize(); i++)
            {
                string artist = mp3Engine.getArtistFromFileName(i);
                string title = mp3Engine.getTitleFromFileName(i);

                if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(title))
                {
                    mp3Engine.editMP3Info(artist, title, i);
                    mp3Engine.moveToArtistFolder(artist, title, i);
                }
                else
                {
                    nonFormattable.Add(mp3Engine.getMP3FileName(i));
                }
            }

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Be'Lakor");
            Assert.AreEqual(4, mp3Engine.getMP3FilesSize());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\Carcass");
            Assert.AreEqual(14, mp3Engine.getMP3FilesSize());

            mp3Engine.changeDirectoryPath(@"C:\Users\yazan\OneDrive\Desktop\Downloaded Songs\ISIS");
            Assert.AreEqual(3, mp3Engine.getMP3FilesSize());

            Assert.AreEqual(0, nonFormattable.Count);
        }
    }
}