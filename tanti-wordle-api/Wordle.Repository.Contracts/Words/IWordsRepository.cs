namespace Wordle.Repository.Contracts.Words;
public interface IWordsRepository
{
    Task<HashSet<string>> GetWordsFromFile(int wordLenght);
}
