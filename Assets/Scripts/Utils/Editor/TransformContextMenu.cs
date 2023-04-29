#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils
{
	public class TransformContextMenu
	{
		[MenuItem("CONTEXT/Transform/Apply Transform to Children and Reset")]
		private static void ApplyTransformToChildrenAndReset()
		{
			foreach (Transform child in Selection.activeTransform)
			{
				if (child == Selection.activeTransform)
					continue;

				child.position += Selection.activeTransform.position;
				child.rotation *= Selection.activeTransform.rotation;
				child.localScale = Vector3.Scale(Selection.activeTransform.localScale, child.localScale);
			}

			Selection.activeTransform.position = Vector3.zero;
			Selection.activeTransform.rotation = Quaternion.identity;
			Selection.activeTransform.localScale = Vector3.one;

			// Make sure to save data
			EditorUtility.SetDirty(Selection.activeTransform);

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null)
			{
				EditorSceneManager.MarkSceneDirty(prefabStage.scene);
			}
		}
	}
}

#endif