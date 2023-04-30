using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class DeliveryManager : MonoBehaviour
{
	public static DeliveryManager Instance;

	[SerializeField] private Delivery deliveryPrefab;

	private List<Delivery> deliveries = new List<Delivery>();
	private LetterBox[] letterBoxes;

	public bool AllDelivered => letterBoxes.All(x => x.HasReceivedDelivery);

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		letterBoxes = FindObjectsOfType<LetterBox>();
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
		Delivery delivery = deliveries.Where(x => x.gameObject.activeSelf).Random();

		// TODO: Make animation (package goes slowly to box, box shows red indicator, etc...)

		delivery.gameObject.SetActive(false);
	}
}
