using System;
using UnityEngine;
using static Facade;

public class LetterBox : MonoBehaviour, IRespawn
{
	public event Action OnPlayerCollision;

	[SerializeField] private SpriteRenderer indicator;

	public bool HasReceivedDelivery { get; private set; }

	private void Awake()
	{
		Initialization();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (HasReceivedDelivery) return;

		if (collision.TryGetComponent(out PlayerController _))
		{
			HasReceivedDelivery = true;
			indicator.gameObject.SetActive(true); // TODO: Animation of rotation

			OnPlayerCollision?.Invoke();
		}
	}

	public void Initialization()
	{
		HasReceivedDelivery = false;
		indicator.gameObject.SetActive(false);
	}
}