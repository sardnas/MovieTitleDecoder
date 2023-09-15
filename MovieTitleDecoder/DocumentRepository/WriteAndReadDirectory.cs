using MovieTitleDecoder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTitleDecoder.DocumentRepository;

public class WriteAndReadDirectory : IWriteAndReadDirectory
{
    public string mediaDirectoryPath { get; }
    public string tmpMediaDirectoryPath { get; set; }

    public WriteAndReadDirectory(string path) 
    {
        mediaDirectoryPath = path;
        tmpMediaDirectoryPath = path + "/tmp";
    }
    public DirectoryInfo[] GetMovieFiles() 
    {
        DirectoryInfo[] movieDirectories = copyOfDirectory();
        return movieDirectories; 
    }
    public FileInfo[] GetTVShowFiles() { return new FileInfo[0]; }
    public void deleteTemporaryDirectory() { }

    private DirectoryInfo[] copyOfDirectory() 
    {
        CopyFilesRecursively(mediaDirectoryPath + "/New Movies", tmpMediaDirectoryPath);
        DirectoryInfo unprocessedMovieDirectory = new DirectoryInfo(tmpMediaDirectoryPath);
        DirectoryInfo[] directoryInfos = unprocessedMovieDirectory.GetDirectories();
        return directoryInfos; 
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }
    
    //getfiles och returnera alla filer som en lista (kopia)
}
