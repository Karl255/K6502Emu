using System;

namespace K6502Emu
{
	public abstract class BusComponent
	{
		public virtual Range AddressRange { get => _addressRange; set => _addressRange = value; }
		protected Range _addressRange;
		public abstract byte this[ushort address] { get; set; }
	}
}
