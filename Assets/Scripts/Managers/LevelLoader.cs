using System;
using System.Collections;
using UnityEngine;
using Utils;

public class LevelLoader : MonoBehaviour
{
	private Coroutine _loadingRoutine;

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