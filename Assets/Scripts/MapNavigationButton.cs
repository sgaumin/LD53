using DG.Tweening;
using UnityEngine;
using static Facade;

public class MapNavigationButton : MonoBehaviour
{
	[SerializeField] private Vector2 destination;

	[Header("Visual")]
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color hoveredColor;

	[Header("Sound")]
	[SerializeField] private AudioExpress.AudioClip clickSound;

	[Header("References")]
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer.transform.DOScale(Vector2.one * 1.2f, 0.5f).From(Vector2.one).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
	}

	private void OnMouseDown()
	{
		clickSound.Play();

		Level.Loader.CurrentLevel.transform.DOKill();
		Level.Loader.CurrentLevel.transform.DOMove(destination, 0.5f).SetEase(Ease.OutSine);
	}

	private void OnMouseEnter()
	{
		spriteRenderer.color = hoveredColor;
	}

	private void OnMouseExit()
	{
		spriteRenderer.color = defaultColor;
	}
}