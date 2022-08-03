using System.IO;

namespace MP3ManagerApplication
{
    public class FileManager
    {
        public FileManager()
        {

        }

        /// <summary>
        /// This Function is used to move the file without the file Loss
        /// </summary>
        /// <param name="sourceFile">The source file</param>
        /// <param name="directoryPath">The directory path that need to be moved for</param>
        public void MoveFile(string sourceFile, string directoryPath)
        {
            string fileType = Path.GetExtension(sourceFile);
            string fileName = Path.GetFileNameWithoutExtension(sourceFile);

            if (File.Exists(directoryPath + "\\" + fileName + fileType))
            {
                if (File.Exists(directoryPath + "\\" + fileName + " - Copy" + fileType))
                {
                    for (int i = 2; i <= int.MaxValue; i++)
                    {
                        if (!File.Exists(directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType))
                        {
                            File.Move(sourceFile, directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType);
                            break;
                        }
                    }
                }
                else
                    File.Move(sourceFile, directoryPath + "\\" + fileName + " - Copy" + fileType);
            }
            else
                File.Move(sourceFile, directoryPath + "\\" + fileName + fileType);
        }

        /// <summary>
        /// This Function is used to delete a file Path
        /// </summary>
        /// <param name="filePath">the selected File Path</param>
        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// This Function is used to rename the current filename 
        /// </summary>
        /// <param name="filePath">The path of the selected file</param>
        /// <param name="renamedFileName">The renamed File name</param>
        public void RenameFile(string filePath, string renamedFileName)
        {
            string fileType = Path.GetExtension(renamedFileName);
            string fileName = Path.GetFileNameWithoutExtension(renamedFileName);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (File.Exists(directoryPath + "\\" + fileName + fileType))
            {
                if (File.Exists(directoryPath + "\\" + fileName + " - Copy" + fileType))
                {
                    for (int i = 2; i <= int.MaxValue; i++)
                    {
                        if (!File.Exists(directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType))
                        {
                            File.Move(filePath, directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType);
                            break;
                        }
                    }
                }
                else
                    File.Move(filePath, directoryPath + "\\" + fileName + " - Copy" + fileType);
            }
            else
            {
                File.Move(filePath, directoryPath + "\\" + fileName + fileType);
            }
        }

        public void RenameAndMoveFile(string sourceFile, string destFile)
        {
            if (File.Exists(sourceFile))
            {
                string fileName = Path.GetFileNameWithoutExtension(destFile);
                string fileType = Path.GetExtension(destFile);
                string directoryPath = Path.GetDirectoryName(destFile);

                if (File.Exists(destFile))
                {
                    if (File.Exists(directoryPath + "\\" + fileName + " - Copy" + fileType))
                    {
                        for (int i = 2; i <= int.MaxValue; i++)
                        {
                            if (!File.Exists(directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType))
                            {
                                File.Move(sourceFile, directoryPath + "\\" + fileName + " - Copy (" + i + ")" + fileType);
                                break;
                            }
                        }
                    }
                    else
                        File.Move(sourceFile, directoryPath + "\\" + fileName + " - Copy" + fileType);
                }
                else
                    File.Move(sourceFile, destFile);
            }
        }
    }
}
