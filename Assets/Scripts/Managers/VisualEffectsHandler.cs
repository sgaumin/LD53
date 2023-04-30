﻿using Cinemachine;
using UnityEngine;

public class VisualEffectsHandler : MonoBehaviour
{
	[SerializeField] private FadeScreen fader;
	[SerializeField] private CinemachineImpulseSource impulse;
	[SerializeField] private CinemachineVirtualCamera currentCamera;

	public FadeScreen Fader => fader;


	private void Start()
	{
		if (fader != null)
		{
			fader.FadeIn();
		}
	}

	public void GenerateImpulse()
	{
		impulse.GenerateImpulse();
	}
}