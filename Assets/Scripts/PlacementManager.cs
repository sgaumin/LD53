using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
	[SerializeField] private List<ObstacleData> configs;

	[Header("References")]
	[SerializeField] private ObstaclePlacer[] obstaclePlacers;

	private void Reset()
	{
		obstaclePlacers = GetComponentsInChildren<ObstaclePlacer>();
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
}