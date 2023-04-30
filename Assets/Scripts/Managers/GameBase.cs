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
	public event Action OnStartLoadingEvent;
	public event Action OnEndLoadingEvent;
	public event Action OnMapEvent;
	public event Action OnLevelEditingEvent;
	public event Action OnRunningEvent;
	public event Action OnGameOverEvent;
	public event Action OnPauseEvent;

	[Header("Debug")]
	[SerializeField] private bool unlockAll;
	[SerializeField] private bool skipCinematic;

	[Header("Audio")]
	[SerializeField] private AudioExpress.AudioClip levelCompletedSound;

	[Header("Settings References")]
	[SerializeField] private LevelLoader levelLoader;
	[SerializeField] private MusicPlayer music;
	[SerializeField] private VisualEffectsHandler effectHandler;

	public LevelLoader Loader => levelLoader;
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
				case GameState.StartLoading:
					OnStartLoadingEvent?.Invoke();
					break;

				case GameState.EndLoading:
					OnEndLoadingEvent?.Invoke();
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

				case GameState.OnMap:
					OnMapEvent?.Invoke();
					break;
			}
		}
	}

	public bool IsRunning => state == GameState.LevelEditing || state == GameState.Running;

	public int UnlockedLevelIndex
	{
		get
		{
			return PlayerPrefs.GetInt("UnlockedLevelIndex");
		}

		private set
		{
			PlayerPrefs.SetInt("UnlockedLevelIndex", value);
			PlayerPrefs.Save();
		}
	}

	protected override void Awake()
	{
		base.Awake();

		if (unlockAll) UnlockedLevelIndex = 100;

		Application.targetFrameRate = 60;

		DOTween.Init();
		DOTween.defaultAutoPlay = AutoPlay.All;

		// Disable screen dimming
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		State = GameState.NotSet;
	}

	private void Start()
	{
		if (skipCinematic)
		{
			levelLoader.LoadMapNavigation();
		}
		else
		{
			levelLoader.LoadCinematic();
		}
	}

	public void LoadMap()
	{
		levelLoader.LoadMapNavigation();
	}

	public void RespawnLevel()
	{
		levelLoader.RespawnCurrentLevel();
	}

	public void RestartLevel()
	{
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

			if (UnlockedLevelIndex == Loader.CurrentIndex)
				UnlockedLevelIndex++;

			LoadMap();
		}
	}

	#region Level Loading

	public void LoadLevelIndex(int index) => levelLoader.LoadLevelIndex(index);
	public void ReloadScene() => levelLoader.Reload();

	public void LoadNextScene() => levelLoader.LoadNextScene();

	public void LoadPrevious() => levelLoader.LoadPrevious();

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