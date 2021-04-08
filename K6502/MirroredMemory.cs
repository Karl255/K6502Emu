using System;

namespace K6502Emu
{
	public class MirroredMemory<TDataSize> : IAddressable<TDataSize>
	{
		protected TDataSize[] memory;
		public int AddressableSize { get; private set; }
		public bool IsReadonly { get; set; }
		private int AddressMask { get; init; }

		/// <summary>
		/// Initializes a MirroredMemory with the specified real size and addressable size
		/// </summary>
		/// <param name="realSizeBits">The exponent for the real size (2^x B) or the number of bits in the memory address</param>
		/// <param name="addressableSize"></param>
		public MirroredMemory(int realSizeBits, int addressableSize, TDataSize[] data)
		{
			AddressMask = (int)Math.Pow(2, realSizeBits) - 1;
			memory = new TDataSize[AddressMask + 1];
			AddressableSize = addressableSize;

			if (data != null)
			{
				int end = Math.Min(memory.Length, data.Length);

				for (int i = 0; i < end; i++)
					memory[i] = data[i];
			}
		}

		public TDataSize this[int address]
		{
			get => memory[address & AddressMask];
			set => memory[address & AddressMask] = value;
		}
	}
}
