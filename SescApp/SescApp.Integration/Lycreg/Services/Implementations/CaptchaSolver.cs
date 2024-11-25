using Microsoft.Extensions.Configuration;

namespace SescApp.Integration.Lycreg.Services.Implementations;

public class CaptchaSolver(HttpClient httpClient, IConfiguration config) : ICaptchaSolver
{
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    private static readonly Dictionary<(int, int), int> ColumnsPairs = new Dictionary<(int, int), int>
    {
        {(524287, 458759), 0}, {(24579, 49155), 0}, {(7, 131071), 1}, {(415, 111), 1},
        {(126983, 258079), 2}, {(24591, 57371), 2}, {(519935, 462343), 3}, {(115459, 99075), 3},
        {(63503, 524287), 4}, {(227, 451), 4}, {(261951, 523903), 5}, {(24831, 6159), 5},
        {(465927, 516095), 6}, {(15111, 29443), 6}, {(460799, 524287), 7}, {(24591, 12303), 7},
        {(524287, 462343), 8}, {(27, 15), 8}, {(459207, 459143), 9}, {(57731, 49347), 9}
    };

    private static readonly Dictionary<long, int> Num2I = new Dictionary<long, int>
    {
        {0, 0}, {1, 0}, {2, 1}, {4, 2}, {8, 3}, {16, 4}, {32, 5}, {64, 6}, {128, 7},
        {256, 8}, {512, 9}, {1024, 10}, {2048, 11}, {4096, 12}, {8192, 13}, {16384, 14},
        {32768, 15}, {65536, 16}, {131072, 17}, {262144, 18}, {524288, 19}, {1048576, 20},
        {2097152, 21}, {4194304, 22}, {8388608, 23}, {16777216, 24}, {33554432, 25},
        {67108864, 26}, {134217728, 27}, {268435456, 28}, {536870912, 29}, {1073741824, 30},
        {2147483648, 31}
    };

    public async Task<(string CaptchaId, string CaptchaSolution)> GetSolvedCaptcha()
    {
        var (captchaBytes, captchaId) = await FetchCaptcha();

        var captchaSolution = SolveCaptcha(captchaBytes);

        return (captchaId, captchaSolution);
    }

    private async Task<(byte[] CaptchaBytes, string CaptchaId)> FetchCaptcha()
    {
        var response = await httpClient.GetAsync($"{_lycregSource}/cpt.a");

        response.EnsureSuccessStatusCode();

        var captchaBytes = await response.Content.ReadAsByteArrayAsync();
        var captchaId = response.Headers.GetValues("X-Cpt").First();

        return (captchaBytes, captchaId);
    }

    private static string SolveCaptcha(byte[] captchaBytes)
    {
        var data = new ArraySegment<byte>(captchaBytes, 104, captchaBytes.Length - 124); // Adjusted for the slice
        var numbers = new List<int>();

        for (var i = 0; i < 121; i++)
        {
            var number = Convert.ToInt32(Convert.ToHexString(data.Array, data.Offset + i * 121, 121), 2);
            numbers.Add(number >> Num2I[number & -number]);
        }

        var solution = string.Empty;
        var waitFor0 = false;

        for (var i = 0; i < 120; i++)
        {
            var pair = (numbers[i], numbers[i + 1]);
            if (waitFor0 && pair.Item2 == 0)
            {
                waitFor0 = false;
            }
            else if (ColumnsPairs.TryGetValue(pair, out int value))
            {
                solution += value.ToString();
                waitFor0 = true;
            }
        }

        return solution;
    }
}
