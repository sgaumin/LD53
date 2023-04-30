using Cinemachine;
using UnityEngine;

public class VisualEffectsHandler : MonoBehaviour
{
	[SerializeField] private FadeScreen fader;
	[SerializeField] private CinemachineImpulseSource impulse;
	[SerializeField] private CinemachineVirtualCamera currentCamera;

	public FadeScreen Fader => fader;

	public void GenerateImpulse()
	{
		impulse.GenerateImpulse();
	}
}