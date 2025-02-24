using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Wordle.Api.Words;
[ExcludeFromCodeCoverage]
public class WordsMapper : Profile
{
    public WordsMapper()
    {
        CreateMap<Services.Contracts.Models.CharacterValidation, Models.CharacterValidation>();
        CreateMap<Services.Contracts.Models.WordValidation, Models.WordValidation>();
    }
}
