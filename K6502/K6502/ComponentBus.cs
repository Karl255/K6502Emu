using System.Collections;
using System.Collections.Generic;

namespace K6502Emu
{
	public class ComponentBus : IEnumerable<BusComponent>
	{
		private List<BusComponent> Components;

		public ComponentBus() => Components = new List<BusComponent>();
		public ComponentBus(IEnumerable<BusComponent> components) => Components = new List<BusComponent>(components);

		public void Add(BusComponent component) => Components.Add(component);

		//implementing IEnumerable
		public IEnumerator<BusComponent> GetEnumerator() => ((IEnumerable<BusComponent>)Components).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<BusComponent>)Components).GetEnumerator();

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
					if (address.InRange(component.AddressRange))
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
