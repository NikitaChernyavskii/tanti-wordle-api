using AutoFixture;
using NSubstitute;
using Wordle.Services.Words;
using Wordle.Services.Words.Validators;

namespace Wordle.Services.Tests.Words;

[TestFixture]
public class WordsServiceTests
{
    private IWordsServiceValidator _wordsServiceValidator;
    private WordsService _wordsService;

    private Fixture _autoFixture;

    [SetUp]
    public void SetUp()
    {
        _wordsServiceValidator = Substitute.For<IWordsServiceValidator>();
        _wordsService = new WordsService(_wordsServiceValidator);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _autoFixture = new Fixture();
    }

    [Test]
    public void GetRandomWord_WhenFileNotExists_ThenThrowArgumentException()
    {
        // Arrange
        var wordLenght = 123;
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Act && Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsService.GetRandomWord(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not exist."));
    }

    [Test]
    public void GetRandomWord_WhenFileIsEmpty_ThenThrowArgumentException()
    {
        // Arrange
        var wordLenght = 1000; // existing empty file for testing
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Act && Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsService.GetRandomWord(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not have words."));
    }

    [Test]
    public async Task GetRandomWord_WhenFileIsValid_ThenReturnRandomWord()
    {
        // Arrange
        var wordLenght = 1001; // existing empty file for testing
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Act
        var result = await _wordsService.GetRandomWord(wordLenght);

        // Assert
        Assert.That(result, Is.EqualTo("Test"));
    }
}
