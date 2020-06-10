using System.Runtime.InteropServices;

namespace K6502Emu
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct DoubleRegister
	{
		[FieldOffset(0)] public ushort Whole;
		[FieldOffset(0)] public byte Lower;
		[FieldOffset(1)] public byte Upper;
	}
}
