using System.Collections;
using UnityEngine;

namespace Utils
{
	public static class MonoBehaviourCoroutineExtensions
	{
		/// <summary>
		/// Start a routine by making sure that it has been stopped.
		/// </summary>
		/// <param name="m">Monobehavior holder</param>
		/// <param name="routineMethod">Method</param>
		/// <param name="routine">Coroutine</param>
		public static void TryStartCoroutine(this MonoBehaviour m, IEnumerator routineMethod, ref Coroutine routine)
		{
			TryStopCoroutine(m, ref routine);
			routine = m.StartCoroutine(routineMethod);
		}

		/// <summary>
		/// Stop a coroutine if it is not null.
		/// </summary>
		/// <param name="m">Monobehavior holder</param>
		/// <param name="routine">Coroutine</param>
		public static void TryStopCoroutine(this MonoBehaviour m, ref Coroutine routine)
		{
			if (routine != null)
			{
				m.StopCoroutine(routine);
				routine = null;
			}
		}
	}
}