using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Utils;
using static Facade;

public class ObstaclePlacer : MonoBehaviour, IRespawn
{
	public static ObstaclePlacer CurrentSelection;
	public static int TotalAmount = 0;

	public static Action<int> OnTotalAmountChangeEvent;

	[Header("Animations")]
	[SerializeField] private float releaseDuration = 0.4f;
	[SerializeField] private Ease releaseEase = Ease.OutSine;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip clickSound;

	[Header("Components")]
	[SerializeField] private LayerMask platformMask;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private TMP_Text multiplierText;

	private Platform currentPlatformTarget;
	private bool isBeingDragged;
	private ObstacleData data;
	private Vector2 startLocalPosition;

	private int amount;
	public int Amount
	{
		get => amount;

		private set
		{
			amount = value;
			SetAmount(amount);

			if (!PlacementManager.ManualMode)
			{
				if (TotalAmount <= 0)
				{
					Level.State = GameState.Running;
				}
				else if (Level.State != GameState.LevelEditing)
				{
					Level.State = GameState.LevelEditing;
				}
			}
			else
			{
				Level.State = GameState.Running;
			}
		}
	}

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		multiplierText = GetComponent<TextMeshProUGUI>();
	}

	private void Awake()
	{
		Level.OnRunningEvent += ResetCachedParameters;
	}

	private void OnDestroy()
	{
		Level.OnRunningEvent -= ResetCachedParameters;
	}

	public void Setup(ObstacleData data)
	{
		this.data = data;
		startLocalPosition = transform.localPosition;
		Initialization();
	}

	private void ResetCachedParameters()
	{
		if (TotalAmount > 0)
			TotalAmount = 0;
	}

	public void Initialization()
	{
		TotalAmount += data.amount;
		Amount = data.amount;
		spriteRenderer.sprite = data.sprite;
		transform.localPosition = startLocalPosition;

		if (!gameObject.activeSelf)
			gameObject.SetActive(true);
	}

	public void SetAmount(int value)
	{
		multiplierText.SetText(value <= 1 ? "" : value.ToString());
	}

	private void Update()
	{
		if (CurrentSelection != this) return;

		if (!isBeingDragged) return;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButtonUp(0))
		{
			if (currentPlatformTarget != null && currentPlatformTarget.SetObstacle(data))
			{
				TotalAmount--;
				Amount--;

				OnTotalAmountChangeEvent?.Invoke(TotalAmount);
			}

			clickSound.Play();
			CleanCachedTarget();
			Release().Forget();
		}
		else
		{
			RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward, 100, platformMask);
			if (hit.collider != null && hit.collider.TryGetComponent(out Platform platform))
			{
				if (platform != currentPlatformTarget)
				{
					CleanCachedTarget();

					currentPlatformTarget = platform;
					currentPlatformTarget.ShowOverlay();
				}
			}
			else
			{
				CleanCachedTarget();
			}
		}

		transform.position = mousePosition.WithZ(0f);
	}

	private void CleanCachedTarget()
	{
		if (currentPlatformTarget != null)
		{
			currentPlatformTarget.HideOverlay();
			currentPlatformTarget = null;
		}
	}

	private void OnMouseDown()
	{
		if (Level.State != GameState.LevelEditing) return;

		if (CurrentSelection != null) return;

		isBeingDragged = true;
		CurrentSelection = this;
		clickSound.Play();
	}

	private async UniTask Release()
	{
		isBeingDragged = false;

		if (Amount <= 0)
		{
			gameObject.SetActive(false);
		}
		else
		{
			await transform.DOLocalMove(startLocalPosition, releaseDuration).SetEase(releaseEase);
		}

		CurrentSelection = null;
	}
}