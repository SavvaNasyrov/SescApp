namespace SescApp.Integration.Lycreg.Services
{
    public interface ICaptchaSolver
    {
        public Task<(string CaptchaId, string CaptchaSolution)> GetSolvedCaptcha();
    }
}
