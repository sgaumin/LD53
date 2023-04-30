using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Facade;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] private GameObject cinematic;
	[SerializeField] private GameObject mapNavigation;
	[SerializeField] private List<GameObject> levels;

	[Header("References")]
	[SerializeField] private Transform levelHolder;

	private bool hasFirstFade;
	private Coroutine _loadingRoutine;

	public int CurrentIndex { get; private set; }
	public GameObject CurrentLevel { get; private set; }

	public void RespawnCurrentLevel()
	{
		LoadLevelIndex(CurrentIndex);
	}

	public void LoadCinematic()
	{
		LoadLevelGameObject(cinematic).Forget();
	}

	public void LoadMapNavigation()
	{
		LoadLevelGameObject(mapNavigation).Forget();
	}

	public void LoadNext()
	{
		LoadLevelIndex(CurrentIndex + 1);
	}

	public void LoadLevelIndex(int index)
	{
		CurrentIndex = index;

		LoadLevelGameObject(levels[index]).Forget();
	}

	private async UniTask LoadLevelGameObject(GameObject gameObject)
	{
		if (Level.State == GameState.StartLoading) return;

		Level.State = GameState.StartLoading;

		if (hasFirstFade)
			await Level.Effects.Fader.FadeOutAsync();

		hasFirstFade = true;

		if (CurrentLevel != null)
			Destroy(CurrentLevel);

		await UniTask.Delay(500);
		Level.State = GameState.EndLoading;

		CurrentLevel = Instantiate(gameObject, levelHolder);

		await Level.Effects.Fader.FadeInAsync();
	}

	public void Reload(float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Reload();
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void LoadNextScene(float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Next();
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void LoadPrevious(float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Previous();
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void LoadMenu(float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Load(Constants.MENU_SCENE);
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void LoadSceneByName(string sceneName, float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Load(sceneName);
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void LoadSceneTransition(SceneLoading loading, float transitionTime = 0f)
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(
				LoadSceneCore
				(
					transitionTime,
					content: () =>
					{
						SceneLoader.Load(loading);
					}
				),
				ref _loadingRoutine
			);
		}
	}

	public void Quit()
	{
		if (_loadingRoutine == null)
		{
			this.TryStartCoroutine(LoadSceneCore(content: () =>
			{
				SceneLoader.Quit();
			}),
			ref _loadingRoutine);
		}
	}

	private IEnumerator LoadSceneCore(float transitionTime = 0f, Action content = null)
	{
		yield return new WaitForSeconds(transitionTime);
		Time.timeScale = 1f;

		content?.Invoke();

		_loadingRoutine = null;
	}
}