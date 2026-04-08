public class LocalTimeConverter
{
    /// <summary>
    /// Converts a UTC DateTimeOffset to the user's local time based on the provided timezone offset.
    /// </summary>
    /// <param name="utcDate">The UTC date and time to convert.</param>
    /// <param name="tzOffsetMinutes">The timezone offset in minutes from UTC.</param>
    /// <returns>The converted local DateTimeOffset.</returns>
    public static DateTimeOffset ConvertToLocalTime(string utcDate, int tzOffsetMinutes)
    {
        if (!DateTimeOffset.TryParse(utcDate, out var utc))
        {
            throw new ArgumentException("Invalid date format. Expected ISO 8601 string.", nameof(utcDate));
        }
        var localDateTime = utc.AddHours(tzOffsetMinutes);
        return new DateTimeOffset(localDateTime.DateTime, TimeSpan.FromMinutes(tzOffsetMinutes));
    }
}