using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Setup a 2D button using <see cref="SpriteRenderer"/> and <see cref="Collider2D"/>.
/// </summary>
[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class SpriteButton : MonoBehaviour
{
	[SerializeField] private bool interactable = true;

	[Header("Colors")]
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color hoverColor;
	[SerializeField] private Color downColor;
	[SerializeField] private Color disableColor;

	[Header("Animations")]
	[SerializeField] private float fadeDuration = 0.15f;

	[Header("References")]
	[SerializeField] private SpriteRenderer spriteRenderer;

	[Header("Events")]
	[SerializeField] private UnityEvent OnMouseDownEvent;
	[SerializeField] private UnityEvent OnMouseUpEvent;
	[SerializeField] private UnityEvent OnMouseEnterEvent;
	[SerializeField] private UnityEvent OnMousExitEvent;

	private void Reset()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		defaultColor = new Color(1f, 1f, 1f, 0.75f);
		hoverColor = new Color(1f, 1f, 1f, 0.9f);
		downColor = new Color(1f, 1f, 1f, 1f);
		disableColor = new Color(1f, 1f, 1f, 0.5f);
	}

	/// <summary>
	/// Sets interaction state of this <see cref="SpriteButton"/>.
	/// </summary>
	public bool Interactable
	{
		get => interactable;
		set
		{
			interactable = value;
			spriteRenderer.color = interactable ? defaultColor : disableColor;
		}
	}

	private void Start()
	{
		spriteRenderer.color = defaultColor;
	}

	private void OnMouseDown()
	{
		if (!interactable) return;

		spriteRenderer.DOKill();
		spriteRenderer.color = downColor;

		OnMouseDownEvent?.Invoke();
	}

	private void OnMouseUpAsButton()
	{
		if (!interactable) return;

		spriteRenderer.DOKill();
		spriteRenderer.DOColor(hoverColor, fadeDuration);

		OnMouseUpEvent?.Invoke();
	}

	private void OnMouseEnter()
	{
		if (!interactable) return;

		spriteRenderer.DOKill();
		spriteRenderer.DOColor(hoverColor, fadeDuration);

		OnMouseEnterEvent?.Invoke();
	}

	private void OnMouseExit()
	{
		if (!interactable) return;

		spriteRenderer.DOKill();
		spriteRenderer.DOColor(defaultColor, fadeDuration);

		OnMousExitEvent?.Invoke();
	}
}