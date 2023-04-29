using DG.Tweening;
using UnityEngine;
using static Facade;

public class Obstacle : MonoBehaviour, IRespawn
{
	[SerializeField] private int stepCountToBreak;

	private void Awake()
	{
		Register();
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

	public void Register()
	{
		Level.Register(this);
	}

	public void Initialization()
	{
		if (gameObject.activeSelf) return;

		gameObject.SetActive(true);
		transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBack);
	}

	public bool TryToBreak(int currentStepCount)
	{
		bool check = currentStepCount >= stepCountToBreak;
		if (check)
		{
			gameObject.SetActive(false);
		}

		return check;
	}
}