public static class Facade
{
	public static GameBase Level => GameBase.Instance;
	public static PrefabsData Prefabs => PrefabsData.Instance;
	public static SettingsData Settings => SettingsData.Instance;
}