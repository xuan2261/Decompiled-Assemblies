using Steamworks.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ServerBrowserInfo : SingletonComponent<ServerBrowserInfo>
{
	public bool isMain;

	public Text serverName;

	public Text serverMeta;

	public Text serverText;

	public RawImage headerImage;

	public Button viewWebpage;

	public Button refresh;

	public ServerInfo? currentServer;

	public Texture defaultServerImage;

	public ServerBrowserInfo()
	{
	}
}