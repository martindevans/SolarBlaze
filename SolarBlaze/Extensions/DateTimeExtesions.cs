namespace SolarBlaze.Extensions
{
    public static class DateTimeExtesions
    {
        public static long ToUnixTimeSeconds(this DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }
    }
}
