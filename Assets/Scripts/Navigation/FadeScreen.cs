using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using static Facade;

public class FadeScreen : MonoBehaviour
{
	[SerializeField] private Image image;

	private Color defaultFadeInColorTarget = Color.black;
	private Color defaultFadeOutColorTarget = Color.black.WithAlpha(0);

	public async UniTask FadeOutAsync()
	{
		image.color = defaultFadeOutColorTarget;
		await image.DOFade(1f, Settings.sceneFadeDuration);
	}

	public async UniTask FadeInAsync()
	{
		image.color = defaultFadeInColorTarget;
		await image.DOFade(0f, Settings.sceneFadeDuration);
	}
}