using System.Runtime.InteropServices;

namespace K6502Emu
{
	// TODO: write unit tests for this
	[StructLayout(LayoutKind.Explicit)]
	public struct DoubleRegister
	{
		[FieldOffset(0)] public ushort Whole;
		[FieldOffset(0)] public byte Lower;
		[FieldOffset(1)] public byte Upper;
	}
}
