using AutoFixture;
using NSubstitute;
using Wordle.Services.Contracts.Models;
using Wordle.Services.Words;
using Wordle.Services.Contracts.Words.Validators;
using Wordle.Services.Contracts.Words.CacheDataProviders;

namespace Wordle.Services.Tests.Words;
[TestFixture]
public class WordsServiceTests
{
    private IWordsServiceValidator _wordsServiceValidator;
    private IWordsCacheDataProvider _wordsCacheDataProvider;
    private WordsService _wordsService;

    private Fixture _autoFixture;

    [SetUp]
    public void SetUp()
    {
        _wordsServiceValidator = Substitute.For<IWordsServiceValidator>();
        _wordsCacheDataProvider = Substitute.For<IWordsCacheDataProvider>();
        _wordsService = new WordsService(_wordsServiceValidator, _wordsCacheDataProvider);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _autoFixture = new Fixture();
    }

    [Test]
    public async Task GetRandomWord_WhenFileIsValid_ThenReturnRandomWord()
    {
        // Arrange
        var wordLenght = 1001; // existing empty file for testing
        var randomWords = new HashSet<string>(_autoFixture.CreateMany<string>(1));

        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);
        _wordsCacheDataProvider.GetWordsFromFile(wordLenght).Returns(randomWords);

        // Act
        var result = await _wordsService.GetRandomWordAsync(wordLenght);

        // Assert
        Assert.That(result, Is.EqualTo(randomWords.First()));
    }

    [Test]
    public async Task GetWordValidation_WhenWordNotExists_ThenThrowValidationFailedException()
    {
        // Arrange
        var wordToValidate = _autoFixture.Create<string>();
        var targetWord = wordToValidate;
        var hashSet = new HashSet<string>(_autoFixture.CreateMany<string>());

        _wordsCacheDataProvider.GetWordsFromFile(wordToValidate.Length).Returns(hashSet);

        var result = await _wordsService.GetWordValidationAsync(wordToValidate, targetWord);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CharacterValidations, Is.Null);
        Assert.That(result.WordExists, Is.False);
    }

    [Test]
    public async Task GetWordValidation_WhenWordsAreEqual_ReturnAllMatchValidation()
    {
        // Arrange
        var wordToValidate = _autoFixture.Create<string>();
        var targetWord = wordToValidate;
        var hashSet = new HashSet<string>(new List<string> { wordToValidate });

        _wordsCacheDataProvider.GetWordsFromFile(wordToValidate.Length).Returns(hashSet);

        // Act
        var result = await _wordsService.GetWordValidationAsync(wordToValidate, targetWord);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CharacterValidations.All(x => x.Status == CharacterValidaionStatus.Matches), Is.True);
        Assert.That(result.WordExists, Is.True);
    }

    [Test]
    public async Task GetWordValidation_WhenWordsAreDifferent_ReturnActualValidation()
    {
        // Arrange
        var wordToValidate = "АААс";
        var targetWord = "cАБССВа";
        var hashSet = new HashSet<string>(new List<string> { wordToValidate, targetWord });

        _wordsCacheDataProvider.GetWordsFromFile(wordToValidate.Length).Returns(hashSet);

        // Act
        var result = await _wordsService.GetWordValidationAsync(wordToValidate, targetWord);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.Matches).Count(), Is.EqualTo(2));
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.Exists).Count(), Is.EqualTo(1));
        Assert.That(result.CharacterValidations.Where(x => x.Status == CharacterValidaionStatus.NotExists).Count(), Is.EqualTo(1));
        Assert.That(result.WordExists, Is.True);
    }
}
