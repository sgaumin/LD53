using DG.Tweening;
using UnityEngine;

namespace Utils
{
	[System.Serializable]
	public class Scaler
	{
		[SerializeField, FloatRangeSlider(0f, 10f)] private FloatRange factor = new FloatRange(1f, 3f);
		[SerializeField, FloatRangeSlider(0f, 80f)] private FloatRange duration = new FloatRange(1f, 3f);
		[SerializeField] private Ease ease = Ease.InOutSine;
		[SerializeField] private LoopType loopType = LoopType.Yoyo;
		[SerializeField] private int loopCount = -1;
		[SerializeField] private bool playOnStart = false;
		[SerializeField] private bool isReverting = false;
		[SerializeField] private bool isIgnoringTime = false;

		public FloatRange Factor => factor;
		public FloatRange Duration => duration;
		public Ease Ease => ease;
		public LoopType LoopType => loopType;
		public int LoopCount => loopCount;
		public bool PlayOnStart => playOnStart;
		public bool IsReverting => isReverting;
		public bool IsIgnoringTime => isIgnoringTime;
	}
}