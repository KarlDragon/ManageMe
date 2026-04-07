public class LocalTimeConverter
{
    /// <summary>
    /// Converts a UTC DateTimeOffset to the user's local time based on the provided timezone offset.
    /// </summary>
    /// <param name="utcDate">The UTC date and time to convert.</param>
    /// <param name="tzOffsetMinutes">The timezone offset in minutes from UTC.</param>
    /// <returns>The converted local DateTimeOffset.</returns>
    public static DateTimeOffset ConvertToLocalTime(DateTimeOffset utcDate, int tzOffsetMinutes)
    {
        return utcDate.ToOffset(TimeSpan.FromMinutes(tzOffsetMinutes));
    }
}