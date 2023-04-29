using UnityEngine;

namespace Utils
{
	public class HideCursor : MonoBehaviour
	{
		private void Start()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}