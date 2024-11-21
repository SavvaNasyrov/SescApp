namespace SescApp.Integration.Schedule.Models.ScheduleRows
{
    public record DividedScheduleRow(Lesson FirstLesson, Lesson SecondLesson) : IScheduleRow
    {
        public Lesson FirstLesson { get; init; } = FirstLesson;

        public Lesson SecondLesson { get; init; } = SecondLesson;
    }
}
