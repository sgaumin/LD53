using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GrassGenerator : MonoBehaviour
{
	[SerializeField] private List<Sprite> grasses;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer.sprite = grasses.Random();
	}
}