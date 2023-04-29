using UnityEngine;

namespace Utils
{
	public static class Vector2Extensions
	{
		public static Vector2 WithX(this Vector2 vector, float x) => new Vector2(x, vector.y);

		public static Vector2 WithY(this Vector2 vector, float y) => new Vector2(vector.x, y);

		public static Vector2 PlusX(this Vector2 vector, float plusX) => new Vector2(vector.x + plusX, vector.y);

		public static Vector2 PlusY(this Vector2 vector, float plusY) => new Vector2(vector.x, vector.y + plusY);

		public static Vector2 TimesX(this Vector2 vector, float timesX) => new Vector2(vector.x * timesX, vector.y);

		public static Vector2 TimesY(this Vector2 vector, float timesY) => new Vector2(vector.x, vector.y * timesY);

		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tx = v.x;
			float ty = v.y;
			v.x = (cos * tx) - (sin * ty);
			v.y = (sin * tx) + (cos * ty);
			return v;
		}
	}
}