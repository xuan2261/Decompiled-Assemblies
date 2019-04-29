using Facepunch.Steamworks;
using Facepunch.Steamworks.Interop;
using System;
using System.Runtime.InteropServices;

namespace SteamNative
{
	internal struct GameConnectedChatLeave_t
	{
		internal const int CallbackId = 340;

		internal ulong SteamIDClanChat;

		internal ulong SteamIDUser;

		internal bool Kicked;

		internal bool Dropped;

		internal static GameConnectedChatLeave_t FromPointer(IntPtr p)
		{
			if (!Platform.PackSmall)
			{
				return (GameConnectedChatLeave_t)Marshal.PtrToStructure(p, typeof(GameConnectedChatLeave_t));
			}
			return (GameConnectedChatLeave_t.PackSmall)Marshal.PtrToStructure(p, typeof(GameConnectedChatLeave_t.PackSmall));
		}

		[MonoPInvokeCallback]
		internal static int OnGetSize()
		{
			return GameConnectedChatLeave_t.StructSize();
		}

		[MonoPInvokeCallback]
		internal static int OnGetSizeThis(IntPtr self)
		{
			return GameConnectedChatLeave_t.OnGetSize();
		}

		[MonoPInvokeCallback]
		internal static void OnResult(IntPtr param)
		{
			GameConnectedChatLeave_t.OnResultWithInfo(param, false, (long)0);
		}

		[MonoPInvokeCallback]
		internal static void OnResultThis(IntPtr self, IntPtr param)
		{
			GameConnectedChatLeave_t.OnResult(param);
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfo(IntPtr param, bool failure, SteamAPICall_t call)
		{
			if (failure)
			{
				return;
			}
			GameConnectedChatLeave_t gameConnectedChatLeaveT = GameConnectedChatLeave_t.FromPointer(param);
			if (Client.Instance != null)
			{
				Client.Instance.OnCallback<GameConnectedChatLeave_t>(gameConnectedChatLeaveT);
			}
			if (Server.Instance != null)
			{
				Server.Instance.OnCallback<GameConnectedChatLeave_t>(gameConnectedChatLeaveT);
			}
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfoThis(IntPtr self, IntPtr param, bool failure, SteamAPICall_t call)
		{
			GameConnectedChatLeave_t.OnResultWithInfo(param, failure, call);
		}

		internal static void Register(BaseSteamworks steamworks)
		{
			CallbackHandle callbackHandle = new CallbackHandle(steamworks);
			if (Config.UseThisCall)
			{
				if (!Platform.IsWindows)
				{
					callbackHandle.vTablePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Callback.VTableThis)));
					Callback.VTableThis vTableThi = new Callback.VTableThis()
					{
						ResultA = new Callback.VTableThis.ResultD(GameConnectedChatLeave_t.OnResultThis),
						ResultB = new Callback.VTableThis.ResultWithInfoD(GameConnectedChatLeave_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableThis.GetSizeD(GameConnectedChatLeave_t.OnGetSizeThis)
					};
					callbackHandle.FuncA = GCHandle.Alloc(vTableThi.ResultA);
					callbackHandle.FuncB = GCHandle.Alloc(vTableThi.ResultB);
					callbackHandle.FuncC = GCHandle.Alloc(vTableThi.GetSize);
					Marshal.StructureToPtr(vTableThi, callbackHandle.vTablePtr, false);
				}
				else
				{
					callbackHandle.vTablePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Callback.VTableWinThis)));
					Callback.VTableWinThis vTableWinThi = new Callback.VTableWinThis()
					{
						ResultA = new Callback.VTableWinThis.ResultD(GameConnectedChatLeave_t.OnResultThis),
						ResultB = new Callback.VTableWinThis.ResultWithInfoD(GameConnectedChatLeave_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableWinThis.GetSizeD(GameConnectedChatLeave_t.OnGetSizeThis)
					};
					callbackHandle.FuncA = GCHandle.Alloc(vTableWinThi.ResultA);
					callbackHandle.FuncB = GCHandle.Alloc(vTableWinThi.ResultB);
					callbackHandle.FuncC = GCHandle.Alloc(vTableWinThi.GetSize);
					Marshal.StructureToPtr(vTableWinThi, callbackHandle.vTablePtr, false);
				}
			}
			else if (!Platform.IsWindows)
			{
				callbackHandle.vTablePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Callback.VTable)));
				Callback.VTable vTable = new Callback.VTable()
				{
					ResultA = new Callback.VTable.ResultD(GameConnectedChatLeave_t.OnResult),
					ResultB = new Callback.VTable.ResultWithInfoD(GameConnectedChatLeave_t.OnResultWithInfo),
					GetSize = new Callback.VTable.GetSizeD(GameConnectedChatLeave_t.OnGetSize)
				};
				callbackHandle.FuncA = GCHandle.Alloc(vTable.ResultA);
				callbackHandle.FuncB = GCHandle.Alloc(vTable.ResultB);
				callbackHandle.FuncC = GCHandle.Alloc(vTable.GetSize);
				Marshal.StructureToPtr(vTable, callbackHandle.vTablePtr, false);
			}
			else
			{
				callbackHandle.vTablePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Callback.VTableWin)));
				Callback.VTableWin vTableWin = new Callback.VTableWin()
				{
					ResultA = new Callback.VTableWin.ResultD(GameConnectedChatLeave_t.OnResult),
					ResultB = new Callback.VTableWin.ResultWithInfoD(GameConnectedChatLeave_t.OnResultWithInfo),
					GetSize = new Callback.VTableWin.GetSizeD(GameConnectedChatLeave_t.OnGetSize)
				};
				callbackHandle.FuncA = GCHandle.Alloc(vTableWin.ResultA);
				callbackHandle.FuncB = GCHandle.Alloc(vTableWin.ResultB);
				callbackHandle.FuncC = GCHandle.Alloc(vTableWin.GetSize);
				Marshal.StructureToPtr(vTableWin, callbackHandle.vTablePtr, false);
			}
			Callback callback = new Callback()
			{
				vTablePtr = callbackHandle.vTablePtr,
				CallbackFlags = (byte)((steamworks.IsGameServer ? 2 : 0)),
				CallbackId = 340
			};
			callbackHandle.PinnedCallback = GCHandle.Alloc(callback, GCHandleType.Pinned);
			steamworks.native.api.SteamAPI_RegisterCallback(callbackHandle.PinnedCallback.AddrOfPinnedObject(), 340);
			steamworks.RegisterCallbackHandle(callbackHandle);
		}

		internal static int StructSize()
		{
			if (Platform.PackSmall)
			{
				return Marshal.SizeOf(typeof(GameConnectedChatLeave_t.PackSmall));
			}
			return Marshal.SizeOf(typeof(GameConnectedChatLeave_t));
		}

		internal struct PackSmall
		{
			internal ulong SteamIDClanChat;

			internal ulong SteamIDUser;

			internal bool Kicked;

			internal bool Dropped;

			public static implicit operator GameConnectedChatLeave_t(GameConnectedChatLeave_t.PackSmall d)
			{
				GameConnectedChatLeave_t gameConnectedChatLeaveT = new GameConnectedChatLeave_t()
				{
					SteamIDClanChat = d.SteamIDClanChat,
					SteamIDUser = d.SteamIDUser,
					Kicked = d.Kicked,
					Dropped = d.Dropped
				};
				return gameConnectedChatLeaveT;
			}
		}
	}
}