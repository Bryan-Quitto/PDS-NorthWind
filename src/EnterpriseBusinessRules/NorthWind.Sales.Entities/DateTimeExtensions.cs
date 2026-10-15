namespace NorthWind.Sales.Entities;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo EcuadorTimeZone = ResolveEcuadorTimeZone();

    private static TimeZoneInfo ResolveEcuadorTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Guayaquil");
        }
        catch (TimeZoneNotFoundException)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.CreateCustomTimeZone("UTC-05", TimeSpan.FromHours(-5), "UTC-05", "UTC-05");
            }
        }
    }

    /// <summary>
    /// Convierte una fecha y hora (asumida o en formato UTC) a la hora local de Ecuador (UTC-5).
    /// </summary>
    public static DateTime ToEcuadorTime(this DateTime dateTime)
    {
        var utcDateTime = dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };

        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, EcuadorTimeZone);
    }

    /// <summary>
    /// Convierte un DateTimeOffset a la hora local de Ecuador (UTC-5).
    /// </summary>
    public static DateTimeOffset ToEcuadorTime(this DateTimeOffset dateTimeOffset)
    {
        return TimeZoneInfo.ConvertTime(dateTimeOffset, EcuadorTimeZone);
    }
}
