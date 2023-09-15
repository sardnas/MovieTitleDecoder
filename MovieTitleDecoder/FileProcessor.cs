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
        _writeAndReadDirectory.deleteTemporaryDirectory();
    }

    private void changeFileNames(DirectoryInfo movieDirectory, string correctTitle) 
    {

    }
    private void changeDirectoryNames(string correctTitle) 
    {
        Directory.CreateDirectory(movieDirectoryPath + "/" + correctTitle);
    }
    private string FindMovieTitle(DirectoryInfo movieDirectory) 
    {
        //[a-zA-Z]+ ord
        //\b(19|20)\d{2}\b år
        string wordRegEx = @"[a-zA-Z]+";
        string yearRegEx = @"\b(19 | 20)\d{ 2}\b";
        List<string> wordsInName = new List<string>();
        string yearInName;
        try
        {
            Match words = Regex.Match(movieDirectory.Name, wordRegEx);
            Match year = Regex.Match(movieDirectory.Name, yearRegEx);
            while (words.Success)
            {
                wordsInName.Add(words.Value);

                words = words.NextMatch();
            }

            yearInName = year.Value;
            handleWordsInTitle(wordsInName);
        }
        catch (RegexMatchTimeoutException)
        {
            // Do nothing: assume that exception represents no match.
        }
        return ""; 
    }

    private string handleWordsInTitle(List<string> matches) 
    { 
        // regler
        // om det är en ensam bokstav måste den vara A eller a eller I eller i
        return ""; 
    }

    // hämta lista med mappar eller filer i en mapp
    // skapa en kopia
    // loopa genom mapparna och fixa namnet
    // gå in i mappen och sortera där i

    // profit
}
