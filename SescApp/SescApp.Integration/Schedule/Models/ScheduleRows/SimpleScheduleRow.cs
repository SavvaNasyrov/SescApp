using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Schedule.Models.ScheduleRows
{
    public record SimpleScheduleRow(Lesson Lesson) : IScheduleRow
    {
        public Lesson Lesson { get; init; } = Lesson;
    }
}
