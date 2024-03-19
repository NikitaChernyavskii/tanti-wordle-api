namespace Wordle.Services.Contracts.Words
{
    public interface IWordsService
    {
        Task<string> GetRandomWord(int wordLenght);
    }
}
