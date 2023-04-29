using UnityEngine;

public class Platform : MonoBehaviour
{
	[SerializeField] private LayerMask obstacleLayer;

	[Header("Checking")]
	[SerializeField] private Color invalidColor;
	[SerializeField] private Color validColor;

	[Header("Obstacles")]
	[SerializeField] private Obstacle woodPrefab;
	[SerializeField] private Obstacle rockPrefab;
	[SerializeField] private Obstacle waterPrefab;

	[Header("References")]
	[SerializeField] private SpriteRenderer overlay;

	private bool HasObstacle()
	{
		return Physics2D.Linecast(transform.position, transform.position, obstacleLayer);
	}

	public bool SetObstacle(ObstacleData data)
	{
		if (HasObstacle()) return false;

		Obstacle obstacle = null;
		switch (data.type)
		{
			case ObstacleType.Wood:
				obstacle = woodPrefab;
				break;
			case ObstacleType.Rock:
				obstacle = rockPrefab;
				break;
			case ObstacleType.Water:
				obstacle = waterPrefab;
				break;
		}

		obstacle = Instantiate(obstacle);
		obstacle.transform.transform.position = transform.position;

		return true;
	}

	public void ShowOverlay()
	{
		overlay.gameObject.SetActive(true);
		overlay.color = HasObstacle() ? invalidColor : validColor;
	}

	public void HideOverlay()
	{
		overlay.gameObject.SetActive(false);
	}
}
