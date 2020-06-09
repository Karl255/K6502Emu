using System;

namespace K6502Emu
{
	public class MemoryComponent : BusComponent
	{
		public override Range AddressRange { get; set; }
		private byte[] memory;

		public MemoryComponent(Range addressRange)
		{
			AddressRange = addressRange;
			memory = new byte[addressRange.End.Value - addressRange.Start.Value + 1];
		}

		public override byte this[ushort address]
		{
			get => memory[address];
			set => memory[address] = value;
		}
	}
}
