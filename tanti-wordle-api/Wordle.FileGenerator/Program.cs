using Wordle.FileGenerator.Services;

int lenght;
Console.WriteLine("Enter word lenght - integer");
if (int.TryParse(Console.ReadLine(), out lenght))
{
    var wordleFilesGenerator = new WordleFilesGenerator();
    var fileName = await wordleFilesGenerator.GenerateFileAsync(lenght);
    Console.WriteLine("File creation completed");
    Console.WriteLine(fileName);
} else
{
    Console.Write("Incorrect lenght");
}