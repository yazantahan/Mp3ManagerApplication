using System;
using System.Collections.Generic;
using System.IO;

namespace MP3ManagerApplication
{
    public class FolderCollector
    {
        private static FolderCollector fc;
        private string directoryPath;

        private FolderCollector(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public static FolderCollector init(string directoryPath)
        {
            if (fc == null)
            {
                fc = new FolderCollector(directoryPath);
            }

            return fc;
        }

        public List<string> collectChildFolders()
        {
            var childFolders = Directory.GetDirectories(directoryPath);

            Array.Sort(childFolders);

            Stack<string> stackChildFolders = new Stack<string>();

            foreach (var childFolder in childFolders)
            {
                stackChildFolders.Push(childFolder);
            }

            childFolders = null;

            List<string> listChildFolders = new List<string>();

            while (stackChildFolders.Count != 0)
            {
                if (Directory.GetDirectories(stackChildFolders.Peek()).Length == 0)
                {
                    listChildFolders.Add(stackChildFolders.Pop());
                }
                else
                {
                    childFolders = Directory.GetDirectories(stackChildFolders.Peek());

                    listChildFolders.Add(stackChildFolders.Pop());

                    foreach (var childFolder in childFolders)
                    {
                        stackChildFolders.Push(childFolder);
                    }
                }
            }

            return listChildFolders;
        }

        public bool checkSubFolders()
        {
            return Directory.GetDirectories(directoryPath).Length != 0;
        }

        public void dispose()
        {
            directoryPath = null;
        }

        public void setDirectory(string directoryPath)
        {
            this.directoryPath = directoryPath;
            fc.directoryPath = directoryPath;
        }
    }
}
