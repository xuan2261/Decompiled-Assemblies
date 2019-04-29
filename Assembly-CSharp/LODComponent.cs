using System;
using UnityEngine;

public abstract class LODComponent : BaseMonoBehaviour, IClientComponent, ILOD
{
	public LODDistanceMode DistanceMode;

	public LODComponent.OccludeeParameters OccludeeParams = new LODComponent.OccludeeParameters()
	{
		isDynamic = false,
		dynamicUpdateInterval = 0.2f,
		shadowRangeScale = 3f,
		showBounds = false
	};

	protected LODComponent()
	{
	}

	[Serializable]
	public struct OccludeeParameters
	{
		[Tooltip("Is Occludee dynamic or static?")]
		public bool isDynamic;

		[Tooltip("Dynamic occludee update interval in seconds; 0 = every frame")]
		public float dynamicUpdateInterval;

		[Tooltip("Distance scale combined with occludee max bounds size at which culled occludee shadows are still visible")]
		public float shadowRangeScale;

		[Tooltip("Show culling bounds via gizmos; editor only")]
		public bool showBounds;
	}
}