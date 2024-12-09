namespace SescApp.Integration.Lycreg.Models;

public record JournalLine
{
    public required DateTime Date { get; init; }
    
    public required int Weight {get; init; }
    
    public required string LessonTopic { get; init; }
    
    public string? HomeWork {get; init; }
    
    public string[]? Marks {get; init; }
}