using UnityEngine;
using static Facade;

public class InputManager : MonoBehaviour
{
	private const KeyCode RESTART = KeyCode.R;
	private readonly KeyCode[] QWERTY = new KeyCode[4] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
	private readonly KeyCode[] AZERTY = new KeyCode[4] { KeyCode.Z, KeyCode.S, KeyCode.Q, KeyCode.D };

	public static Vector2 InputDirection;

	private KeyCode[] _codes;

	private void Start()
	{
		_codes = GetKeyCodes();
	}

	private void Update()
	{
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_EDITOR
		if (Input.GetButtonDown("Quit"))
		{
			Level.Quit();
		}
		if (Input.GetButtonDown("Mute"))
		{
			Level.Mute();
		}
		if (Input.GetButtonDown("Fullscreen"))
		{
			Screen.fullScreen = !Screen.fullScreen;
		}
#endif

		ListenRestart();
		ListenKeyAxis();
	}

	private static void ListenRestart()
	{
		if (!Level.IsRunning && Input.GetKeyDown(RESTART))
		{
			Level.ReloadScene();
		}
	}

	private void ListenKeyAxis()
	{
		if (!Level.IsRunning) return;

		if (Input.GetKey(_codes[0]) || Input.GetKey(KeyCode.UpArrow)) // UP
		{
			InputDirection.y = 1;
		}
		else if (Input.GetKey(_codes[1]) || Input.GetKey(KeyCode.DownArrow)) // DOWN
		{
			InputDirection.y = -1;
		}
		else
		{
			InputDirection.y = 0;
		}

		if (Input.GetKey(_codes[2]) || Input.GetKey(KeyCode.LeftArrow)) // LEFT
		{
			InputDirection.x = -1;
		}
		else if (Input.GetKey(_codes[3]) || Input.GetKey(KeyCode.RightArrow)) // RIGHT
		{
			InputDirection.x = 1;
		}
		else
		{
			InputDirection.x = 0;
		}
	}

	private KeyCode[] GetKeyCodes()
	{
		string language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
		switch (language)
		{
			case "fr":
				return AZERTY;

			default:
				return QWERTY;
		}
	}
}