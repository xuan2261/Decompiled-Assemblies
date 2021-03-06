using System;
using UnityEngine;
using UnityEngine.UI;

public class MapInterface : SingletonComponent<MapInterface>
{
	public static bool IsOpen;

	public RawImage mapImage;

	public Image cameraPositon;

	public ScrollRectEx scrollRect;

	public Toggle showGridToggle;

	public Button FocusButton;

	public GameObject monumentMarkerContainer;

	public GameObject monumentMarkerPrefab;

	public UnityEngine.CanvasGroup CanvasGroup;

	public bool followingPlayer = true;

	public bool DebugStayOpen;

	public MapInterface()
	{
	}
}