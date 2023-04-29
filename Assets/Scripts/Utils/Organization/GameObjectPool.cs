using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public class GameObjectPool : Singleton<GameObjectPool>
	{
		public void OnSingletonSetup()
		{
		}

		[SerializeField] private List<GameObject> prefabs = null;

		private Dictionary<string, List<GameObject>> objectLists = new Dictionary<string, List<GameObject>>();
		private Dictionary<string, Transform> objectHolders = new Dictionary<string, Transform>();

		public GameObject GetObject(Enum type) => GetObjectInternal(type.ToString());

		private GameObject GetObjectInternal(string typeName = "")
		{
			if (!objectLists.ContainsKey(typeName))
			{
				objectLists.Add(typeName, new List<GameObject>());
			}

			List<GameObject> list = objectLists[typeName];
			if (list.IsEmpty())
			{
				GameObject prefab = GetPrefab(typeName);
				GameObject newObject = Instantiate(prefab);
				newObject.transform.SetParent(objectHolders[typeName], false);
				newObject.name = prefab.name;

				return newObject;
			}
			else
			{
				int lastCellIndex = list.Count - 1;
				GameObject obj = list[lastCellIndex];
				list.RemoveAt(lastCellIndex);
				obj.SetActive(true);

				return obj;
			}
		}

		private GameObject GetPrefab(string prefabName)
		{
			if (prefabs == null)
			{
				throw new Exception("GameObjectPool-> Prefab List is empty");
			}
			else
			{
				GameObject requestedPrefab = prefabs.Find(x => x.name == prefabName);
				if (requestedPrefab)
				{
					return requestedPrefab;
				}
				else
				{
					throw new Exception("GameObjectPool->GetPrefab : Could not find " + prefabName + " prefab.");
				}
			}
		}

		public void ReturnToRepository(GameObject obj, Enum type)
		{
			string groupName = type.ToString();

			if (!objectHolders.ContainsKey(groupName))
			{
				GameObject o = new GameObject(groupName);
				o.transform.SetParent(transform, false);
				objectHolders.Add(groupName, o.transform);
			}
			obj.transform.SetParent(objectHolders[groupName]);
			obj.transform.position = Vector3.zero;
			obj.gameObject.SetActive(false);

			if (!objectLists.ContainsKey(groupName))
			{
				objectLists.Add(groupName, new List<GameObject>());
			}
			objectLists[groupName].Add(obj.gameObject);
		}
	}
}