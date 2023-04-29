using DG.Tweening;
using System;
using UnityEngine;
using Utils;

public class GameBase : Singleton<GameBase>
{
	public event Action OnLoadingEvent;
	public event Action OnStartRunningEvent;
	public event Action OnGameOverEvent;
	public event Action OnPauseEvent;

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
					OnStartRunningEvent?.Invoke();
					break;
			}
		}
	}

	public bool IsRunning => state == GameState.Running;

	protected override void Awake()
	{
		base.Awake();

		Application.targetFrameRate = 60;

		DOTween.Init();
		DOTween.defaultAutoPlay = AutoPlay.All;

		// Disable screen dimming
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
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

	#region Audio

	public bool IsMuted => music.IsMuted;

	public void Mute()
	{
		music.Mute();
	}

	#endregion Audio
}