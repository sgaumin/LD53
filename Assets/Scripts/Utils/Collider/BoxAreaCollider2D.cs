using UnityEngine;

namespace Utils
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(PolygonCollider2D))]
	public class BoxAreaCollider2D : MonoBehaviour
	{
		[SerializeField] private float width = 14f;
		[SerializeField] private float height = 6f;
		[SerializeField] private float size = 2f;

		[Header("References")]
		[SerializeField] private PolygonCollider2D polygonCollider2D;

		private Vector2 point1;
		private Vector2 point2;
		private Vector2 point3;
		private Vector2 point4;
		private Vector2 point5;
		private Vector2 point6;
		private Vector2 point7;
		private Vector2 point8;

		private void Update()
		{
			polygonCollider2D.points = new Vector2[] { };

			// Outside Points
			point1 = new Vector2(-width / 2 - size, height / 2 + size);
			point2 = new Vector2(width / 2 + size, height / 2 + size);
			point3 = new Vector2(width / 2 + size, -height / 2 - size);
			point4 = new Vector2(-width / 2 - size, -height / 2 - size);

			// Inside Points
			point5 = new Vector2(-width / 2, height / 2);
			point6 = new Vector2(width / 2, height / 2);
			point7 = new Vector2(width / 2, -height / 2);
			point8 = new Vector2(-width / 2, -height / 2);

			polygonCollider2D.points = new Vector2[] { point1, point2, point3, point4, point1, point5, point6, point7, point8, point5 };
		}
	}
}