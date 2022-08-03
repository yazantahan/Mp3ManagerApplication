using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MP3ManagerApplication.Tests
{
    [TestClass()]
    public class FileManagerTests
    {
        [TestMethod()]
        public void moveFileTest()
        {
            FileManager fm = new FileManager();

            fm.renameFile(@"C:\Users\yazan\OneDrive\Desktop\MP3FilesTest\Carcass - Intensive Battery Brooding.mp3", "Carcass - Intensive Battery Brooding.mp3");

        }
    }
}