using System;

namespace K6502Emu
{
	public partial class K6502
	{
		private byte A = 0;
		private byte X = 0;
		private byte Y = 0;
		private byte S; //stack pointer

		private StatusRegister P = new StatusRegister();
		private DoubleRegister PC = new DoubleRegister { Whole = 0 }; //program counter
		private DoubleRegister Address = new DoubleRegister { Whole = 0 };

		private byte _opCode = 0;
		private byte OpCode { get => _opCode; set { _opCode = value; OpCodeCycle = 1; } }
		private byte Operand = 0;
		private int OpCodeCycle = 0;

		private Action[][] Instructions = new Action[256][];
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
