﻿using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;
using SescApp.Integration.Lycreg.Models.MediatR;

namespace SescApp.Integration.Lycreg.Services.MediatR;

public class CaptchaSolver(HttpClient httpClient, IConfiguration config) : IRequestHandler<GetSolvedCaptchaRequest, SolvedCaptcha>
{
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    private static readonly Dictionary<(long, long), int> COLUMNS_PAIRS = new()
    {
        {(524287, 458759), 0}, {(24579, 49155), 0}, {(7, 131071), 1}, {(415, 111), 1},
        {(126983, 258079), 2}, {(24591, 57371), 2}, {(519935, 462343), 3}, {(115459, 99075), 3},
        {(63503, 524287), 4}, {(227, 451), 4}, {(261951, 523903), 5}, {(24831, 6159), 5},
        {(465927, 516095), 6}, {(15111, 29443), 6}, {(460799, 524287), 7}, {(24591, 12303), 7},
        {(524287, 462343), 8}, {(27, 15), 8}, {(459207, 459143), 9}, {(57731, 49347), 9}
    };

    private static readonly Dictionary<long, int> NUM2I = new()
    {
        {0, 0}, {1, 0}, {2, 1}, {4, 2}, {8, 3}, {16, 4}, {32, 5}, {64, 6}, {128, 7}, {256, 8}, {512, 9},
        {1024, 10}, {2048, 11}, {4096, 12}, {8192, 13}, {16384, 14}, {32768, 15}, {65536, 16}, {131072, 17},
        {262144, 18}, {524288, 19}, {1048576, 20}, {2097152, 21}, {4194304, 22}, {8388608, 23}, {16777216, 24},
        {33554432, 25}, {67108864, 26}, {134217728, 27}, {268435456, 28}, {536870912, 29}, {1073741824, 30},
        {2147483648, 31}
    };

    public async Task<SolvedCaptcha> Handle(GetSolvedCaptchaRequest request, CancellationToken cancellationToken)
    {
        var (captchaBytes, captchaId) = await FetchCaptcha(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var captchaSolution = SolveCaptcha(captchaBytes);

        return new SolvedCaptcha
        {
            CaptchaId = captchaId,
            CaptchaSolution = captchaSolution,
            CaptchaBytes = captchaBytes,
        };
    }

    private async Task<(byte[] CaptchaBytes, string CaptchaId)> FetchCaptcha(CancellationToken token)
    {
        var response = await httpClient.GetAsync($"{_lycregSource}cpt.a", token);

        token.ThrowIfCancellationRequested();

        response.EnsureSuccessStatusCode();

        var captchaBytes = await response.Content.ReadAsByteArrayAsync(token);

        token.ThrowIfCancellationRequested();

        var captchaId = response.Headers.GetValues("X-Cpt").First();

        return (captchaBytes, captchaId);
    }

    private static string SolveCaptcha(byte[] captchaBytes)
    {
        var data = captchaBytes.Skip(104).SkipLast(20).ToArray();

        var numbers = new List<long>();

        for (int i = 0; i < 121; i++)
        {
            byte[] subArray = data.Skip(i).Take(3630 - i).Where((b, index) => index % 121 == 0).ToArray();

            var number = Convert.ToInt64(new string(subArray.Select(x => x == 0 ? '0' : '1').ToArray()), 2);

            numbers.Add(number);
        }

        var columns = numbers.Select(x => x >> NUM2I[x & -x]).ToArray();

        var solution = new StringBuilder();
        var waitFor0 = false;

        for (int i = 0; i < 120; i++)
        {
            var pair = (columns[i], columns[i + 1]);

            if (waitFor0 && pair.Item2 == 0)
            {
                waitFor0 = false;
            }
            else if (COLUMNS_PAIRS.TryGetValue(pair, out int value))
            {
                solution.Append(value);
                waitFor0 = true;
            }
        }

        return solution.ToString();
    }
}

