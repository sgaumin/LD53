using UnityEngine;
using static Facade;

public class Delivery : MonoBehaviour, IRespawn
{
	[SerializeField] private float smoothFollow = 0.1f;

	private PlayerController player;

	private void Awake()
	{
		Initialization();
	}

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, player.transform.position, smoothFollow);
	}

	public void Initialization()
	{
		if (!gameObject.activeSelf)
			gameObject.SetActive(true);
	}
}