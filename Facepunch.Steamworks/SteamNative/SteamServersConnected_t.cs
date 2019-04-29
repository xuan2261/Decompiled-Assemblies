using Facepunch.Steamworks;
using Facepunch.Steamworks.Interop;
using System;
using System.Runtime.InteropServices;

namespace SteamNative
{
	internal struct SteamServersConnected_t
	{
		internal const int CallbackId = 101;

		internal static SteamServersConnected_t FromPointer(IntPtr p)
		{
			if (!Platform.PackSmall)
			{
				return (SteamServersConnected_t)Marshal.PtrToStructure(p, typeof(SteamServersConnected_t));
			}
			return (SteamServersConnected_t.PackSmall)Marshal.PtrToStructure(p, typeof(SteamServersConnected_t.PackSmall));
		}

		[MonoPInvokeCallback]
		internal static int OnGetSize()
		{
			return SteamServersConnected_t.StructSize();
		}

		[MonoPInvokeCallback]
		internal static int OnGetSizeThis(IntPtr self)
		{
			return SteamServersConnected_t.OnGetSize();
		}

		[MonoPInvokeCallback]
		internal static void OnResult(IntPtr param)
		{
			SteamServersConnected_t.OnResultWithInfo(param, false, (long)0);
		}

		[MonoPInvokeCallback]
		internal static void OnResultThis(IntPtr self, IntPtr param)
		{
			SteamServersConnected_t.OnResult(param);
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfo(IntPtr param, bool failure, SteamAPICall_t call)
		{
			if (failure)
			{
				return;
			}
			SteamServersConnected_t steamServersConnectedT = SteamServersConnected_t.FromPointer(param);
			if (Client.Instance != null)
			{
				Client.Instance.OnCallback<SteamServersConnected_t>(steamServersConnectedT);
			}
			if (Server.Instance != null)
			{
				Server.Instance.OnCallback<SteamServersConnected_t>(steamServersConnectedT);
			}
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfoThis(IntPtr self, IntPtr param, bool failure, SteamAPICall_t call)
		{
			SteamServersConnected_t.OnResultWithInfo(param, failure, call);
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
						ResultA = new Callback.VTableThis.ResultD(SteamServersConnected_t.OnResultThis),
						ResultB = new Callback.VTableThis.ResultWithInfoD(SteamServersConnected_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableThis.GetSizeD(SteamServersConnected_t.OnGetSizeThis)
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
						ResultA = new Callback.VTableWinThis.ResultD(SteamServersConnected_t.OnResultThis),
						ResultB = new Callback.VTableWinThis.ResultWithInfoD(SteamServersConnected_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableWinThis.GetSizeD(SteamServersConnected_t.OnGetSizeThis)
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
					ResultA = new Callback.VTable.ResultD(SteamServersConnected_t.OnResult),
					ResultB = new Callback.VTable.ResultWithInfoD(SteamServersConnected_t.OnResultWithInfo),
					GetSize = new Callback.VTable.GetSizeD(SteamServersConnected_t.OnGetSize)
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
					ResultA = new Callback.VTableWin.ResultD(SteamServersConnected_t.OnResult),
					ResultB = new Callback.VTableWin.ResultWithInfoD(SteamServersConnected_t.OnResultWithInfo),
					GetSize = new Callback.VTableWin.GetSizeD(SteamServersConnected_t.OnGetSize)
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
				CallbackId = 101
			};
			callbackHandle.PinnedCallback = GCHandle.Alloc(callback, GCHandleType.Pinned);
			steamworks.native.api.SteamAPI_RegisterCallback(callbackHandle.PinnedCallback.AddrOfPinnedObject(), 101);
			steamworks.RegisterCallbackHandle(callbackHandle);
		}

		internal static int StructSize()
		{
			if (Platform.PackSmall)
			{
				return Marshal.SizeOf(typeof(SteamServersConnected_t.PackSmall));
			}
			return Marshal.SizeOf(typeof(SteamServersConnected_t));
		}

		internal struct PackSmall
		{
			public static implicit operator SteamServersConnected_t(SteamServersConnected_t.PackSmall d)
			{
				return new SteamServersConnected_t();
			}
		}
	}
}