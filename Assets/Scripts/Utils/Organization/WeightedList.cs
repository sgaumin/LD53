using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	[System.Serializable]
	public struct WeightedListEntry<T> where T : UnityEngine.Object
	{
		public int weight;
		public T item;
	}

	/// <summary>
	/// List of elements with weight.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public class WeightedList<T> where T : UnityEngine.Object
	{
		[SerializeField] private List<WeightedListEntry<T>> items = new List<WeightedListEntry<T>>();

		/// <summary>
		/// Return a element from the weighted list randomly choosen.
		/// </summary>
		/// <returns>An element T.</returns>
		public T GetRandom()
		{
			T result = null;
			int maxCount = 0;
			foreach (WeightedListEntry<T> item in items)
			{
				maxCount += item.weight;
			}

			int selection = UnityEngine.Random.Range(0, maxCount) + 1;
			int currentIndex = 0;
			foreach (WeightedListEntry<T> item in items)
			{
				if ((currentIndex + item.weight) >= selection)
				{
					result = item.item;
					break;
				}

				currentIndex += item.weight;
			}

			return result;
		}
	}
}