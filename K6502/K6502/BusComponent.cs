using System;

namespace K6502Emu
{
	public abstract class BusComponent
	{
		public abstract Range AddressRange { get; set; }
		public abstract byte this[ushort address] { get; set; }
	}
}
