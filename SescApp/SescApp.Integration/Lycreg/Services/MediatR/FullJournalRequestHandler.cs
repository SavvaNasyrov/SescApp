using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;
using SescApp.Integration.Lycreg.Models.JournalModels;
using SescApp.Integration.Lycreg.Models.MediatR;
using SescApp.Integration.Lycreg.Services.Implementations;

namespace SescApp.Integration.Lycreg.Services.MediatR;

public class FullJournalRequestHandler(HttpClient httpClient, IConfiguration config, IMemoryCache cache, IMediator mediator)
    : IRequestHandler<FullJournalRequest, FullJournalResponse>
{
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    public async Task<FullJournalResponse> Handle(FullJournalRequest request, CancellationToken cancellationToken)
    {
        var journalRequest = new
        {
            t = request.Auth.ShortRole,
            l = request.Auth.Login,
            p = request.Auth.Token,
            f = "jrnGet",
            z = (List<object>)[],
        };

        var resp = await httpClient.PostAsJsonAsync(_lycregSource, journalRequest, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        if (!resp.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http status code {resp.StatusCode}: {resp.ReasonPhrase}");
        }

        var journalData = await resp.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, List<object>>>>(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        
        // преобразовываем сырой json в модельки
        var subjectsList = new List<JournalSubject>();
        foreach (var subject in journalData!)
        {
            var lessonsList = subject.Value.Select(line => new JournalLine()
                {
                    Date = DateConverter.DateConv(line.Key),
                    LessonTopic = line.Value[0].ToString()!,
                    HomeWork = line.Value[1].ToString()! != "" ? line.Value[1].ToString()! : null,
                    Weight = Convert.ToInt32(line.Value[2].ToString()),
                    Marks = line.Value.Count == 4 ? line.Value[3].ToString()!.Split(' ').ToArray() : null,
                })
                .ToList();

            var parts = subject.Key.Split('_');
            var subgroup = parts[0].Split('-').Length == 2 ? parts[0].Split('-')[1] : null; // подгруппа обучения
            var subjectId = parts[1];
            var teacherLogin = parts[2];
            
            var subjList = await mediator.Send(new SubjListRequest()
            {
                Auth = request.Auth,
            }, cancellationToken);
            var teacherList = await mediator.Send(new TeachListRequest()
            {
                Auth = request.Auth,
            }, cancellationToken);

            var journalSubject = new JournalSubject()
            {
                SubjectName = subjList[subjectId],
                Subgroup = subgroup,
                TeacherName = teacherList[teacherLogin],
                Journal = lessonsList,
                Marks = lessonsList
                    .SelectMany(lesson => lesson.Marks ?? Enumerable.Empty<string>())
                    .ToList()
            };
            subjectsList.Add(journalSubject);
        }

        return new FullJournalResponse()
        {
            Subjects = subjectsList,
        };
    }
}