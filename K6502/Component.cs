using System;

namespace K6502Emu
{
	public abstract class Component
	{
		public virtual Range AddressRange { get => _addressRange; set => _addressRange = value; }
		public bool IsReadOnly { set; get; } = false;
		protected Range _addressRange;
		public abstract byte this[ushort address] { get; set; }
	}
}
