namespace Wordle.Repository.Contracts.Words;
public interface IWordsRepository
{
    Task<List<string>> GetWordsFromFile(int wordLenght);
}
