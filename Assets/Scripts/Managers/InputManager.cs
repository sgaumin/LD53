using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Rendering.UI;
using static Facade;

public class InputManager : MonoBehaviour
{
	public static event Action OnUpEvent;
	public static event Action OnDownEvent;
	public static event Action OnLeftEvent;
	public static event Action OnRightEvent;

	[SerializeField] private LayerMask obstacleMask;

	private Vector2 inputs;
	private bool hasReleased;

	private void Update()
	{
		if (Input.GetButtonDown("Quit") && Level.State != GameState.OnMap)
		{
			Level.LoadMap();
			return;
		}

		ListenRestart();
		ListenKeyAxis();
		ShowObstacleBubble();
	}

	private void ShowObstacleBubble()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward, 100, obstacleMask);
		if (hit.collider != null && hit.collider.TryGetComponent(out Obstacle obstacle))
		{
			if (Input.GetMouseButtonDown(0))
			{
				obstacle.ShowBubble().Forget();
			}
		}
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

		if (inputs.x > 0 && hasReleased)
		{
			hasReleased = false;
			OnRightEvent?.Invoke();
		}
		else if (inputs.x < 0 && hasReleased)
		{
			hasReleased = false;
			OnLeftEvent?.Invoke();
		}
		else if (inputs.y > 0 && hasReleased)
		{
			hasReleased = false;
			OnUpEvent?.Invoke();
		}
		else if (inputs.y < 0 && hasReleased)
		{
			hasReleased = false;
			OnDownEvent?.Invoke();
		}

		// Checking if input buttons have been released 
		if (!hasReleased && inputs == Vector2.zero)
		{
			hasReleased = true;
		}
	}
}