using AudioExpress;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Platform : MonoBehaviour, IRespawn
{
	[SerializeField] private bool isFalling;

	[Header("Visual")]
	[SerializeField] private Color normalGround;
	[SerializeField] private Color fallingGround;

	[Header("Sound")]
	[SerializeField] private AudioExpress.AudioClip fallingSound;

	[Header("Checking")]
	[SerializeField] private Color invalidColor;
	[SerializeField] private Color validColor;

	[Header("Obstacles")]
	[SerializeField] private Obstacle woodPrefab;
	[SerializeField] private Obstacle rockPrefab;
	[SerializeField] private Obstacle waterPrefab;

	[Header("References")]
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private SpriteRenderer platform;
	[SerializeField] private SpriteRenderer overlay;
	[SerializeField] private Collider2D boxCollider2D;

	public bool IsFalling => isFalling;

	private void Start()
	{
		Initialization();
	}

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
		obstacle.PlayEditingSound();
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

		fallingSound.Play();
		await platform.transform.DOScale(Vector2.zero, 0.5f);
	}

	public void Initialization()
	{
		boxCollider2D.enabled = true;

		platform.color = isFalling ? fallingGround : normalGround;
		transform.DOKill();
		platform.transform.localScale = Vector3.one;
	}
}
