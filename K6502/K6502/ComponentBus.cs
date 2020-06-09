using System.Collections.Generic;

namespace K6502Emu
{
	public class ComponentBus
	{
		private List<BusComponent> components;

		public byte this[ushort address]
		{
			get
			{
				byte data = 0xff;

				foreach (var compoennt in components)
					if (address.InRange(compoennt.AddressRange))
						data &= compoennt[address];

				return data;
			}

			set
			{
				foreach (var component in components)
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
