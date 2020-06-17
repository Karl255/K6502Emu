using System;

namespace K6502Emu
{
	internal static class Extensions
	{
		public static bool InRange(this ushort @this, Range range) => @this >= range.Start.Value && @this <= range.End.Value;
	}
}
