using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
	public static class EnumerableExtensions
	{
		// The random number generator
		public static Random defaultRNG = new Random();

		/// <summary>
		/// Returns a list of (element, index) tuples to be able to iterate over the enumerable with the
		/// index embedded.
		/// </summary>
		/// <param name="e">The enumerable</param>
		/// <returns>
		/// A list of (element, index) tuples to be able to iterate over the enumerable with the index embedded.
		/// </returns>
		public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> e)
		   => e.Select((item, index) => (item, index));

		/// <summary>
		/// Returns true if enumerable is empty. /// This method exists to prevent using `Count() &gt;
		/// 0` in the code (slower implementation than this one)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <returns>True if the enumerable is empty</returns>
		public static bool IsEmpty<T>(this IEnumerable<T> e) => !e.Any();

		/// <summary>
		/// Orders this enumerable randomly
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array">The enumerable to shuffle</param>
		/// <returns>An enumerable with its values shuffled</returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> e, Random rng = null) =>
			e.OrderBy(x => (rng ?? defaultRNG).Next());

		/// <summary>
		/// Flattens an array of collections to a single collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable array to flatten</param>
		/// <returns>A flatten enumerable</returns>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<T>[] e) => e.SelectMany(x => x);

		/// <summary>
		/// Flattens a collection of collections to a single collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable array to flatten</param>
		/// <returns>A flatten enumerable</returns>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> e) => e.SelectMany(x => x);

		/// <summary>
		/// Returns the last specified number of contiguous elements at the end of a sequence. The
		/// ordering stays the same.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The sequence to return elements from.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns></returns>
		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count) =>
			source.Skip(System.Math.Max(0, source.Count() - count));

		/// <summary>
		/// Returns the set without all instances of the given element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="element">The element to omit</param>
		/// <returns>An enumerable without all the instances of the element</returns>
		public static IEnumerable<T> Except<T>(this IEnumerable<T> e, T element) => e.Except(new T[] { element });

		/// <summary>
		/// Returns the set without the first instance of the given element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="element">The element to omit</param>
		/// <returns>An enumerable without the first instance of the element</returns>
		public static IEnumerable<T> ExceptFirst<T>(this IEnumerable<T> e, T element)
		{
			IEnumerable<T> firstPart = e.TakeWhile(x => !x.Equals(element));
			return firstPart.Concat(e.Skip(firstPart.Count() + 1));
		}

		/// <summary>
		/// Returns the current enumerable without null values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <returns>The enumerable without the null values</returns>
		public static IEnumerable<T> WithoutNullValues<T>(this IEnumerable<T> e) => e.Where(i => i != null);

		/// <summary>
		/// Returns a random element from the enumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="r">RNG to use</param>
		/// <returns>A random value from the enumerable</returns>
		public static T Random<T>(this IEnumerable<T> e, Random rng = null) => e.ElementAt((rng ?? defaultRNG).Next(0, e.Count()));

		/// <summary>
		/// Returns a random element from the list This method is optimized for lists
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The list</param>
		/// <param name="r">RNG to use</param>
		/// <returns>A random value from the list</returns>
		public static T Random<T>(this IList<T> e, Random rng = null) => e[(rng ?? defaultRNG).Next(0, e.Count)];

		/// <summary>
		/// Executes an action on each element of the enumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="action">The action to execute on each element</param>
		public static void ForEach<T>(this IEnumerable<T> e, Action<T> action)
		{
			foreach (T t in e)
			{
				action(t);
			}
		}

		/// <summary>
		/// Executes an action on each element of the enumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="action">
		/// The action to execute on each element, with its index as the second parameter
		/// </param>
		public static void ForEach<T>(this IEnumerable<T> e, Action<T, int> action)
		{
			int i = 0;
			foreach (T t in e)
			{
				action(t, i++);
			}
		}

		/// <summary>
		/// Return the closest component to target from the list in 3D space
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static T GetClosest<T>(this IEnumerable<T> e, Vector3 target) where T : Component
			=> e.OrderBy(x => Vector3.Distance(x.transform.position, target)).FirstOrDefault();

		/// <summary>
		/// Return the furthest component to target from the list in 3D space
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static T GetFurthest<T>(this IEnumerable<T> e, Vector3 target) where T : Component
			=> e.OrderBy(x => Vector3.Distance(x.transform.position, target)).LastOrDefault();

		/// <summary>
		/// Return the closest component to target from the list in 2D space
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static T GetClosest2D<T>(this IEnumerable<T> e, Vector2 target) where T : Component
			=> e.OrderBy(x => Vector2.Distance(x.transform.position, target)).FirstOrDefault();

		/// <summary>
		/// Return the furthest component to target from the list in 2D space
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e">The enumerable</param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static T GetFurthest2D<T>(this IEnumerable<T> e, Vector2 target) where T : Component
			=> e.OrderBy(x => Vector2.Distance(x.transform.position, target)).LastOrDefault();
	}
}