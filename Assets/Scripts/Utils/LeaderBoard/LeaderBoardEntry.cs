using System;
using Utils;

public class LeaderBoardEntry
{
	private string name;
	private string ip;
	private float value;
	private long utx;

	public string Name => name;
	public string IP => ip;
	public float Value => value;
	public long UTX => utx;

	public LeaderBoardEntry(string name,string ip, float value, long utx)
	{
		this.name = name;
		this.ip = ip;
		this.value = value;
		this.utx = utx;
	}

	public DateTime GetDateTime()
	{
		return TimeManager.UnixTimeStampToDateTime(utx);
	}
}