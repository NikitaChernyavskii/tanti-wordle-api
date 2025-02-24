using AutoFixture;
using NSubstitute;
using Wordle.Services.Contracts.Models;
using Wordle.Services.Words;
using Wordle.Services.Contracts.Words.Validators;
using Wordle.Repository.Contracts.Words;

namespace Wordle.Services.Tests.Words;
[TestFixture]
public class WordsServiceTests
{
    private IWordsServiceValidator _wordsServiceValidator;
    private IWordsRepository _wordsRepository;
    private WordsService _wordsService;

    private Fixture _autoFixture;

    [SetUp]
    public void SetUp()
    {
        _wordsServiceValidator = Substitute.For<IWordsServiceValidator>();
        _wordsRepository = Substitute.For<IWordsRepository>();
        _wordsService = new WordsService(_wordsServiceValidator, _wordsRepository);
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
        var randomWords = _autoFixture.CreateMany<string>(1).ToList();

        _wordsServiceValidator.ValidateGetWordsFromFile(wordLenght);
        _wordsRepository.GetWordsFromFile(wordLenght).Returns(randomWords);

        // Act
        var result = await _wordsService.GetRandomWordAsync(wordLenght);

        // Assert
        Assert.That(result, Is.EqualTo(randomWords.First()));
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
