#if UNITY_EDITOR
using UnityEditor;

namespace Utils
{
	public static class CompressionSettingsSetter
	{
		private static TextureImporter buffer;

		[MenuItem("CONTEXT/TextureImporter/Copy", priority = 201)]
		private static void CopySettings(MenuCommand command)
		{
			TextureImporter textureImporter = (TextureImporter)command.context;
			buffer = textureImporter;
		}

		[MenuItem("CONTEXT/TextureImporter/Paste", priority = 202)]
		private static void PasteSettings(MenuCommand command)
		{
			if (buffer != null && buffer is TextureImporter)
			{
				TextureImporter target = (TextureImporter)command.context;
				EditorUtility.CopySerializedIfDifferent(buffer, target);
				target.SaveAndReimport();
			}
		}
	}
}

#endif