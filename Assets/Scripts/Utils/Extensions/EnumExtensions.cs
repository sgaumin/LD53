using System;

namespace Utils
{
	public static class EnumExtensions
	{
		/// <summary>
		/// Parse enum in a simple way
		///
		/// Example: StatusEnum MyStatus = "Active".ToEnum<StatusEnum>();
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="value">string to parse</param>
		/// <returns></returns>
		public static T ToEnum<T>(this string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		/// <summary>
		/// Get next entry of this enum.
		/// </summary>
		public static T Next<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf<T>(Arr, src) + 1;
			return (Arr.Length == j) ? Arr[0] : Arr[j];
		}

		/// <summary>
		/// Get previous entry of this enum.
		/// </summary>
		public static T Previous<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf<T>(Arr, src) - 1;
			return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
		}
	}
}