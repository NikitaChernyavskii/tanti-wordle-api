using AutoFixture;
using NSubstitute;
using Wordle.Services.Contracts.Models;
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
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsService.GetRandomWordAsync(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not exist."));
    }

    [Test]
    public void GetRandomWord_WhenFileIsEmpty_ThenThrowArgumentException()
    {
        // Arrange
        var wordLenght = 1000; // existing empty file for testing
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Act && Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _wordsService.GetRandomWordAsync(wordLenght));
        Assert.That(exception.Message, Is.EqualTo($"File with {wordLenght} lenght does not have words."));
    }

    [Test]
    public async Task GetRandomWord_WhenFileIsValid_ThenReturnRandomWord()
    {
        // Arrange
        var wordLenght = 1001; // existing empty file for testing
        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);

        // Act
        var result = await _wordsService.GetRandomWordAsync(wordLenght);

        // Assert
        Assert.That(result, Is.EqualTo("Test"));
    }

    [Test]
    public void GetWordValidation_WhenWordsAreEqual_ReturnAllMatchValidation()
    {
        // Arrange
        var wordToValdiate = _autoFixture.Create<string>();
        var targetWord = wordToValdiate;

        // Act
        var result = _wordsService.GetWordValidation(wordToValdiate, targetWord);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CharacterValidations.All(x => x.Status == CharacterValidaionStatus.Matches), Is.True);
    }

    [Test]
    public void GetWordValidation_WhenWordsAreDifferent_ReturnActualValidation()
    {
        // Arrange
        var wordToValdiate = "АААс";
        var targetWord = "cАБССВа";

        // Act
        var result = _wordsService.GetWordValidation(wordToValdiate, targetWord);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.Matches).Count(), Is.EqualTo(2));
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.Exists).Count(), Is.EqualTo(1));
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.NotExists).Count(), Is.EqualTo(1));
    }
}
