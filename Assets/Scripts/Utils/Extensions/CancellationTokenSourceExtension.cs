using System.Threading;

namespace Utils
{
	/// <summary>
	/// Extensions class for <see cref="CancellationTokenSource"/>.
	/// </summary>
	public static class CancellationTokenSourceExtension
	{
		/// <summary>
		/// Resets a <see cref="CancellationTokenSource"/> and generate a new one.
		/// </summary>
		/// <param name="source"><see cref="CancellationTokenSource"/> reference.</param>
		/// <returns>Returns a new <see cref="CancellationTokenSource"/>.</returns>
		public static CancellationTokenSource SafeReset(this CancellationTokenSource source)
		{
			source.SafeDispose();
			return new CancellationTokenSource();
		}

		/// <summary>
		/// Disposes in a safe way a <see cref="CancellationTokenSource"/>.
		/// </summary>
		/// <param name="source"><see cref="CancellationTokenSource"/> reference.</param>
		public static void SafeDispose(this CancellationTokenSource source)
		{
			if (source != null)
			{
				source.Cancel();
				source.Dispose();
			}
		}
	}
}