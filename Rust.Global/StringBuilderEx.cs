using System;
using System.Runtime.CompilerServices;
using System.Text;

public static class StringBuilderEx
{
	public static void Clear(this StringBuilder value)
	{
		value.Length = 0;
	}
}