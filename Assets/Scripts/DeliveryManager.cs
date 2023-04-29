using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class DeliveryManager : MonoBehaviour
{
	[SerializeField] private Delivery deliveryPrefab;

	private List<Delivery> deliveries = new List<Delivery>();

	private void Start()
	{
		LetterBox[] letterBoxes = FindObjectsOfType<LetterBox>();
		foreach (LetterBox letterBox in letterBoxes)
		{
			letterBox.OnPlayerCollision += Deliver;
		}

		int count = letterBoxes.Count();
		for (int i = 0; i < count; i++)
		{
			Delivery delivery = Instantiate(deliveryPrefab, transform);
			deliveries.Add(delivery);
		}
	}

	private void Deliver()
	{
		Delivery delivery = deliveries.Random();

		// TODO: Make animation (package goes slowly to box, box shows red indicator, etc...)

		delivery.gameObject.SetActive(false);
	}
}