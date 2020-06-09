using System;

namespace K6502Emu
{
	public partial class K6502
	{
		private byte A { get; set; } = 0;
		private byte X { get; set; } = 0;
		private byte Y { get; set; } = 0;
		private ushort PC { get; set; } = 0; //program counter
		private byte S { get; set; } = 0;    //stack pointer
		private StatusRegister P = new StatusRegister();

		private byte OpCode { get; set; } = 0;
		private byte AddressL { get; set; } = 0;
		private byte AddressH { get; set; } = 0;
		private byte Operand { get; set; } = 0;

		private Action[][] Instructions;
		private ComponentBus Memory;

		public K6502()
		{
			InitInstructions();
			Memory = new ComponentBus();
		}


	}
}
