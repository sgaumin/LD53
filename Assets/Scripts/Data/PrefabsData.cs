using Utils;

public sealed class PrefabsData : BaseIndex
{
	private static PrefabsData _instance;

	public static PrefabsData Instance => GetOrLoad(ref _instance);

	// Set up your references below!
}