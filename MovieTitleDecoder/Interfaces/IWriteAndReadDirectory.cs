using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTitleDecoder.Interfaces;

public interface IWriteAndReadDirectory
{
    public string tmpMediaDirectoryPath { get; set; }
    public string mediaDirectoryPath { get; }
    public DirectoryInfo[] GetMovieFiles();
    public FileInfo[] GetTVShowFiles();
    public void deleteTemporaryDirectory();
    //getfiles och returnera alla filer som en lista (kopia)
}
