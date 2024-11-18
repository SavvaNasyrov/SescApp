using SescApp.Integration.Schedule.Models;
using SescApp.Integration.Schedule.Models.ScheduleRows;

namespace SescApp.Integration.Schedule.Services.Implementations
{
    public class FakeScheduleService : IScheduleService
    {
        public Task<ScheduleModel> GetScheduleAsync(GetScheduleRequest req)
        {
            return Task.FromResult(new ScheduleModel
            {
                Row1 = new EmptyScheduleRow(),
                Row2 = new SimpleScheduleRow(new Lesson("goooool, with goyda, 201")),
                Row3 = new SimpleScheduleRow(new Lesson("goooool, with goyda, 201")),
                Row4 = new SimpleScheduleRow(new Lesson("goooool, with goyda, 201")),
                Row5 = new DividedScheduleRow(new Lesson("English, Hulk, 313"), new Lesson("English, Otvetctvennost, 312")),
                Row6 = new EmptyScheduleRow(),
                Row7 = new SimpleScheduleRow(new Lesson("goooool, with goyda, 201")),
            });
        }
    }
}
