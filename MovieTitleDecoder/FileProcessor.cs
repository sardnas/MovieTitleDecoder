using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MovieTitleDecoder.DocumentRepository;
using MovieTitleDecoder.Interfaces;

namespace MovieTitleDecoder;

public class FileProcessor
{
    private readonly IWriteAndReadDirectory _writeAndReadDirectory;
    private string movieDirectoryPath;

    public FileProcessor(IWriteAndReadDirectory writeAndReadDirectory) 
    {
        _writeAndReadDirectory = writeAndReadDirectory;
        movieDirectoryPath = _writeAndReadDirectory.mediaDirectoryPath + "/Movies";
        processFiles();
    }

    private void processFiles() 
    {
        DirectoryInfo[] movieDirectories = _writeAndReadDirectory.GetMovieFiles();
        Directory.CreateDirectory(movieDirectoryPath);
        string correctTitle;
        foreach (var movieDirectory in movieDirectories) 
        {
            correctTitle = FindMovieTitle(movieDirectory);
            changeDirectoryNames(correctTitle);
            changeFileNames(movieDirectory, correctTitle);
        }
        //_writeAndReadDirectory.deleteTemporaryDirectory();
    }

    private void changeFileNames(DirectoryInfo movieDirectory, string correctTitle) 
    {
        FileInfo[] files = movieDirectory.GetFiles();
        foreach (var file in files)
        {
            if (file.Extension == ".mp4" || file.Extension == ".mkv") 
            {
                file.MoveTo(movieDirectoryPath + "/" + correctTitle + "/" + correctTitle + file.Extension);
            }
            if (file.Extension == ".srt")
            {
                file.MoveTo(movieDirectoryPath + "/" + correctTitle + "/" + correctTitle + ".en" + file.Extension);
            }
        }
        
    }
    private void changeDirectoryNames(string correctTitle) 
    {
        Directory.CreateDirectory(movieDirectoryPath + "/" + correctTitle);
    }
    private string FindMovieTitle(DirectoryInfo movieDirectory) 
    {
        //[a-zA-Z]+ ord
        //\b(19|20)\d{2}\b år
        string wordRegEx = @"\b(?![A-Z]{2,}[a-z]*\d*\b)[a-zA-Z]+";//@"[a-zA-Z]+";
        string yearRegEx = @"\b(19|20)\d{2}\b";
        List<string> wordsInName = new List<string>();
        string yearInName = "0";
        try
        {
            Match words = Regex.Match(movieDirectory.Name, wordRegEx);
            Match year = Regex.Match(movieDirectory.Name, yearRegEx);
            while (words.Success)
            {
                wordsInName.Add(words.Value);

                words = words.NextMatch();
            }
            while (year.Success) 
            {
                yearInName = year.Value;
                year = year.NextMatch();
            }
            
        }
        catch (RegexMatchTimeoutException)
        {
            return "";
        }
        
        StringBuilder title = handleWordsInTitle(wordsInName);
        title.Append("(");
        title.Append(yearInName);
        title.Append(")");

        return title.ToString(); 
    }

    private StringBuilder handleWordsInTitle(List<string> matches) 
    { 
        StringBuilder sb = new StringBuilder();
        foreach (string word in matches)
        {
            if (word.Length > 1 || word == "A" || word == "a" || word == "I" || word == "i")
            {
                sb.Append(word);
                sb.Append(' ');
            }            
        }

        return sb; 
    }

}
