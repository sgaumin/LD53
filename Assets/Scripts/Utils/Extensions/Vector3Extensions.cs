using UnityEngine;

namespace Utils
{
	public static class Vector3Extensions
	{
		public static Vector3 WithX(this Vector3 vector, float x) => new Vector3(x, vector.y, vector.z);

		public static Vector3 WithY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);

		public static Vector3 WithZ(this Vector3 vector, float z) => new Vector3(vector.x, vector.y, z);

		public static Vector3 PlusX(this Vector3 vector, float plusX) => new Vector3(vector.x + plusX, vector.y, vector.z);

		public static Vector3 PlusY(this Vector3 vector, float plusY) => new Vector3(vector.x, vector.y + plusY, vector.z);

		public static Vector3 PlusZ(this Vector3 vector, float plusZ) => new Vector3(vector.x, vector.y, vector.z + plusZ);

		public static Vector3 TimesX(this Vector3 vector, float timesX) => new Vector3(vector.x * timesX, vector.y, vector.z);

		public static Vector3 TimesY(this Vector3 vector, float timesY) => new Vector3(vector.x, vector.y * timesY, vector.z);

		public static Vector3 TimesZ(this Vector3 vector, float timesZ) => new Vector3(vector.x, vector.y, vector.z * timesZ);
	}
}