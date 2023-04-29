using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Facade;

public class PlacementManager : MonoBehaviour
{
	[SerializeField] private List<ObstacleData> configs;

	[Header("Animation")]
	[SerializeField] private float yInPosition;
	[SerializeField] private float fadeDuration = 0.5f;
	[SerializeField] private Ease fadeInEase = Ease.OutBack;
	[SerializeField] private Ease fadeOutEase = Ease.InBack;

	[Header("References")]
	[SerializeField] private ObstaclePlacer[] obstaclePlacers;

	private Vector2 startPosition;

	private void Reset()
	{
		obstaclePlacers = GetComponentsInChildren<ObstaclePlacer>();
	}

	private void Awake()
	{
		Level.OnLevelEditingEvent += FadIn;
		Level.OnRunningEvent += FadOut;

		startPosition = transform.position;
	}

	private void OnDestroy()
	{
		Level.OnLevelEditingEvent -= FadIn;
		Level.OnRunningEvent -= FadOut;
	}

	private void Start()
	{
		foreach (ObstaclePlacer obstaclePlacer in obstaclePlacers)
		{
			obstaclePlacer.gameObject.SetActive(false);
		}

		// Visual rearrangement 
		int count = configs.Count;
		List<ObstaclePlacer> reorderedPlacers = new List<ObstaclePlacer>();
		if (count == 1)
		{
			reorderedPlacers.Add(obstaclePlacers[1]);
		}
		else if (count == 2)
		{
			reorderedPlacers.Add(obstaclePlacers[0]);
			reorderedPlacers.Add(obstaclePlacers[2]);

		}
		else
		{
			reorderedPlacers.AddRange(obstaclePlacers);
		}

		// Setup
		for (int i = 0; i < count; i++)
		{
			reorderedPlacers[i].Setup(configs[i]);
			reorderedPlacers[i].gameObject.SetActive(true);
		}
	}

	private void FadIn()
	{
		transform.DOMoveY(yInPosition, fadeDuration).SetEase(fadeInEase);
	}

	private void FadOut()
	{
		transform.DOMoveY(startPosition.y, fadeDuration).SetEase(fadeOutEase);
	}
}