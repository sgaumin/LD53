﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using static Facade;

public class FadeScreen : MonoBehaviour
{
	[SerializeField] private Image image;

	private Color defaultFadeInColorTarget = Color.black;
	private Color defaultFadeOutColorTarget = Color.black.WithAlpha(0);

	public void FadeOut(float fadeDuration)
	{
		image.color = defaultFadeOutColorTarget;
		image.DOFade(1f, fadeDuration);
	}

	public void FadeOut()
	{
		image.color = defaultFadeOutColorTarget;
		image.DOFade(1f, Settings.sceneFadeDuration);
	}

	public async UniTask FadeOutCore(float fadeDuration)
	{
		image.color = defaultFadeOutColorTarget;
		await image.DOFade(1f, fadeDuration);
	}

	public async UniTask FadeOutAsync()
	{
		image.color = defaultFadeOutColorTarget;
		await image.DOFade(1f, Settings.sceneFadeDuration);
	}

	public void FadeIn(float fadeDuration)
	{
		image.color = defaultFadeInColorTarget;
		image.DOFade(0f, fadeDuration);
	}

	public void FadeIn()
	{
		image.color = defaultFadeInColorTarget;
		image.DOFade(0f, Settings.sceneFadeDuration);
	}

	public IEnumerator FadeInCore(float fadeDuration)
	{
		image.color = defaultFadeInColorTarget;
		Tweener fade = image.DOFade(0f, fadeDuration);
		yield return fade.WaitForCompletion();
	}

	public IEnumerator FadeInAsync()
	{
		image.color = defaultFadeInColorTarget;
		Tweener fade = image.DOFade(0f, Settings.sceneFadeDuration);
		yield return fade.WaitForCompletion();
	}
}