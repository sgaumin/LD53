using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
	public const string MASTER_VOLUME = "masterVolume";
	public const string MUSIC_VOLUME = "musicVolume";
	public const string MUSIC_LOWPASS = "musicLowPass";

	[Header("References")]
	[SerializeField] private AudioMixer mixer;
	[SerializeField] private AudioSource audioSource;

	private float masterVolume;
	private float musicVolume;
	private float musicLowPass;

	public bool IsMuted => masterVolume == -80f;

	public void FadIn()
	{
		audioSource.DOKill();
		audioSource.DOFade(1f, 0.2f);
	}

	public void FadOut()
	{
		audioSource.DOKill();
		audioSource.DOFade(0f, 2f);
	}

	public void UpdateSceneMasterVolume(float percentage)
	{
		masterVolume = Mathf.Lerp(-80f, 0f, percentage);
		mixer.SetFloat(MASTER_VOLUME, masterVolume);
	}

	public void UpdateSceneMusicVolume(float percentage)
	{
		musicVolume = Mathf.Lerp(-80f, 0f, percentage);
		mixer.SetFloat(MUSIC_VOLUME, musicVolume);
	}

	public void UpdateSceneMusicLowPass(float percentage)
	{
		musicLowPass = Mathf.Lerp(800f, 22000f, percentage);
		mixer.SetFloat(MUSIC_LOWPASS, musicLowPass);
	}

	public void Mute()
	{
		if (IsMuted)
		{
			UpdateSceneMasterVolume(1f);
		}
		else
		{
			UpdateSceneMasterVolume(0f);
		}
	}
}