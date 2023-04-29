using System;
using UnityEngine;
using static Facade;

public class LetterBox : MonoBehaviour, IRespawn
{
	public event Action OnPlayerCollision;

	[SerializeField] private SpriteRenderer indicator;

	private bool haReceivedDelivery;

	private void Awake()
	{
		Register();
		Initialization();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (haReceivedDelivery) return;

		if (collision.TryGetComponent(out PlayerController _))
		{
			haReceivedDelivery = true;
			indicator.gameObject.SetActive(true); // TODO: Animation of rotation

			OnPlayerCollision?.Invoke();
		}
	}

	public void Register()
	{
		Level.Register(this);
	}

	public void Initialization()
	{
		haReceivedDelivery = false;
		indicator.gameObject.SetActive(false);
	}
}