namespace SescApp.Integration.Schedule.DomainModels
{
    internal record LessonWrapping
    {
        public bool IsDiff { get; init; }

        public DomainModels.Lesson Lesson { get; init; }
    }
}
