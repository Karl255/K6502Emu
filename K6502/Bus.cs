using System.Collections;
using System.Collections.Generic;

namespace K6502Emu
{
	public class Bus : IEnumerable<Component>
	{
		private List<Component> Components;

		public Bus() => Components = new List<Component>();
		public Bus(IEnumerable<Component> components) => Components = new List<Component>(components);

		public void Add(Component component) => Components.Add(component);

		//implementing IEnumerable
		public IEnumerator<Component> GetEnumerator() => Components.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

		//indexers for accessing components on the bus
		public byte this[ushort address]
		{
			get
			{
				//due to open-collector drivers, multiple components outputting on the bus at once causes an "and" operation between them
				//or simlper: bus is pulled up, to set a bit to 0 they're pulled down, for 1 the output stays floating
				byte data = 0xff;

				foreach (var compoennt in Components)
					if (address.InRange(compoennt.AddressRange))
						data &= compoennt[address];

				return data;
			}

			set
			{
				foreach (var component in Components)
					if ((!component.IsReadOnly) && address.InRange(component.AddressRange))
						component[address] = value;
			}
		}

		public byte this[int address]
		{
			get => this[(ushort)address];
			set => this[(ushort)address] = value;
		}
	}
}
