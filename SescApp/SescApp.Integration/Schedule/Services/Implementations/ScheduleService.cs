using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Schedule.DomainModels;
using SescApp.Integration.Schedule.Models;
using SescApp.Integration.Schedule.Models.ScheduleRows;

namespace SescApp.Integration.Schedule.Services.Implementations
{
    public class ScheduleService(HttpClient httpClient, IConfiguration config) : IScheduleService
    {
        private readonly HttpClient _httpClient = httpClient;

        private readonly string _scheduleSource = config["Paths:Schedule"] ?? throw new KeyNotFoundException("Paths:Schedule");

        public async Task<ScheduleModel> GetScheduleAsync(GetScheduleRequest request)
        {
            var resp = await _httpClient.GetAsync($"{_scheduleSource}?type=11&scheduleType={request.Type.ToString().ToLower()}&weekday={(int)request.Weekday}&group={request.Group}");

            if (!resp.IsSuccessStatusCode)
                throw new Exception("Can not load schedule");

            var obj = await resp.Content.ReadFromJsonAsync<GetScheduleResponse>() ?? throw new Exception("Can not read server answer");

            var groupings = obj.Diffs
                .Select(x => new LessonWrapping() { Lesson = x, IsDiff = true })
                .GroupBy(x => x.Lesson.Number)
                .UnionBy(obj.Lessons
                    .Select(x => new LessonWrapping() { Lesson = x, IsDiff = true })
                    .GroupBy(x => x.Lesson.Number), x => x.First().Lesson.Number)
                .Select(x => x.ToList())
                .ToList();

            Dictionary<int, IScheduleRow> rows = default!;
            switch (obj.Type)
            {
                case "group":
                    rows = groupings.ToDictionary(x => x.First().Lesson.Number, x => ParseRow(x, GroupRepr));
                    break;
            }

            var schedule = new ScheduleModel()
            {
                Row1 = rows.GetValueOrDefault(1) ?? new EmptyScheduleRow(),
                Row2 = rows.GetValueOrDefault(2) ?? new EmptyScheduleRow(),
                Row3 = rows.GetValueOrDefault(3) ?? new EmptyScheduleRow(),
                Row4 = rows.GetValueOrDefault(4) ?? new EmptyScheduleRow(),
                Row5 = rows.GetValueOrDefault(5) ?? new EmptyScheduleRow(),
                Row6 = rows.GetValueOrDefault(6) ?? new EmptyScheduleRow(),
                Row7 = rows.GetValueOrDefault(7) ?? new EmptyScheduleRow(),
            };

            return schedule;
        }

        private static Models.Lesson GroupRepr(LessonWrapping? wrapping)
        {
            if (wrapping == null)
                return new Models.Lesson("");

            return new Models.Lesson($"{wrapping.Lesson.Subject}, {wrapping.Lesson.Teacher}, {wrapping.Lesson.Auditory}", wrapping.IsDiff);
        }

        private static IScheduleRow ParseRow(List<LessonWrapping> lessons, Func<LessonWrapping?, Models.Lesson> selector)
        {
            if (lessons.First().Lesson.Subgroup == 0)
            {
                return new SimpleScheduleRow(selector(lessons.First()));
            }

            var firstWrapping = lessons.FirstOrDefault(x => x.Lesson.Subgroup == 1);
            var secondWrapping = lessons.FirstOrDefault(x => x.Lesson.Subgroup == 2);

            return new DividedScheduleRow(selector(firstWrapping), selector(secondWrapping));
        }
    }
}
