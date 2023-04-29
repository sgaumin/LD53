using System;

namespace Utils
{
	public static class TimeManager
	{
		public static long DateTimeToUnixTimeStamp(DateTime dt)
		{
			var utc = dt.ToUniversalTime();
			return (long)(utc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public static DateTime DateTimeNow()
		{
			return DateTime.UtcNow;
		}

		public static long UnixTimeNow()
		{
			return DateTimeToUnixTimeStamp(DateTime.UtcNow);
		}
	}
}