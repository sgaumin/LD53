using UnityEngine;

namespace Utils
{
	public static class TransformExtensions
	{
		/// <summary>
		/// Create a new Transform from the given Transform with a new value for its x coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="x">The new x value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithX(this Transform t, float x)
		{
			t.position = new Vector3(x, t.position.y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with a value added to its x coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="x">The value added to the x coordinate</param>
		/// <returns>A new Transform</returns>
		public static Transform WithXAdded(this Transform t, float x)
		{
			t.position = new Vector3(t.position.x + x, t.position.y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with its x coordinate multiplied.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="x">The multiplier value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithXMultiplied(this Transform t, float x)
		{
			t.position = new Vector3(t.position.x * x, t.position.y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with a new value for its y coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="y">The new y value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithY(this Transform t, float y)
		{
			t.position = new Vector3(t.position.x, y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with a value added to its y coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="y">The value added to the y coordinate</param>
		/// <returns>A new Transform</returns>
		public static Transform WithYAdded(this Transform t, float y)
		{
			t.position = new Vector3(t.position.x, t.position.y + y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with its y coordinate multiplied.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="y">The multiplier value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithYMultiplied(this Transform t, float y)
		{
			t.position = new Vector3(t.position.x, t.position.y * y, t.position.z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with a new value for its z coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="z">The new z value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithZ(this Transform t, float z)
		{
			t.position = new Vector3(t.position.x, t.position.y, z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with a value added to its z coordinate.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="z">The value added to the z coordinate</param>
		/// <returns>A new Transform</returns>
		public static Transform WithZAdded(this Transform t, float z)
		{
			t.position = new Vector3(t.position.x, t.position.y, t.position.z + z);
			return t;
		}

		/// <summary>
		/// Create a new Transform from the given Transform with its z coordinate multiplied.
		/// </summary>
		/// <param name="t">The source Transform</param>
		/// <param name="z">The multiplier value</param>
		/// <returns>A new Transform</returns>
		public static Transform WithZMultiplied(this Transform t, float z)
		{
			t.position = new Vector3(t.position.x, t.position.y, t.position.z * z);
			return t;
		}
	}
}