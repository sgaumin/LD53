using RoutineExpress;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
	public static class LeaderBoardRequests
	{
		private static readonly string URL = "https://sgaumin.com/";

		public static void Get(LeaderBoardOrder order = LeaderBoardOrder.DESC, Action<List<LeaderBoardEntry>> callback = null)
		{
			RoutinePool.Run(GetCore(order, callback));
		}

		public static void Post(string name, float value, Action<string> callback = null)
		{
			RoutinePool.Run(PostCore(name, value, callback));
		}

		private static IEnumerator GetCore(LeaderBoardOrder order = LeaderBoardOrder.DESC, Action<List<LeaderBoardEntry>> callback = null)
		{
			yield return GetExternalIPAddress.Get();

			var url = $"{URL}getLeaderBoard.php";

			WWWForm form = new WWWForm();
			form.AddField("game", Application.productName);
			form.AddField("prd", (!Application.isEditor).ToString());
			form.AddField("order", order.ToString());

			using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
			{
				yield return webRequest.SendWebRequest();
				var result = DoValidation(url, webRequest);

				// Parsing retrieved data from server
				List<LeaderBoardEntry> entries = new List<LeaderBoardEntry>();
				foreach (var entry in result.Split('|'))
				{
					string[] p = entry.Split('#');
					if (p.Length != 4) continue;

					float val = 0f;
#if UNITY_WEBGL && !UNITY_EDITOR // Fixing potential float passing issues on WebGL
					val = float.Parse(p[2].Replace('.', ','));
#else
					val = float.Parse(p[2]);
#endif

					entries.Add(new LeaderBoardEntry(p[0], p[1], val, long.Parse(p[3])));
				}

				callback?.Invoke(entries);
			}
		}

		private static IEnumerator PostCore(string name, float value, Action<string> callback = null)
		{
			yield return GetExternalIPAddress.Get();

			var url = $"{URL}addLeaderBoardEntry.php";

			// Cleanup name parameter for avoiding potential content parsing issues
			name = name.Replace("|", "").Replace(",", "").Replace("#", "");

			WWWForm form = new WWWForm();
			form.AddField("game", Application.productName);
			form.AddField("prd", (!Application.isEditor).ToString());
			form.AddField("ip", GetExternalIPAddress.IP);
			form.AddField("name", name);
			form.AddField("value", value.ToString());
			form.AddField("utx", TimeManager.UnixTimeNow().ToString());

			using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
			{
				yield return webRequest.SendWebRequest();
				var result = DoValidation(url, webRequest);
				callback?.Invoke(result);
			}
		}

		private static string DoValidation(string url, UnityWebRequest webRequest)
		{
			string[] pages = url.Split('/');
			int page = pages.Length - 1;

			switch (webRequest.result)
			{
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError(pages[page] + ": Error: " + webRequest.error);
					break;

				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
					break;

				case UnityWebRequest.Result.Success:
					Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
					return webRequest.downloadHandler.text;
			}

			return null;
		}
	}
}