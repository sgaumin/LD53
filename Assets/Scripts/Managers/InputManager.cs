using System;
using UnityEngine;
using static Facade;

public class InputManager : MonoBehaviour
{
	public static event Action OnUpEvent;
	public static event Action OnDownEvent;
	public static event Action OnLeftEvent;
	public static event Action OnRightEvent;

	private Vector2 inputs;

	private void Update()
	{
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_EDITOR
		if (Input.GetButtonDown("Quit"))
		{
			Level.Quit();
		}
#endif

		ListenRestart();
		ListenKeyAxis();
	}

	private static void ListenRestart()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Level.RespawnLevel();
		}
	}

	private void ListenKeyAxis()
	{
		if (!Level.IsRunning) return;

		inputs.x = Input.GetAxisRaw("Horizontal");
		inputs.y = Input.GetAxisRaw("Vertical");

		if (inputs.x > 0) OnRightEvent?.Invoke();
		else if (inputs.x < 0) OnLeftEvent?.Invoke();
		else if (inputs.y > 0) OnUpEvent?.Invoke();
		else if (inputs.y < 0) OnDownEvent?.Invoke();
	}
}