using UnityEngine;
using Utils;

public class SettingsData : BaseIndex
{
	private static SettingsData _instance;

	public static SettingsData Instance => GetOrLoad(ref _instance);

	[Header("Animations")]
	public float sceneFadeDuration = 0.5f;

	[Header("Audio")]
	public float audioFadeDuration = 0.2f;
}