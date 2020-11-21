using System.Collections;
using System.Collections.Generic;

namespace K6502Emu
{
	public class Bus : IAddressable<byte>, IEnumerable<(int offset, IAddressable<byte> component)>
	{
		private List<(int offset, IAddressable<byte> component)> Components;

		public Bus(int addressableSize)
		{
			Components = new List<(int offset, IAddressable<byte> component)>();
			AddressableSize = addressableSize;
		}

		public Bus(int addressableSize, IEnumerable<(int offset, IAddressable<byte> component)> components)
		{
			Components = new List<(int offset, IAddressable<byte> component)>(components);
			AddressableSize = addressableSize;
		}

		public void Add(int offset, IAddressable<byte> component) =>
			Components.Add((offset, component));

		// implementing IEnumerable
		public IEnumerator<(int offset, IAddressable<byte> component)> GetEnumerator() =>
			Components.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			Components.GetEnumerator();

		// implementing IAddressable
		public int AddressableSize { get; private set; }
		public bool IsReadonly { get; set; }

		// indexers for accessing components on the bus
		// supports mutliple components reading from/writing to the bus at once
		public byte this[int address]
		{
			get
			{
				// due to open-collector drivers, multiple components outputting on the bus at once causes an "and" operation between them
				// or simlper: each bit is 1 by default and is 0 when one (or more) components change it to a 0
				byte data = 0xff;

				foreach (var (offset, component) in Components)
					if (address.IsInRange(offset, offset + component.AddressableSize - 1))
						data &= component[address];

				return data;
			}

			set
			{
				foreach (var (offset, component) in Components)
					if (address.IsInRange(offset, offset + component.AddressableSize - 1))
						component[address] = value;
			}
		}
	}
}
