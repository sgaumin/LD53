using System;
using UnityEngine;

public static class GameData
{
	private const string USER_NAME = "userName";

	public static string UserName
	{
		get
		{
			return PlayerPrefs.HasKey(USER_NAME) ? PlayerPrefs.GetString(USER_NAME) : "";
		}
		set
		{
			PlayerPrefs.SetString(USER_NAME, value);
			PlayerPrefs.Save();
		}
	}

	public static void DeleteAllSave()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}