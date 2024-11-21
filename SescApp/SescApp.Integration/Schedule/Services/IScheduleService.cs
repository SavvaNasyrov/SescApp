using SescApp.Integration.Schedule.Models;

namespace SescApp.Integration.Schedule.Services
{
    public interface IScheduleService
    {
        public Task<ScheduleModel> GetScheduleAsync(GetScheduleRequest request);
    }
}
