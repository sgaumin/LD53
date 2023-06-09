using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Facade;

public class PlacementManager : MonoBehaviour
{
	[SerializeField] private bool startWithManualMode;
	[SerializeField] private List<ObstacleData> configs;

	[Header("Animation")]
	[SerializeField] private float yInPosition;
	[SerializeField] private float fadeDuration = 0.5f;
	[SerializeField] private Ease fadeInEase = Ease.OutBack;
	[SerializeField] private Ease fadeOutEase = Ease.InBack;

	[Header("References")]
	[SerializeField] private Transform obstacleHolder;
	[SerializeField] private Transform clearButton;
	[SerializeField] private ObstaclePlacer[] obstaclePlacers;

	private float startVerticalPosition;

	public static bool ManualMode { get; set; }

	private void Reset()
	{
		obstaclePlacers = GetComponentsInChildren<ObstaclePlacer>();
	}

	private void Awake()
	{
		Level.OnLevelEditingEvent += HolderFadIn;
		Level.OnRunningEvent += HolderFadOut;
		Level.OnRunningEvent += ClearFadOut;
		ObstaclePlacer.OnTotalAmountChangeEvent += ClearFadIn;

		ManualMode = startWithManualMode;

		startVerticalPosition = obstacleHolder.position.y;
	}

	private void OnDestroy()
	{
		Level.OnLevelEditingEvent -= HolderFadIn;
		Level.OnRunningEvent -= HolderFadOut;
		Level.OnRunningEvent -= ClearFadOut;
		ObstaclePlacer.OnTotalAmountChangeEvent -= ClearFadIn;
	}

	private void Start()
	{
		int count = configs.Count;
		if (count == 0)
		{
			Level.State = GameState.Running;
			return;
		}

		foreach (ObstaclePlacer obstaclePlacer in obstaclePlacers)
		{
			obstaclePlacer.gameObject.SetActive(false);
		}

		// Visual rearrangement 
		List<ObstaclePlacer> reorderedPlacers = new List<ObstaclePlacer>();
		if (count == 1)
		{
			reorderedPlacers.Add(obstaclePlacers[1]);

			Destroy(obstaclePlacers[0].gameObject);
			Destroy(obstaclePlacers[2].gameObject);
		}
		else if (count == 2)
		{
			reorderedPlacers.Add(obstaclePlacers[0]);
			reorderedPlacers.Add(obstaclePlacers[2]);

			Destroy(obstaclePlacers[1].gameObject);
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

	public void ResetPlacement()
	{
		ObstaclePlacer.TotalAmount = 0;

		foreach (ObstaclePlacer placer in FindObjectsOfType<ObstaclePlacer>(true).ToList())
		{
			placer.Initialization();
		}

		foreach (Obstacle obstacle in FindObjectsOfType<Obstacle>(true).ToList())
		{
			obstacle.Initialization();
		}
	}

	private void HolderFadIn()
	{
		if (ManualMode) return;

		obstacleHolder.DOKill();
		obstacleHolder.DOMoveY(yInPosition, fadeDuration).SetEase(fadeInEase);
	}

	private void HolderFadOut()
	{
		if (ManualMode) return;

		obstacleHolder.DOKill();
		obstacleHolder.DOMoveY(startVerticalPosition, fadeDuration).SetEase(fadeOutEase);
	}

	private void ClearFadIn(int amount)
	{
		if (ManualMode || amount <= 0) return;

		clearButton.DOKill();
		clearButton.DOMoveY(yInPosition, fadeDuration).SetEase(fadeInEase);
	}

	private void ClearFadOut()
	{
		if (ManualMode) return;

		clearButton.DOKill();
		clearButton.DOMoveY(startVerticalPosition, fadeDuration).SetEase(fadeOutEase);
	}
}