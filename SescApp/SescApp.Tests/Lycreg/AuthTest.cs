using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SescApp.Integration.Lycreg.Services;
using SescApp.Integration.Lycreg.Services.Implementations;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using SescApp.Integration.Lycreg.Services.MediatR;
using SescApp.Integration.Lycreg.Models.MediatR;


namespace SescApp.Tests.Lycreg
{
    public class AuthServiceTests
    {
        private ICaptchaSolver _captchaSolver;
        private HttpClient _httpClient;
        // private AuthServiceTests _authService;
        private IMediator _mediator;
        
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
            services.AddSingleton<ICaptchaSolver, CaptchaSolver>();
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
            var configuration = new TestConfiguration();
            _captchaSolver = new CaptchaSolver(_httpClient, configuration);
            _mediator = BuildMediator();
        }

        [Test]
        public async Task AuthorizationAsync_ValidCredentials_ReturnsSuccess()
        {
            const string login = "login";
            const string password = "password";
            var authResponse = await _mediator.Send(new AuthRequest()
            {
                Login = login,
                Password = password
            });

            Assert.That(authResponse.Auth, Is.Not.Null, "Авторизация провалилась");
        }
    }
}