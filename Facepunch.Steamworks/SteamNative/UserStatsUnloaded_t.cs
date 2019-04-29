using Facepunch.Steamworks;
using Facepunch.Steamworks.Interop;
using System;
using System.Runtime.InteropServices;

namespace SteamNative
{
	internal struct UserStatsUnloaded_t
	{
		internal const int CallbackId = 1108;

		internal ulong SteamIDUser;

		internal static UserStatsUnloaded_t FromPointer(IntPtr p)
		{
			if (!Platform.PackSmall)
			{
				return (UserStatsUnloaded_t)Marshal.PtrToStructure(p, typeof(UserStatsUnloaded_t));
			}
			return (UserStatsUnloaded_t.PackSmall)Marshal.PtrToStructure(p, typeof(UserStatsUnloaded_t.PackSmall));
		}

		[MonoPInvokeCallback]
		internal static int OnGetSize()
		{
			return UserStatsUnloaded_t.StructSize();
		}

		[MonoPInvokeCallback]
		internal static int OnGetSizeThis(IntPtr self)
		{
			return UserStatsUnloaded_t.OnGetSize();
		}

		[MonoPInvokeCallback]
		internal static void OnResult(IntPtr param)
		{
			UserStatsUnloaded_t.OnResultWithInfo(param, false, (long)0);
		}

		[MonoPInvokeCallback]
		internal static void OnResultThis(IntPtr self, IntPtr param)
		{
			UserStatsUnloaded_t.OnResult(param);
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfo(IntPtr param, bool failure, SteamAPICall_t call)
		{
			if (failure)
			{
				return;
			}
			UserStatsUnloaded_t userStatsUnloadedT = UserStatsUnloaded_t.FromPointer(param);
			if (Client.Instance != null)
			{
				Client.Instance.OnCallback<UserStatsUnloaded_t>(userStatsUnloadedT);
			}
			if (Server.Instance != null)
			{
				Server.Instance.OnCallback<UserStatsUnloaded_t>(userStatsUnloadedT);
			}
		}

		[MonoPInvokeCallback]
		internal static void OnResultWithInfoThis(IntPtr self, IntPtr param, bool failure, SteamAPICall_t call)
		{
			UserStatsUnloaded_t.OnResultWithInfo(param, failure, call);
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
						ResultA = new Callback.VTableThis.ResultD(UserStatsUnloaded_t.OnResultThis),
						ResultB = new Callback.VTableThis.ResultWithInfoD(UserStatsUnloaded_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableThis.GetSizeD(UserStatsUnloaded_t.OnGetSizeThis)
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
						ResultA = new Callback.VTableWinThis.ResultD(UserStatsUnloaded_t.OnResultThis),
						ResultB = new Callback.VTableWinThis.ResultWithInfoD(UserStatsUnloaded_t.OnResultWithInfoThis),
						GetSize = new Callback.VTableWinThis.GetSizeD(UserStatsUnloaded_t.OnGetSizeThis)
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
					ResultA = new Callback.VTable.ResultD(UserStatsUnloaded_t.OnResult),
					ResultB = new Callback.VTable.ResultWithInfoD(UserStatsUnloaded_t.OnResultWithInfo),
					GetSize = new Callback.VTable.GetSizeD(UserStatsUnloaded_t.OnGetSize)
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
					ResultA = new Callback.VTableWin.ResultD(UserStatsUnloaded_t.OnResult),
					ResultB = new Callback.VTableWin.ResultWithInfoD(UserStatsUnloaded_t.OnResultWithInfo),
					GetSize = new Callback.VTableWin.GetSizeD(UserStatsUnloaded_t.OnGetSize)
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
				CallbackId = 1108
			};
			callbackHandle.PinnedCallback = GCHandle.Alloc(callback, GCHandleType.Pinned);
			steamworks.native.api.SteamAPI_RegisterCallback(callbackHandle.PinnedCallback.AddrOfPinnedObject(), 1108);
			steamworks.RegisterCallbackHandle(callbackHandle);
		}

		internal static int StructSize()
		{
			if (Platform.PackSmall)
			{
				return Marshal.SizeOf(typeof(UserStatsUnloaded_t.PackSmall));
			}
			return Marshal.SizeOf(typeof(UserStatsUnloaded_t));
		}

		internal struct PackSmall
		{
			internal ulong SteamIDUser;

			public static implicit operator UserStatsUnloaded_t(UserStatsUnloaded_t.PackSmall d)
			{
				return new UserStatsUnloaded_t()
				{
					SteamIDUser = d.SteamIDUser
				};
			}
		}
	}
}