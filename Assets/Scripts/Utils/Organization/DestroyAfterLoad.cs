using UnityEngine;

namespace Utils
{
	public class DestroyAfterLoad : MonoBehaviour
	{
		[SerializeField] private float durationBeforeDestroy = 5f;

		private void Start() => Initialize(durationBeforeDestroy);

		public void Initialize(float duration) => Destroy(gameObject, duration);
	}
}