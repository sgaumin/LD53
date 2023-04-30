using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using Utils;
using static Facade;

public class PlayerController : MonoBehaviour, IRespawn
{
	[SerializeField] private float moveSpeed;

	[Header("Movements")]
	[SerializeField] private float speedMultiplier = 1.5f;

	[Header("Animations")]
	[SerializeField] private float deathScaleDownDuration = 1f;

	[Space]
	[SerializeField] private float idleAnimationDuration = 0.2f;
	[SerializeField] private float idleAnimationFactor = 0.2f;
	[SerializeField] private Ease idleAnimationEase = Ease.OutSine;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip fallingSound;
	[SerializeField] private AudioExpress.AudioClip footStepSound;
	[SerializeField] private AudioExpress.AudioClip spawnSound;

	[Header("References")]
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private VerticalLayerSortingSetter sorting;
	[SerializeField] private LayerMask platformLayer;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private LayerMask wallLayer;

	private Vector2 spawnPosition;
	private bool isDead;
	private bool isMoving;
	private bool isFacingLeft = true;
	private CancellationTokenSource source;

	public bool CanInteract { get; private set; }

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
		source.SafeDispose();

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

		if (isMoving || isDead || !CanInteract) return;

		Vector2 startPosition = (Vector2)transform.position;
		if (GetCurrentPlatform() == null) return;

		isMoving = true;
		CheckPlayerOriantation(direction);
		int step = 1;
		float currentMoveSpeed = moveSpeed;

		while (true)
		{
			Platform startPlatform = GetCurrentPlatform();

			Vector2 destination = startPosition + (direction * step);

			footStepSound.Play();

			// If destination is wall stops
			if (IsThereWallAt(destination)) break;

			await transform.DOMove(destination, 1f / currentMoveSpeed).SetEase(Ease.Linear);

			Vector2 currentPosition = (Vector2)transform.position;

			RaycastHit2D hitObstacle = Physics2D.Linecast(currentPosition, currentPosition, obstacleLayer);
			if (hitObstacle)
			{
				if (hitObstacle.collider.TryGetComponent(out Obstacle obstacle))
				{
					if (!obstacle.TryToBreak(step))
					{
						if (!obstacle.CanStandOnIt)
						{
							obstacle.PlayContactSound();

							Level.GenerateImpulse();

							await transform.DOMove(startPosition + (direction * (step - 1)), 1f / (moveSpeed * 2f));
						}
						else
						{
							obstacle.PlayStandOnSound();

							if (startPlatform != null && startPlatform.IsFalling)
								startPlatform.Fall().Forget();
						}
						break;
					}
					else
					{
						Level.GenerateImpulse();
					}
				}
			}
			else
			{
				if (startPlatform != null && startPlatform.IsFalling)
					startPlatform.Fall().Forget();
			}

			Platform platform = GetCurrentPlatform();
			if ((platform == null && !IsOnGround()) || (platform != null && platform.HasFallen))
			{
				Kill().Forget();
				return;
			}

			currentMoveSpeed *= speedMultiplier;
			step++;
		}

		isMoving = false;
		Level.CheckLevelCompleted();
	}

	private Platform GetCurrentPlatform()
	{
		Platform platform = null;
		RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position, platformLayer);
		hit.collider?.TryGetComponent(out platform);
		return platform;
	}

	private bool IsOnGround()
	{
		return Physics2D.Linecast((Vector2)transform.position, (Vector2)transform.position + 0.1f * Vector2.right, groundLayer);
	}

	private bool IsThereWallAt(Vector2 location)
	{
		return Physics2D.Linecast(location, location + 0.1f * Vector2.right, wallLayer);
	}

	private async UniTask PlayIdle()
	{
		source = source.SafeReset();
		CancellationToken token = source.Token;
		while (!token.IsCancellationRequested)
		{
			sprite.transform.DOKill();
			await sprite.transform.DOScaleY(idleAnimationFactor, idleAnimationDuration).SetEase(idleAnimationEase).ToUniTask(cancellationToken: token);
			await UniTask.Delay(200, cancellationToken: token);
			await sprite.transform.DOScaleY(1f, idleAnimationDuration).SetEase(idleAnimationEase).ToUniTask(cancellationToken: token); ;
			await UniTask.Delay(500, cancellationToken: token);
		}
	}

	private async UniTask Kill()
	{
		isDead = true;
		CanInteract = false;

		fallingSound.Play();

		source.Cancel();
		sprite.transform.DOKill();

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


		Spawn().Forget();
	}

	private async UniTask Spawn()
	{
		sorting.enabled = false;
		sprite.sortingOrder = 10000;
		transform.position = spawnPosition.WithY(8f);

		transform.DOKill();
		await transform.DOMoveY(spawnPosition.y, 0.5f).SetEase(Ease.Linear);

		spawnSound.Play();
		Level.GenerateImpulse();
		sorting.enabled = true;

		CanInteract = true;

		PlayIdle().Forget();
	}
}
