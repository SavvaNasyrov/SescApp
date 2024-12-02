using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using SescApp.Integration.Lycreg.Models.MediatR;
using SescApp.Integration.Lycreg.Services;
using SescApp.Integration.Lycreg.Services.Implementations;
using SescApp.Integration.Lycreg.Services.MediatR;

namespace SescApp.Tests.Lycreg;

public class UtilsTest
{
    private HttpClient _httpClient;
    private IMediator _mediator;
    private IConfiguration _configuration;
    
    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
    
    // Custom configuration implementation
    private class TestConfiguration : IConfiguration
    {
        public string? this[string key]
        {
            get => key switch
            {
                "Paths:Lycreg" => "https://lycreg.urfu.ru/",
                _ => null
            };
            set => throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();

        public IChangeToken GetReloadToken() => NullChangeToken.Singleton;

        public IConfigurationSection GetSection(string key) => null;
    }
    
    private static IMediator BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration, TestConfiguration>();
        services.AddSingleton<HttpClient, HttpClient>();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(AuthRequestHandler).Assembly);
        });

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<IMediator>();
    }
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _configuration = new TestConfiguration();
        _mediator = BuildMediator();
    }

    private async Task<AuthResponse> Auth()
    {
        const string login = "login";
        const string password = "password";
        var captchaSolver = new CaptchaSolver(_httpClient, _configuration);
        var captchaSolve =  await captchaSolver.Handle(new GetSolvedCaptchaRequest(), CancellationToken.None);
            
        var authResponse = await _mediator.Send(new AuthRequest()
        {
            Login = login,
            Password = password,
            CaptchaId = captchaSolve.CaptchaId,
            CaptchaSolution = captchaSolve.CaptchaSolution
        });
        
        return authResponse;
    }

    [Test]
    public async Task GetSubjectList()
    {
        var authResponse = await Auth();
        
        var utils = new LycregUtils(_httpClient, _configuration);
        Debug.Assert(authResponse.Auth != null, "_authResponse.Auth != null");
        var result = await utils.GetSubjListAsync(authResponse.Auth);
        
        Assert.That(result, Is.Not.Null);
    }
    
    [Test]
    public async Task GetTeachList()
    {
        var authResponse = await Auth();
        
        var utils = new LycregUtils(_httpClient, _configuration);
        Debug.Assert(authResponse.Auth != null, "_authResponse.Auth != null");
        var result = await utils.GetTeachListAsync(authResponse.Auth);
        
        Assert.That(result, Is.Not.Null);
    }
}
