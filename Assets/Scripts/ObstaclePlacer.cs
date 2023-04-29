using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Utils;

public class ObstaclePlacer : MonoBehaviour
{
	public static ObstaclePlacer CurrentSelection;

	[Header("Animations")]
	[SerializeField] private float releaseDuration = 0.4f;
	[SerializeField] private Ease releaseEase = Ease.OutSine;

	[Header("Components")]
	[SerializeField] private LayerMask platformMask;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private TMP_Text multiplierText;

	private Platform currentPlatformTarget;
	private bool isBeingDragged;
	private ObstacleData data;
	private Vector2 startPosition;

	private int amount;
	public int Amount
	{
		get => amount;

		private set
		{
			amount = value;
			SetAmount(amount);
		}
	}

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		multiplierText = GetComponent<TextMeshProUGUI>();
	}

	public void Setup(ObstacleData data)
	{
		this.data = data;

		Amount = data.amount;
		spriteRenderer.sprite = data.sprite;
		startPosition = transform.position;
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
				Amount--;
			}

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
		if (CurrentSelection != null) return;

		isBeingDragged = true;
		CurrentSelection = this;
	}

	private async UniTask Release()
	{
		isBeingDragged = false;

		if (Amount <= 0)
		{
			gameObject.gameObject.SetActive(false);
		}
		else
		{
			await transform.DOMove(startPosition, releaseDuration).SetEase(releaseEase);
		}

		CurrentSelection = null;
	}
}