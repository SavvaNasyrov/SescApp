using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Schedule.Models.ScheduleRows
{
    public record DividedScheduleRow(Lesson FirstLesson, Lesson SecondLesson) : IScheduleRow
    {
        public Lesson FirstLesson { get; init; } = FirstLesson;

        public Lesson SecondLesson { get; init; } = SecondLesson;
    }
}
