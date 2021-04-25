using System.Runtime.InteropServices;

namespace K6502Emu
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DoubleRegister
	{
		[FieldOffset(0)] public byte Lower;
		[FieldOffset(1)] public byte Upper;
		[FieldOffset(0)] public ushort Whole;

		public DoubleRegister(ushort initial)
		{
			Lower = 0;
			Upper = 0;
			Whole = initial;
		}
	}
}
