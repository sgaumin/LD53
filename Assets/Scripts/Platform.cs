using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Platform : MonoBehaviour, IRespawn
{
	[SerializeField] private bool isFalling;

	[Header("Checking")]
	[SerializeField] private Color invalidColor;
	[SerializeField] private Color validColor;

	[Header("Obstacles")]
	[SerializeField] private Obstacle woodPrefab;
	[SerializeField] private Obstacle rockPrefab;
	[SerializeField] private Obstacle waterPrefab;

	[Header("References")]
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private SpriteRenderer overlay;
	[SerializeField] private Collider2D boxCollider2D;

	public bool IsFalling => isFalling;

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
		obstacle.HasBeenEdited = true;
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

	public async UniTask Fall()
	{
		boxCollider2D.enabled = false;

		await transform.DOScale(Vector2.zero, 0.5f);

		gameObject.SetActive(false);
	}

	public void Initialization()
	{
		boxCollider2D.enabled = true;

		transform.DOKill();
		transform.localScale = Vector3.one;

		if (!gameObject.activeSelf)
			gameObject.SetActive(true);
	}
}
