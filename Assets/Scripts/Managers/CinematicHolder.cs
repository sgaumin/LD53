using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using static Facade;

public class CinematicHolder : MonoBehaviour
{
	[Serializable]
	private class CinematicData
	{
		public Sprite sprite;
		public string[] lines;
	}

	[SerializeField] private List<CinematicData> data;

	[Header("Components")]
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private TextMeshPro description;

	private int currentDataIndex;
	private int currentLineIndex;

	private void Start()
	{
		Level.State = GameState.OnMap;
		Show().Forget();
	}

	private async UniTask Show()
	{
		description.alpha = 0f;

		while (currentDataIndex < data.Count)
		{
			currentLineIndex = 0;
			await UniTask.Delay(500);

			spriteRenderer.sprite = data[currentDataIndex].sprite;
			await spriteRenderer.DOFade(1f, 0.5f).From(0f);

			while (currentLineIndex < data[currentDataIndex].lines.Length)
			{
				description.SetText($"{data[currentDataIndex].lines[currentLineIndex++]}");
				await description.DOFade(1f, 0.5f).From(0f);

				while (!Input.anyKeyDown)
				{
					await UniTask.Yield();
				}

				await description.DOFade(0f, 0.5f).From(1f);
				await UniTask.Delay(500);
			}

			await spriteRenderer.DOFade(0f, 0.5f).From(1f);

			await UniTask.Delay(500);

			currentDataIndex++;
		}

		Level.LoadMap();
	}
}