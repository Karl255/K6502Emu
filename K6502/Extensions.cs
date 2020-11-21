using System;

namespace K6502Emu
{
	internal static class Extensions
	{
		public static bool IsInRange(this int @this, int start, int end) =>
			@this >= start && @this <= end;

		public static bool IsInRange(this int @this, Range range) =>
			@this.IsInRange(range.Start.Value, range.End.Value);
	}
}
