namespace SescApp.Integration.Schedule.Models
{
    public record SheduleModel
    {
        public required IScheduleRow Row1 { get; init; }

        public required IScheduleRow Row2 { get; init; }

        public required IScheduleRow Row3 { get; init; }

        public required IScheduleRow Row4 { get; init; }

        public required IScheduleRow Row5 { get; init; }

        public required IScheduleRow Row6 { get; init; }

        public required IScheduleRow Row7 { get; init; }
    }
}
