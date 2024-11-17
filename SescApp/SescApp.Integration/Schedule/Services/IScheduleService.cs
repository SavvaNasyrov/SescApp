using SescApp.Integration.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Schedule.Services
{
    public interface IScheduleService
    {
        public Task<SheduleModel> GetScheduleAsync();
    }
}
