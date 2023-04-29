using DG.Tweening;
using UnityEngine;

namespace Utils
{
	[System.Serializable]
	public class Rotater
	{
		[SerializeField] private float angleTarget = 360f;
		[SerializeField] private bool hasFullAmplitude;
		[SerializeField, FloatRangeSlider(0f, 80f)] private FloatRange duration = new FloatRange(1f, 3f);
		[SerializeField] private Ease ease = Ease.InOutSine;
		[SerializeField] private LoopType loopType = LoopType.Yoyo;
		[SerializeField] private int loopCount = -1;
		[SerializeField] private bool playOnStart = false;
		[SerializeField] private bool isReverting = false;
		[SerializeField] private bool isIgnoringTime = false;
		[SerializeField] private bool canStartAtEndPosition = false;

		public float AngleTarget => angleTarget;
		public bool HasFullAmplitude => hasFullAmplitude;
		public FloatRange Duration => duration;
		public Ease Ease => ease;
		public LoopType LoopType => loopType;
		public int LoopCount => loopCount;
		public bool PlayOnStart => playOnStart;
		public bool IsReverting => isReverting;
		public bool IsIgnoringTime => isIgnoringTime;
		public bool CanStartAtEndPosition => canStartAtEndPosition;
	}
}