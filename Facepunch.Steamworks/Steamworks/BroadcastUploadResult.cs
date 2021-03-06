using System;

namespace Steamworks
{
	public enum BroadcastUploadResult
	{
		None,
		OK,
		InitFailed,
		FrameFailed,
		Timeout,
		BandwidthExceeded,
		LowFPS,
		MissingKeyFrames,
		NoConnection,
		RelayFailed,
		SettingsChanged,
		MissingAudio,
		TooFarBehind,
		TranscodeBehind,
		NotAllowedToPlay,
		Busy,
		Banned,
		AlreadyActive,
		ForcedOff,
		AudioBehind,
		Shutdown,
		Disconnect,
		VideoInitFailed,
		AudioInitFailed
	}
}