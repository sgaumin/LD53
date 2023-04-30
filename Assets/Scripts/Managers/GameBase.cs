using AudioExpress;
using Cysharp.Threading.Tasks;
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

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip levelCompletedSound;

	[Header("Settings References")]
	[SerializeField] private LevelLoader levelLoader;
	[SerializeField] private MusicPlayer music;
	[SerializeField] private VisualEffectsHandler effectHandler;

	public VisualEffectsHandler Effects => effectHandler;


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
		levelLoader.LoadLevelIndex(0).Forget();

		State = GameState.LevelEditing;
	}

	public void RespawnLevel()
	{
		levelLoader.RespawnCurrentLevel();
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
		if (DeliveryManager.Instance.AllDelivered)
		{
			levelCompletedSound.Play();
			levelLoader.LoadNext();
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