using UnityEngine;
using static Facade;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private AudioExpress.AudioClip mapMusic;
	[SerializeField] private AudioExpress.AudioClip editingMusic;
	[SerializeField] private AudioExpress.AudioClip gameMusic;

	private void Awake()
	{
		Level.OnLevelEditingEvent += PlayLevelEditingMusic;
		Level.OnRunningEvent += PlayGameMusic;
	}

	private void OnDestroy()
	{
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
		Stop();
		mapMusic.Play();
	}

	private void PlayLevelEditingMusic()
	{
		Stop();
		editingMusic.Play();
	}

	private void PlayGameMusic()
	{
		Stop();
		gameMusic.Play();
	}
}