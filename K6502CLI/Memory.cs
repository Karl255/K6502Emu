using System;
using K6502Emu;

namespace K6502Cli
{
	internal class Memory<TDataSize> : IAddressable<TDataSize>
	{
		protected TDataSize[] memory;
		public int AddressableSize { get; private set; }
		public bool IsReadonly { get; set; }

		public Memory(int size) =>
			memory = new TDataSize[size];

		public Memory(int size, TDataSize[] data)
		{
			memory = new TDataSize[size];
			AddressableSize = size;

			int end = Math.Min(memory.Length, data.Length);

			for (int i = 0; i < end; i++)
				memory[i] = data[i];
		}

		public TDataSize this[int address]
		{
			get => memory[address];
			set => memory[address] = value;
		}
	}
}
