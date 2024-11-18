using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Schedule.DomainModels
{
    public record GetScheduleRequest(ScheduleType Type, Weekday Weekday, int Group);
}
