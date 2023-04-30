﻿using AudioExpress;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameBase : Singleton<GameBase>
{
	public event Action OnLoadingEvent;
	public event Action OnLevelEditingEvent;
	public event Action OnRunningEvent;
	public event Action OnGameOverEvent;
	public event Action OnPauseEvent;

	[Header("Gameplay References")]
	[SerializeField] private DeliveryManager deliveryManager;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip levelCompletedSound;

	[Header("Settings References")]
	[SerializeField] private LevelLoader levelLoader;
	[SerializeField] private MusicPlayer music;
	[SerializeField] private VisualEffectsHandler effectHandler;

	private GameState state;

	public GameState State
	{
		get => state;
		set
		{
			state = value;

			switch (value)
			{
				case GameState.Loading:
					OnLoadingEvent?.Invoke();
					break;

				case GameState.GameOver:
					OnGameOverEvent?.Invoke();
					break;

				case GameState.Pause:
					OnPauseEvent?.Invoke();
					break;

				case GameState.Running:
					OnRunningEvent?.Invoke();
					break;

				case GameState.LevelEditing:
					OnLevelEditingEvent?.Invoke();
					break;
			}
		}
	}

	public bool IsRunning => state == GameState.LevelEditing || state == GameState.Running;

	protected override void Awake()
	{
		base.Awake();

		Application.targetFrameRate = 60;

		DOTween.Init();
		DOTween.defaultAutoPlay = AutoPlay.All;

		// Disable screen dimming
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	private void Start()
	{
		levelLoader.LoadLevelIndex(0);

		State = GameState.LevelEditing;
	}

	public void RestartLevel()
	{
		State = GameState.LevelEditing;

		foreach (IRespawn respawner in FindObjectsOfType<MonoBehaviour>(true).OfType<IRespawn>().ToList())
		{
			respawner.Initialization();
		}
	}

	public void CheckLevelCompleted()
	{
		if (deliveryManager.AllDelivered)
		{
			levelCompletedSound.Play();
			ReloadScene();
		}
	}

	#region Level Loading

	public void ReloadScene() => levelLoader.Reload();

	public void LoadNextScene() => levelLoader.LoadNextScene();

	public void LoadPrevious() => levelLoader.LoadPrevious();

	public void LoadMenu() => levelLoader.LoadMenu();

	public void LoadSceneTransition(SceneLoading loading) => levelLoader.LoadSceneTransition(loading);

	public void Quit() => levelLoader.Quit();

	#endregion Level Loading

	#region Post-Processing and Effects

	public void GenerateImpulse()
	{
		effectHandler.GenerateImpulse();
	}

	#endregion Post-Processing and Effects
}