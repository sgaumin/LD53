using UnityEngine;

public class Delivery : MonoBehaviour, IRespawn
{
	[SerializeField] private float smoothFollow = 0.1f;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private PlayerController player;
	private PlayerDeliveryHolder holder;
	private Transform target;

	private void Awake()
	{
		player = FindObjectOfType<PlayerController>();
		player.OnSpawningStartEvent += SetLayerOnTop;
		player.OnSpawningEndEvent += SetLayerPlayer;

		SetLayerOnTop();

		Initialization();
	}

	private void OnDestroy()
	{
		player.OnSpawningStartEvent -= SetLayerOnTop;
		player.OnSpawningEndEvent -= SetLayerPlayer;
	}

	private void Start()
	{
		GetTarget();
	}

	private void SetLayerPlayer() => spriteRenderer.sortingLayerName = "Player";
	private void SetLayerOnTop() => spriteRenderer.sortingLayerName = "OnTop";

	private void GetTarget()
	{
		if (target != null) return;

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