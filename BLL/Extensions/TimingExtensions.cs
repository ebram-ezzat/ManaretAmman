namespace BusinessLogicLayer.Extensions;

public static class TimingExtensions
{
    public static int? DateToIntValue(this DateTime? date)
    {
        if (date == null)
            return null;
       var _date  = date.Value;
        string month  = _date.Month.ToString().Length==1?"0"+ _date.Month.ToString(): _date.Month.ToString();
        string day  = _date.Day.ToString().Length == 1 ? "0" + _date.Day.ToString() : _date.Day.ToString();
         int  result;
        if(int.TryParse(_date.Year.ToString() + month + day, out result))
        return result ;
        return null;
    }

    public static long? DateWithTimeToIntValue(this DateTime? date)
    {
        if (date == null)
            return null;

        var _date = date.Value;
        string month = _date.Month.ToString().Length == 1 ? "0" + _date.Month.ToString() : _date.Month.ToString();
        string day = _date.Day.ToString().Length == 1 ? "0" + _date.Day.ToString() : _date.Day.ToString();
        string hour = _date.Hour.ToString().Length == 1 ? "0" + _date.Hour.ToString() : _date.Hour.ToString();
        string minute = _date.Minute.ToString().Length == 1 ? "0" + _date.Minute.ToString() : _date.Minute.ToString();
        string second = _date.Second.ToString().Length == 1 ? "0" + _date.Second.ToString() : _date.Second.ToString();

        long result;
        if (long.TryParse(_date.Year.ToString() + month + day + hour + minute + second, out result))
            return result;
        return null;
    }
    public static DateTime? IntToDateTimeValue(this int? intDate)
    {
        if (intDate == null || intDate.ToString().Length != 14)
            return null;
        var _date = intDate.Value.ToString();
        string year = _date.Substring(0, 4);
        string month = _date.Substring(4, 2);
        string day = _date.Substring(6, 2);
        string hour = _date.Substring(8, 2);
        string minute = _date.Substring(10, 2);
        string second = _date.Substring(12, 2);

        if (int.TryParse(day, out int dayint) && int.TryParse(month, out int monthint) && int.TryParse(year, out int yearint) &&
            int.TryParse(hour, out int hourint) && int.TryParse(minute, out int minuteint) && int.TryParse(second, out int secondint))
        {
            Console.WriteLine(new DateTime(yearint, monthint, dayint, hourint, minuteint, secondint).ToString());
            return new DateTime(yearint, monthint, dayint, hourint, minuteint, secondint);
        }
        return null;
    }
    public static DateTime? IntToDateValue(this int? intDate)
    {
        if (intDate == null || intDate.ToString().Length!=8)
            return null;
        var _date = intDate.Value.ToString();
        string year = _date.Substring(0, 4);
        string month = _date.Substring(4, 2);
        string day = _date.Substring(6, 2);
        if (int.TryParse(day, out int dayint) && int.TryParse(month, out int monthint) && int.TryParse(year, out int yearint))
        { 
            Console.WriteLine(new DateTime(yearint, monthint, dayint).ToString());
            return new DateTime(yearint, monthint, dayint);
        }
        return null;
    }
    public static int? ConvertFromDateTimeToUnixTimestamp(this DateTime? date)
    {
        if (date == null)
            return null;

        return (int?)date.Value.ToUniversalTime()
              .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
              .TotalSeconds;
    }

    public static int ConvertFromTimeStringToMinutes(this string time)
    {
        if (string.IsNullOrEmpty(time)) return 0;
        var timeSpan = TimeSpan.Parse(time);

        return (int)timeSpan.TotalMinutes;
    }

    public static DateTime? ConvertFromUnixTimestampToDateTime(this int? timestamp)
    {
        if (timestamp == null) return null;

        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return epoch.AddSeconds((int)timestamp);
    }

    public static string ConvertFromMinutesToTimeString(this int? minutes)
    {
        if (minutes == null) return null;

        var timeSpan = TimeSpan.FromMinutes((int)minutes);

        return timeSpan.ToString(@"hh\:mm");
    }

    public static string ConvertFromMinutesToTimeString(this int minutes)
    {
        //if (minutes == null) return null;

        var timeSpan = TimeSpan.FromMinutes((int)minutes);

        return timeSpan.ToString(@"hh\:mm");
    }

    public static int? TimeStringToIntValue(this string Time)
    {
        // Parse the time string into a TimeSpan
        TimeSpan timeSpan = TimeSpan.Parse(Time);

        // Calculate the total number of minutes
        return (int)timeSpan.TotalMinutes;
    }
}
