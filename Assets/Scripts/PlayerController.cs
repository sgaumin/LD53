using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static Facade;

public class PlayerController : MonoBehaviour, IRespawn
{
	[SerializeField] private float moveSpeed;

	[Header("Animations")]
	[SerializeField] private float deathScaleDownDuration = 1f;

	[Header("References")]
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private LayerMask platformLayer;
	[SerializeField] private LayerMask obstacleLayer;

	private Vector2 spawnPosition;
	private bool isDead;
	private bool isMoving;
	private bool isFacingLeft = true;

	private void Awake()
	{
		InputManager.OnUpEvent += MoveUp;
		InputManager.OnDownEvent += MoveDown;
		InputManager.OnLeftEvent += MoveLeft;
		InputManager.OnRightEvent += MoveRight;

		spawnPosition = transform.position;
		Initialization();
	}

	private void OnDestroy()
	{
		InputManager.OnUpEvent -= MoveUp;
		InputManager.OnDownEvent -= MoveDown;
		InputManager.OnLeftEvent -= MoveLeft;
		InputManager.OnRightEvent -= MoveRight;
	}

	private void MoveUp()
	{
		Move(Vector2.up).Forget();
	}

	private void MoveDown()
	{
		Move(Vector2.down).Forget();
	}

	private void MoveLeft()
	{
		Move(Vector2.left).Forget();
	}

	private void MoveRight()
	{
		Move(Vector2.right).Forget();
	}

	private async UniTask Move(Vector2 direction)
	{
		// Force game running if receiving input while editing
		if (Level.State == GameState.LevelEditing)
			Level.State = GameState.Running;

		if (isMoving || isDead) return;

		Vector2 startPosition = (Vector2)transform.position;
		bool isOnPlatform = Physics2D.Linecast(startPosition, startPosition, platformLayer);
		if (!isOnPlatform) return;

		isMoving = true;
		CheckPlayerOriantation(direction);
		int step = 1;
		while (true)
		{
			Vector2 destination = startPosition + (direction * step);

			await transform.DOMove(destination, 1f / moveSpeed);

			Vector2 currentPosition = (Vector2)transform.position;

			RaycastHit2D hitObstacle = Physics2D.Linecast(currentPosition, currentPosition, obstacleLayer);
			if (hitObstacle)
			{
				if (hitObstacle.collider.TryGetComponent(out Obstacle obstacle))
				{
					if (!obstacle.TryToBreak(step))
					{
						await transform.DOMove(startPosition + (direction * (step - 1)), 1f / moveSpeed);
						break;
					}
				}
			}

			if (!Physics2D.Linecast(currentPosition, currentPosition, platformLayer))
			{
				Kill().Forget();
				return;
			}

			step++;
		}

		isMoving = false;
		Level.CheckLevelCompleted();
	}

	private async UniTask Kill()
	{
		isDead = true;
		await sprite.transform.DOScale(Vector2.zero, deathScaleDownDuration);

		Level.RestartLevel();
	}

	private void CheckPlayerOriantation(Vector2 direction)
	{
		if (isFacingLeft && direction.x <= 0f || !isFacingLeft && direction.x >= 0f) return;

		isFacingLeft = !isFacingLeft;
		Vector2 scale = sprite.transform.localScale;
		scale.x *= -1f;
		sprite.transform.localScale = scale;
	}

	public void Initialization()
	{
		isMoving = false;
		isDead = false;
		isFacingLeft = true;
		sprite.transform.localScale = Vector2.one;

		transform.position = spawnPosition;
	}
}
