using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Facade;

public class LevelDesigner : MonoBehaviour
{
	[Serializable]
	private struct LevelEvent
	{
		public GameState type;
		public int restartCount;
		public UnityEvent levelEvent;
	}

	[SerializeField] private bool flipPlayerAtStart; // TODO

	[Header("Level Events")]
	[SerializeField] private List<LevelEvent> events = new List<LevelEvent>();

	[Header("References")]
	[SerializeField] private TextMeshPro instructions;

	private int currentRunningCount;

	private void Awake()
	{
		Level.OnLevelEditingEvent += TriggerLevelEvent;
		Level.OnRunningEvent += TriggerLevelEvent;
	}

	private void OnDestroy()
	{
		Level.OnLevelEditingEvent -= TriggerLevelEvent;
		Level.OnRunningEvent -= TriggerLevelEvent;
	}

	public void ShowInstruction(string text)
	{
		instructions.SetText(text);
		instructions.UpdateVertexData();
		instructions.ForceMeshUpdate();
	}

	private void TriggerLevelEvent()
	{
		foreach (var e in events)
		{
			if (Level.State != e.type) continue;

			if (currentRunningCount == e.restartCount)
			{
				e.levelEvent?.Invoke();
			}
		}

		if (Level.State == GameState.Running)
			currentRunningCount++;
	}
}