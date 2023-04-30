using AudioExpress;
using TMPro;
using UnityEngine;
using static Facade;

public class MapLevelButton : MonoBehaviour
{
	[SerializeField] private int levelIndex;

	[Header("Visuals")]
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color hoveredColor;

	[Header("Sounds")]
	[SerializeField] private AudioExpress.AudioClip clicSound;

	[Header("References")]
	[SerializeField] private GameObject completed;
	[SerializeField] private TextMeshPro indexText;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void OnValidate()
	{
		indexText.SetText((levelIndex + 1).ToString());
	}

	private void Start()
	{
		gameObject.SetActive(Level.UnlockedLevelIndex >= levelIndex);
		completed.SetActive(Level.UnlockedLevelIndex > levelIndex);
	}

	private void OnMouseDown()
	{
		clicSound.Play();
		Level.LoadLevelIndex(levelIndex);
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