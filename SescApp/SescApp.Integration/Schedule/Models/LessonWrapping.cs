namespace SescApp.Integration.Schedule.Models
{
    internal record LessonWrapping
    {
        public bool IsDiff { get; init; }

        public DomainModels.Lesson Lesson { get; init; }
    }
}
