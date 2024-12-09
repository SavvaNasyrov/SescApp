namespace SescApp.Integration.Lycreg.Models.JournalModels;

public record JournalSubject
{
    public required string SubjectName { get; init; }
    
    public string? Subgroup { get; init; }
    
    public required string TeacherName { get; init; }
    
    public required List<JournalLine> Journal { get; init; }
}