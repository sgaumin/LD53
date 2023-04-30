using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour, IRespawn
{
	[SerializeField] private int stepCountToBreak;
	[SerializeField] private bool canStandOnIt;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip breakingSound;
	[SerializeField] private AudioExpress.AudioClip editingSound;
	[SerializeField] private AudioExpress.AudioClip contactSound;
	[SerializeField] private AudioExpress.AudioClip specialSound;

	[Header("References")]
	[SerializeField] private GameObject bubble;
	[SerializeField] private TMP_Text bubbleText;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private bool isShowingBubble;

	public bool HasBeenEdited { get; set; }
	public bool CanStandOnIt => canStandOnIt;

	private void Awake()
	{
		Initialization();
	}

	private void OnEnable()
	{
		if (Random.value >= 0.5f) Flip();
	}

	[Button]
	public async UniTask ShowBubble()
	{
		if (isShowingBubble || stepCountToBreak > 9) return;

		isShowingBubble = true;
		bubble.SetActive(true);
		bubble.transform.DOKill();

		bubbleText.SetText($">{stepCountToBreak - 1}");

		await bubble.transform.DOScaleY(1f, 0.3f).From(0f).SetEase(Ease.OutBack);
		await UniTask.Delay(500);
		await bubble.transform.DOScaleY(0f, 0.3f).From(1f).SetEase(Ease.InBack);

		bubble.SetActive(false);
		isShowingBubble = false;
	}

	private void Flip()
	{
		Vector2 scale = spriteRenderer.transform.localScale;
		scale.x *= -1f;
		spriteRenderer.transform.localScale = scale;
	}

	public void Initialization()
	{
		if (HasBeenEdited) Destroy(gameObject);

		if (gameObject.activeSelf) return;

		gameObject.SetActive(true);
		transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack);

		isShowingBubble = false;
		bubble.transform.DOKill();
		bubble.SetActive(false);
	}

	public bool TryToBreak(int currentStepCount)
	{
		bool check = currentStepCount >= stepCountToBreak;
		if (check)
		{
			breakingSound.Play();
			gameObject.SetActive(false);
		}

		return check;
	}

	public void PlayStandOnSound()
	{
		specialSound?.Play();
	}

	public void PlayContactSound()
	{
		contactSound?.Play();
	}

	public void PlayEditingSound()
	{
		editingSound.Play();
	}
}