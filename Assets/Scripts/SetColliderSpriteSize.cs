using NaughtyAttributes;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SetColliderSpriteSize : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private BoxCollider2D boxCollider;

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void Start()
	{
		UpdateSize();
	}

	private void Update()
	{
		if (Application.isPlaying) return;

		UpdateSize();
	}

	[Button]
	private void UpdateSize()
	{
		boxCollider.size = spriteRenderer.size;
	}
}