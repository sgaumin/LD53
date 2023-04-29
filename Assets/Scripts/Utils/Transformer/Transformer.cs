using DG.Tweening;
using UnityEngine;

namespace Utils
{
	public class Transformer : MonoBehaviour
	{
		[SerializeField] private Positioner positioner;
		[SerializeField] private Rotater rotater;
		[SerializeField] private Scaler scaler;

		private Tween currentPositioner;
		private Tween currentRotater;
		private Tween currentScaler;
		private Vector3 startPosition;
		private Vector3 startRotation;
		private Vector3 startScale;
		private int rotationAmplitudeFactor;
		private bool isAlreadySetup;

		protected void OnEnable()
		{
			if (!isAlreadySetup)
			{
				isAlreadySetup = true;

				startPosition = transform.localPosition;
				startRotation = transform.localRotation.eulerAngles;
				startScale = transform.localScale;

				rotationAmplitudeFactor = rotater.HasFullAmplitude ? 2 : 1;
			}

			RestartAnimations();
		}

		public void RestartAnimations()
		{
			ResetState();
			Init();
		}

		public void Init()
		{
			if (positioner.PlayOnStart)
			{
				PlayLoop(TransformerType.Position, positioner.LoopCount);
			}

			if (rotater.PlayOnStart)
			{
				PlayLoop(TransformerType.Rotation, rotater.LoopCount);
			}

			if (scaler.PlayOnStart)
			{
				PlayLoop(TransformerType.Scale, scaler.LoopCount);
			}
		}

		public void PlayOnce(TransformerType type)
		{
			switch (type)
			{
				case TransformerType.Position:
					currentPositioner?.Kill();
					transform.localPosition = !positioner.IsReverting ? startPosition : transform.localPosition + positioner.Target;

					currentPositioner = transform
						.DOLocalMove(!positioner.IsReverting ? transform.localPosition + positioner.Target : startPosition, positioner.Duration.RandomValue)
						.SetEase(positioner.Ease)
						.SetUpdate(UpdateType.Normal, positioner.IsIgnoringTime)
						.Play();
					break;

				case TransformerType.Rotation:

					currentRotater?.Kill();

					Vector3 target = !rotater.IsReverting ?
						new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + rotationAmplitudeFactor * rotater.AngleTarget) :
						startRotation;

					if (rotater.HasFullAmplitude)
					{
						transform.localRotation = !rotater.IsReverting ?
							Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, startRotation.z - rotater.AngleTarget)) :
							Quaternion.Euler(target);

						currentRotater = transform
								.DORotate(target, rotater.Duration.RandomValue, RotateMode.FastBeyond360)
								.SetEase(rotater.Ease)
								.SetRelative()
								.SetUpdate(UpdateType.Normal, rotater.IsIgnoringTime)
								.Play();
					}
					else
					{
						transform.localRotation = !rotater.IsReverting ?
							Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, startRotation.z)) :
							Quaternion.Euler(target);

						currentRotater = transform
							.DORotate(target, rotater.Duration.RandomValue, RotateMode.FastBeyond360)
							.SetEase(rotater.Ease)
							.SetRelative()
							.SetUpdate(UpdateType.Normal, rotater.IsIgnoringTime)
							.Play();
					}

					break;

				case TransformerType.Scale:
					currentScaler?.Kill();

					float currentFactor = scaler.Factor.RandomValue;
					transform.localScale = !scaler.IsReverting ? startScale : startScale * currentFactor;

					currentScaler = transform
						.DOScale(!scaler.IsReverting ? startScale * currentFactor : startScale, scaler.Duration.RandomValue)
						.SetEase(scaler.Ease)
						.SetUpdate(UpdateType.Normal, scaler.IsIgnoringTime)
						.Play();
					break;
			}
		}

		public void PlayLoop(TransformerType type, int loop = -1)
		{
			switch (type)
			{
				case TransformerType.Position:

					currentPositioner?.Kill();

					transform.localPosition = !positioner.IsReverting ?
						startPosition :
						transform.localPosition + positioner.Target;

					currentPositioner = transform
						.DOLocalMove(!positioner.IsReverting ? transform.localPosition + positioner.Target : startPosition, positioner.Duration.RandomValue)
						.SetEase(positioner.Ease)
						.SetLoops(loop, positioner.LoopType)
						.SetUpdate(UpdateType.Normal, positioner.IsIgnoringTime)
						.Play();
					break;

				case TransformerType.Rotation:

					currentRotater?.Kill();

					Vector3 target = !rotater.IsReverting ?
						new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + rotationAmplitudeFactor * rotater.AngleTarget) :
						new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z - rotationAmplitudeFactor * rotater.AngleTarget);

					if (rotater.HasFullAmplitude)
					{
						transform.localRotation = !rotater.IsReverting ?
							Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, startRotation.z - rotater.AngleTarget)) :
							Quaternion.Euler(target);

						currentRotater = transform
							.DORotate(target, rotater.Duration.RandomValue, RotateMode.FastBeyond360)
							.SetEase(rotater.Ease)
							.SetLoops(loop, rotater.LoopType)
							.SetRelative()
							.SetUpdate(UpdateType.Normal, rotater.IsIgnoringTime)
							.Play();
					}
					else
					{
						transform.localRotation = !rotater.IsReverting ?
							Quaternion.Euler(new Vector3(startRotation.x, startRotation.y, startRotation.z)) :
							Quaternion.Euler(target);

						currentRotater = transform
							.DORotate(target, rotater.Duration.RandomValue, RotateMode.FastBeyond360)
							.SetEase(rotater.Ease)
							.SetLoops(loop, rotater.LoopType)
							.SetRelative()
							.SetUpdate(UpdateType.Normal, rotater.IsIgnoringTime)
							.Play();
					}

					if (rotater.CanStartAtEndPosition)
					{
						currentRotater.Goto(Random.value > 0.5f ? (float)rotater.Duration.RandomValue : 0f, true);
					}

					break;

				case TransformerType.Scale:
					currentScaler?.Kill();

					float currentFactor = scaler.Factor.RandomValue;
					transform.localScale = !scaler.IsReverting ? startScale : startScale * currentFactor;

					currentScaler = transform
						.DOScale(!scaler.IsReverting ? startScale * currentFactor : startScale, scaler.Duration.RandomValue)
						.SetEase(scaler.Ease)
						.SetLoops(loop, scaler.LoopType)
						.SetUpdate(UpdateType.Normal, scaler.IsIgnoringTime)
						.Play();

					break;
			}
		}

		public void ResetState()
		{
			currentPositioner?.Kill();
			transform.localPosition = startPosition;

			currentRotater?.Kill();
			transform.localRotation = Quaternion.Euler(startRotation);

			currentScaler?.Kill();
			transform.localScale = startScale;
		}
	}
}