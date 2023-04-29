using UnityEngine;

public static class OpenURL
{
	public static void Open(string link)
	{
		if (!string.IsNullOrEmpty(link))
		{
			Application.OpenURL(link);
		}
	}

	public static void OpenTwitterProfile()
	{
		Application.OpenURL(Constants.TWITTER_PROFILE);
	}

	public static void OpenItchProfile()
	{
		Application.OpenURL(Constants.ITCH_PROFILE);
	}

	public static void OpenGitHubProfile()
	{
		Application.OpenURL(Constants.GITHUB_PROFILE);
	}
}