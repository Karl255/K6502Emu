using System;
using System.Reflection;

namespace K6502Emu
{
	public partial class K6502
	{
		private byte A { get; set; } = 0;
		private byte X { get; set; } = 0;
		private byte Y { get; set; } = 0;
		private ushort PC { get; set; } //program counter
		private byte S { get; set; }    //stack pointer
		private StatusRegister P = new StatusRegister();

		private byte _opCode = 0;
		private byte OpCode { get => _opCode; set { _opCode = value; OpCodeCycle = 1; } }

		private byte AddressL { get; set; } = 0;
		private byte AddressH { get; set; } = 0;
		private byte Operand { get; set; } = 0;
		private int OpCodeCycle { get; set; } = 0;

		private Action[][] Instructions;
		private ComponentBus Memory;

		public K6502(ComponentBus bus)
		{
			InitInstructions();
			
			Memory = bus;

		}

		public void Tick()
		{
			if (OpCodeCycle >= Instructions[OpCode].Length)
				OpCodeCycle = 0;
			
			Instructions[OpCode][OpCodeCycle]();
			OpCodeCycle++;

		}
	}
}
