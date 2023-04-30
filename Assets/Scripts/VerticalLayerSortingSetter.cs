using UnityEngine;

[ExecuteAlways]
public class VerticalLayerSortingSetter : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 1000);
	}
}