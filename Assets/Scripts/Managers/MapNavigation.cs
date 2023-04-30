using UnityEngine;
using static Facade;

public class MapNavigation : MonoBehaviour
{
	private static Vector2 lastPosition;

	private void OnEnable()
	{
		Level.State = GameState.OnMap;
		transform.position = lastPosition;
	}

	private void OnDisable()
	{
		lastPosition = transform.position;
	}
}