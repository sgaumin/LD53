using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

public class LogoController : MonoBehaviour
{
	[SerializeField] private float fadeDuration = 0.5f;

	[Header("References")]
	[SerializeField] private SpriteRenderer spriteRenderer;

	private bool fading;

	private void OnEnable()
	{
		fading = false;
		spriteRenderer.WithAlpha(1f);
	}

	private void Update()
	{
		if (Input.anyKeyDown && !fading)
		{
			FadOut().Forget();
		}
	}

	private async UniTask FadOut()
	{
		fading = true;
		await spriteRenderer.DOFade(0f, fadeDuration).From(1f);
		gameObject.SetActive(false);
	}
}