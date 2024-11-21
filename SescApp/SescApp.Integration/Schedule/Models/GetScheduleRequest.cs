namespace SescApp.Integration.Schedule.Models
{
    public record GetScheduleRequest(ScheduleType Type, Weekday Weekday, int Group);
}
