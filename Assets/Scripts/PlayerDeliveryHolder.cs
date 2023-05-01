using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class PlayerDeliveryHolder : MonoBehaviour
{
	[SerializeField, ReadOnly] private List<Transform> spots = new List<Transform>();

	private List<Transform> inUsage = new List<Transform>();

	private void OnValidate()
	{
		spots.Clear();
		foreach (Transform child in transform)
		{
			spots.Add(child);
		}
	}

	private void Start()
	{
		transform.DORotate(new Vector3(0f, 0f, 360f), 3f, RotateMode.FastBeyond360).From(Vector3.zero).SetLoops(-1).SetEase(Ease.Linear);
	}

	public Transform Get()
	{
		Transform t = spots.Random();
		spots.Remove(t);
		inUsage.Add(t);
		return t;
	}


	public void Release(Transform t)
	{
		inUsage.Remove(t);
		spots.Add(t);
	}
}