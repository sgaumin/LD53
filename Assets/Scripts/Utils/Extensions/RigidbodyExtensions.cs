using UnityEngine;

namespace Utils
{
	public static class RigidbodyExtensions
	{
		public static bool Freeze(this Rigidbody2D rigidbody2D)
		{
			if (rigidbody2D == null)
				return false;

			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.angularVelocity = 0;
			rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
			return true;
		}
	}
}