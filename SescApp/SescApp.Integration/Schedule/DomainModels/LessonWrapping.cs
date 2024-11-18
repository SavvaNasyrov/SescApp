using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Schedule.DomainModels
{
    internal record LessonWrapping
    {
        public bool IsDiff {  get; init; }

        public DomainModels.Lesson Lesson { get; init; }
    }
}
