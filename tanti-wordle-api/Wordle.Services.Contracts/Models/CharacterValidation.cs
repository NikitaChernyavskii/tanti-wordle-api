using System.Diagnostics.CodeAnalysis;

namespace Wordle.Services.Contracts.Models;

[ExcludeFromCodeCoverage]
public class CharacterValidation
{
    public char Character { get; set; }
    public CharacterValidaionStatus Status { get; set; }
}

