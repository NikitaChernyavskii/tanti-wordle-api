using AutoFixture;
using Wordle.Repository.Words;

namespace Wordle.Repository.Tests.Words;
[TestFixture]
public class WordsRepositoryTests
{
    private WordsRepository _wordsRepository;

    private Fixture _autoFixture;

    [SetUp]
    public void SetUp()
    {
        _wordsRepository = new WordsRepository();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _autoFixture = new Fixture();
    }

    [Test]
    public void GetWordsFromFile_WhenFileNotExists_ThenThrowArgumentException()
    {
        // Arrange
        var wordLenght = 123;

        // Act && Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsRepository.GetWordsFromFile(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not exist."));
    }

    [Test]
    public void GetRandomWord_WhenFileIsEmpty_ThenThrowArgumentException()
    {
        // Arrange
        var wordLenght = 1000; // existing empty file for testing

        // Act && Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsRepository.GetWordsFromFile(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not have words."));
    }
}
