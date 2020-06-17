using System;

namespace K6502Emu
{
	public class MirroredMemory : Memory
	{
		private readonly int MirrorSize;

		public MirroredMemory(Range addressRange, int mirrorSize) : base(addressRange) => MirrorSize = mirrorSize;
		public MirroredMemory(Range addressRange, byte[] data, int mirrorSize) : base(addressRange, data) => MirrorSize = mirrorSize;

		public override byte this[ushort address]
		{
			get => memory[(address - AddressRange.Start.Value) % MirrorSize];
			set { if (!IsReadOnly) memory[(address - AddressRange.Start.Value) % MirrorSize] = value; }
		}
	}
}
