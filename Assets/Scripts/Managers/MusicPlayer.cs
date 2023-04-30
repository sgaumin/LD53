using UnityEngine;
using static Facade;

public class MusicPlayer : MonoBehaviour
{
	private enum CurrentMusicPlying
	{
		Map,
		Editing,
		Game,
		None
	}

	[SerializeField] private AudioExpress.AudioClip mapMusic;
	[SerializeField] private AudioExpress.AudioClip editingMusic;
	[SerializeField] private AudioExpress.AudioClip gameMusic;

	private CurrentMusicPlying currentType = CurrentMusicPlying.None;

	private void Awake()
	{
		Level.OnMapEvent += PlayMapMusic;
		Level.OnLevelEditingEvent += PlayLevelEditingMusic;
		Level.OnRunningEvent += PlayGameMusic;
	}

	private void OnDestroy()
	{
		Level.OnMapEvent -= PlayMapMusic;
		Level.OnLevelEditingEvent -= PlayLevelEditingMusic;
		Level.OnRunningEvent -= PlayGameMusic;
	}

	public void Stop()
	{
		mapMusic.Stop();
		editingMusic.Stop();
		gameMusic.Stop();
	}

	private void PlayMapMusic()
	{
		if (currentType == CurrentMusicPlying.Map) return;

		currentType = CurrentMusicPlying.Map;

		Stop();
		mapMusic.Play();
	}

	private void PlayLevelEditingMusic()
	{
		if (currentType == CurrentMusicPlying.Editing) return;

		currentType = CurrentMusicPlying.Editing;

		Stop();
		editingMusic.Play();
	}

	private void PlayGameMusic()
	{
		if (currentType == CurrentMusicPlying.Game) return;

		currentType = CurrentMusicPlying.Game;

		Stop();
		gameMusic.Play();
	}
}