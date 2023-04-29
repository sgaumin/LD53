using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
	public static class GetExternalIPAddress
	{
		public static string IP;

		public static IEnumerator Get()
		{
			if (!string.IsNullOrEmpty(IP)) yield break;

			UnityWebRequest www = UnityWebRequest.Get("https://api64.ipify.org/");
			www.timeout = 10;
			yield return www.SendWebRequest();

			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
			{
				throw new Exception(www.error);
			}
			else
			{
				IP = www.downloadHandler.text;
			}
			Debug.Log(IP);
		}
	}
}