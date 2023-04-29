using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public static class GameObjectExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
		}

		public static List<T> GetAllComponents<T>(this GameObject gameObject) where T : Component
		{
			List<T> allColliders = new List<T>();

			gameObject.GetComponents<T>()?.ForEach(x => allColliders.Add(x));
			gameObject.GetComponentsInChildren<T>()?.ForEach(x => allColliders.Add(x));

			return allColliders;
		}

		public static bool TryGetComponent<T>(this GameObject gameObject, out T val) where T : Component
		{
			val = gameObject.GetComponent<T>();
			return val == null;
		}

		public static bool TryGetComponent<T>(this Component component, out T val) where T : Component
		{
			val = component.GetComponent<T>();
			return val == null;
		}

		public static void EnableColliders(this GameObject gameObject)
		{
			gameObject.GetAllComponents<Collider>()?.ForEach(x => x.enabled = true);
			gameObject.GetAllComponents<Collider2D>()?.ForEach(x => x.enabled = true);
		}

		public static void DisableColliders(this GameObject gameObject)
		{
			gameObject.GetAllComponents<Collider>()?.ForEach(x => x.enabled = false);
			gameObject.GetAllComponents<Collider2D>()?.ForEach(x => x.enabled = false);
		}
	}
}