using System;

namespace K6502Emu
{
	public class Memory : Component
	{
		protected byte[] memory;

		public Memory(Range addressRange)
		{
			AddressRange = addressRange;
			memory = new byte[addressRange.End.Value - addressRange.Start.Value + 1];
		}

		public Memory(Range addressRange, byte[] data) : this(addressRange)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (i >= memory.Length)
					break;

				memory[i] = data[i];
			}
		}

		public override byte this[ushort address]
		{
			get => memory[address - AddressRange.Start.Value];
			set { if (!IsReadOnly) memory[address - AddressRange.Start.Value] = value; }
		}
	}
}
