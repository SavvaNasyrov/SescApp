namespace SescApp.Integration.Lycreg.Services;

public static class DtsitData
{
    public static readonly Dictionary<string, string[]> Dtsit = new Dictionary<string, string[]>
    {
        { "d205a", ["1ч", "Первая четверть", "d001", "d205"] },
        { "d204a", ["1ч", "Первая четверть", "d001", "d205"] },
        { "d331b", ["2ч", "Вторая четверть", "d206", "d331"] },
        { "d628d", ["3ч", "Третья четверть", "d401", "d628"] },
        { "d915e", ["4ч", "Четвертая четверть", "d629", "d915"] },
        { "d331c", ["1п", "Первое полугодие", "d001", "d331"] },
        { "d915f", ["2п", "Второе полугодие", "d401", "d915"] },
        { "d925g", ["Год", "Учебный год", "d001", "d925"] },
    };
}
