#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
{
#if !UNITY_EDITOR && UNITY_WEBGL
		[DllImport("__Internal")]
		private static extern void OpenNewTab(string url);
#endif

	[SerializeField] private Color linkColor;
	[SerializeField] private Color linkHighlightedColor;

	private TMP_Text pTextMeshPro;
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
		pTextMeshPro = GetComponent<TMP_Text>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, mainCamera);
		if (linkIndex != -1)
		{
			TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

#if !UNITY_EDITOR && UNITY_WEBGL
			OpenNewTab(linkInfo.GetLinkID());
#else
			Application.OpenURL(linkInfo.GetLinkID());
#endif
		}
	}

	private void Update()
	{
		for (int i = 0; i < pTextMeshPro.textInfo.linkCount; i++)
		{
			SetLinkToColor(i, linkColor);
		}

		int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, mainCamera);
		if (linkIndex != -1)
		{
			SetLinkToColor(linkIndex, linkHighlightedColor);
		}
	}

	private void SetLinkToColor(int linkIndex, Color32 color)
	{
		TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

		if (linkInfo.textComponent != null)
		{
			for (int i = 0; i < linkInfo.linkTextLength; i++)
			{
				int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
				var charInfo = pTextMeshPro.textInfo.characterInfo[characterIndex];
				if (charInfo.character != ' ')
				{
					int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
					int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

					Color32[] vertexColors = pTextMeshPro.textInfo.meshInfo[meshIndex].colors32;
					vertexColors[vertexIndex + 0] = color;
					vertexColors[vertexIndex + 1] = color;
					vertexColors[vertexIndex + 2] = color;
					vertexColors[vertexIndex + 3] = color;
				}
			}
		}

		pTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
	}
}