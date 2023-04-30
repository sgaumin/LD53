using AudioExpress;
using DG.Tweening;
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

	private void Flip()
	{
		Vector2 scale = transform.localScale;
		scale.x *= -1f;
		transform.localScale = scale;
	}

	public void Initialization()
	{
		if (HasBeenEdited) Destroy(gameObject);

		if (gameObject.activeSelf) return;

		gameObject.SetActive(true);
		transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack);
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