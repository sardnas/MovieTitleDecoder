// See https://aka.ms/new-console-template for more information

using MovieTitleDecoder;
using MovieTitleDecoder.DocumentRepository;

var fileProcessor = new FileProcessor(new WriteAndReadDirectory("test"));

Console.WriteLine("Hello, World!");
