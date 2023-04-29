using UnityEngine;

namespace Utils
{
	// See Design Pattern example: https://www.notion.so/Index-8c49dc7f08e241238ca8b933268d2661

	public abstract class BaseIndex : ScriptableObject
	{
		protected static T GetOrLoad<T>(ref T _instance) where T : BaseIndex
		{
			if (_instance == null)
			{
				var name = typeof(T).Name;

				_instance = Resources.Load<T>(name);

				if (_instance == null)
				{
					Debug.LogWarning($"Failed to load index: '{name}'.\nIndex file must be placed at: Resources/{name}.asset");
				}
			}

			return _instance;
		}
	}
}