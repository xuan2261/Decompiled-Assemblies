using System;

public class RendererGrid : SingletonComponent<RendererGrid>, IClientComponent
{
	public static bool Paused;

	public GameObjectRef BatchPrefab;

	public float CellSize = 50f;

	public float MaxMilliseconds = 0.1f;

	static RendererGrid()
	{
	}

	public RendererGrid()
	{
	}
}