namespace SescApp.Integration.Lycreg.Services.Implementations;

public class DateConverter : IDateConverter
{
    public static DateTime DateConv(string e)
    {
        if (e.Contains('-'))
        {
            var parts = e.Split('-');
            var month = int.Parse(parts[1]);
            var day = int.Parse(parts[2]);
            month = (month > 8) ? (month - 9) : (month + 3);
            return new DateTime(DateTime.Now.Year, month, day);
        }
        if (e.Contains('.'))
        {
            var parts = e.Split('.');
            var day = parts[0];
            var month = int.Parse(parts[1]);
            month = (month > 8) ? (month - 9) : (month + 3);
            return new DateTime(DateTime.Now.Year, month, int.Parse(day));
        }
        else
        {
            var r = int.Parse(e.Substring(1, 1));
            var day = int.Parse(e.Substring(2, 2));
            var month = (r < 4) ? (r + 9) : (r - 3);
            month = month < 1 ? 1 : month; // Убедитесь, что месяц не меньше 1
            
            var currentDate = DateTime.Now;
            var year = currentDate.Year;
            var monthNow = currentDate.Month;

            if (r < 4 && monthNow < 8)
            {
                year--;
            }
            return new DateTime(year, month, day);

        }
    }
}
