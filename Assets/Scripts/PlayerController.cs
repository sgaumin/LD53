using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using Utils;
using static Facade;

public class PlayerController : MonoBehaviour, IRespawn
{
	public Action OnSpawningStartEvent;
	public Action OnSpawningEndEvent;

	[SerializeField] private float moveSpeed;

	[Header("Movements")]
	[SerializeField] private float speedMultiplier = 1.5f;

	[Header("Animations")]
	[SerializeField] private float deathScaleDownDuration = 1f;

	[Space]
	[SerializeField] private float idleAnimationDuration = 0.2f;
	[SerializeField] private float idleAnimationFactor = 0.2f;
	[SerializeField] private Ease idleAnimationEase = Ease.OutSine;

	[Header("Effects")]
	[SerializeField] private GameObject stepEffect;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip fallingSound;
	[SerializeField] private AudioExpress.AudioClip footStepSound;
	[SerializeField] private AudioExpress.AudioClip spawnSound;

	[Header("References")]
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private PlayerDeliveryHolder deliveryHolder;
	[SerializeField] private LayerMask platformLayer;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private LayerMask wallLayer;

	private Vector2 spawnPosition;
	private bool isDead;
	private bool isMoving;
	private bool isFacingLeft = true;
	private CancellationTokenSource idleAnimationCancellationSource;

	public PlayerDeliveryHolder DeliveryHolder => deliveryHolder;

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
		idleAnimationCancellationSource.SafeDispose();

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
		// Caching input
		while (isMoving)
			await UniTask.Yield();

		// Force game running if receiving input while editing
		if (Level.State == GameState.LevelEditing)
			Level.State = GameState.Running;

		if (isDead || !CanInteract) return;

		Vector2 startPosition = (Vector2)transform.position;
		isMoving = true;
		CheckPlayerOriantation(direction);
		int step = 1;
		float currentMoveSpeed = moveSpeed;

		while (true)
		{
			Platform currentPlatform = GetCurrentPlatform();

			Vector2 destination = startPosition + (direction * step);

			footStepSound.Play();

			// If destination is wall stops
			if (IsThereWallAt(destination)) break;

			SpawnEffect();
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
							SpawnEffect();

							await transform.DOMove(startPosition + (direction * (step - 1)), 1f / (moveSpeed * 2f));
						}
						else
						{
							obstacle.PlayStandOnSound();

							if (currentPlatform != null && currentPlatform.IsFalling)
								currentPlatform.Fall().Forget();
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
				if (currentPlatform != null && currentPlatform.IsFalling)
					currentPlatform.Fall().Forget();
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
		CheckLevelCompleted().Forget();
	}

	private async UniTask CheckLevelCompleted()
	{
		if (Level.IsLevelCompleted())
		{
			CanInteract = false;

			idleAnimationCancellationSource.Cancel();
			sprite.transform.DOKill();

			await UniTask.Delay(1000);

			Level.PlayerLevelCompletion();
			SpawnEffect();
			Level.GenerateImpulse();

			await transform.DOMoveY(8f, 0.5f).SetEase(Ease.Linear);

			Level.LoadMap();
		}
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
		idleAnimationCancellationSource = idleAnimationCancellationSource.SafeReset();
		CancellationToken token = idleAnimationCancellationSource.Token;
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

		idleAnimationCancellationSource.Cancel();
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
		sprite.sortingLayerName = "OnTop";
		transform.position = spawnPosition.WithY(8f);
		OnSpawningStartEvent?.Invoke();

		transform.DOKill();
		await transform.DOMoveY(spawnPosition.y, 0.5f).SetEase(Ease.Linear);

		SpawnEffect();

		spawnSound.Play();
		Level.GenerateImpulse();
		sprite.sortingLayerName = "Player";
		PlayIdle().Forget();

		CanInteract = true;
		OnSpawningEndEvent?.Invoke();
	}

	private void SpawnEffect()
	{
		GameObject effect = Instantiate(stepEffect);
		effect.transform.position = transform.position;
	}
}
