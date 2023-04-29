#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils
{
	// Based on documentation: https://docs.unity3d.com/Manual/BuildPlayerPipeline.html
	public class ScriptBatch
	{
		private static string GeneralBuildsFolder => Path.Combine(Path.GetDirectoryName(Application.dataPath), "Builds");

		[MenuItem("Tools/Build Main Platforms %B", false, 1)]
		public static void BuildMainPlatform()
		{
			BuildTarget startTarget = EditorUserBuildSettings.activeBuildTarget;
			BuildTargetGroup startTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			try
			{
				BuildProcess(BuildTarget.WebGL);
				BuildProcess(BuildTarget.StandaloneWindows64);
				BuildProcess(BuildTarget.StandaloneWindows);
				EditorUserBuildSettings.SwitchActiveBuildTarget(startTargetGroup, startTarget);
			}
			catch (System.Exception)
			{
				return;
			}

			ShowExplorer();
		}

		[MenuItem("Tools/Reserialize All Assets", false, 2)]
		public static void ReserializeAllAssets()
		{
			AssetDatabase.ForceReserializeAssets();
		}

		private static void ShowExplorer()
		{
			System.Diagnostics.Process.Start(GeneralBuildsFolder);
		}

		public static void BuildProcess(BuildTarget target)
		{
			if (!Directory.Exists(GeneralBuildsFolder))
			{
				Directory.CreateDirectory(GeneralBuildsFolder);
			}

			string templateName = $"{Application.productName.Replace(" ", "")}_{target}_{Application.version}";
			string buildFolder = Path.Combine(GeneralBuildsFolder, templateName);
			if (!Directory.Exists(buildFolder))
			{
				Directory.CreateDirectory(buildFolder);
			}

			string buildName;
			if (target == BuildTarget.WebGL)
			{
				buildName = buildFolder;
			}
			else
			{
				buildName = Path.Combine(buildFolder, $"{Application.productName}.exe");
			}
			if (EditorBuildSettings.scenes.Length > 0)
			{
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildName, target, BuildOptions.None);
			}

			// Compression
			string zipFolder = Path.Combine(GeneralBuildsFolder, $"{templateName}.zip");
			if (File.Exists(zipFolder))
			{
				File.Delete(zipFolder);
			}
			System.IO.Compression.ZipFile.CreateFromDirectory(buildFolder, zipFolder, System.IO.Compression.CompressionLevel.Fastest, false);
		}
	}
}

#endif