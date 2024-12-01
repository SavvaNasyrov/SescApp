namespace SescApp.Integration.Lycreg.Models
{
    public class SolvedCaptcha
    {
        public required string CaptchaId { get; init; }

        public required string CaptchaSolution { get; set; }

        public required byte[] CaptchaBytes { get; init; }
    }
}
