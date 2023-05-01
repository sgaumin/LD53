using UnityEngine;
using static Facade;

public class Delivery : MonoBehaviour, IRespawn
{
	[SerializeField] private float smoothFollow = 0.1f;

	private PlayerDeliveryHolder holder;
	private Transform target;

	private void Awake()
	{
		Initialization();
	}

	private void Start()
	{
		GetTarget();
	}

	private void GetTarget()
	{
		if (target != null) return;

		PlayerController player = FindObjectOfType<PlayerController>();
		holder = player.DeliveryHolder;
		target = holder.Get();
	}

	public void ReleaseTarget()
	{
		if (target != null)
		{
			holder.Release(target);
			target = null;
		}
	}

	private void FixedUpdate()
	{
		if (target != null)
		{
			transform.position = Vector3.Lerp(transform.position, target.position, smoothFollow);
		}
	}

	private void OnDisable()
	{
		ReleaseTarget();
	}

	public void Initialization()
	{
		if (!gameObject.activeSelf)
			gameObject.SetActive(true);

		GetTarget();
	}
}