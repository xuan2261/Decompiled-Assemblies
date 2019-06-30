using System;

namespace Steamworks
{
	public struct AppId
	{
		public uint Value;

		public static implicit operator AppId(uint value)
		{
			return new AppId()
			{
				Value = value
			};
		}

		public static implicit operator AppId(int value)
		{
			return new AppId()
			{
				Value = (uint)value
			};
		}

		public static implicit operator UInt32(AppId value)
		{
			return value.Value;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}