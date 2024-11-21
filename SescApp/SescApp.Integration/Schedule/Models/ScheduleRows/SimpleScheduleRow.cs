namespace SescApp.Integration.Schedule.Models.ScheduleRows
{
    public record SimpleScheduleRow(Lesson Lesson) : IScheduleRow
    {
        public Lesson Lesson { get; init; } = Lesson;
    }
}
