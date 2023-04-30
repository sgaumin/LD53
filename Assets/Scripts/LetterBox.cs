using System;
using UnityEngine;
using static Facade;
using Random = UnityEngine.Random;

public class LetterBox : MonoBehaviour, IRespawn
{
	public event Action OnPlayerCollision;

	[SerializeField] private AudioExpress.AudioClip deliverySound;
	[SerializeField] private SpriteRenderer indicator;

	public bool HasReceivedDelivery { get; private set; }

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (HasReceivedDelivery) return;

		if (collision.TryGetComponent(out PlayerController player) && player.CanInteract)
		{
			HasReceivedDelivery = true;
			indicator.gameObject.SetActive(true); // TODO: Animation of rotation

			deliverySound.Play();

			OnPlayerCollision?.Invoke();
		}
	}

	public void Initialization()
	{
		HasReceivedDelivery = false;
		indicator.gameObject.SetActive(false);
	}
}