﻿using System;

namespace K6502Emu
{
	public partial class K6502
	{
		//the flags zero and negative are set/cleared when A/X/Y get loaded in any way
		private byte A { get => _a; set { _a = SetFlagsOnLoad(value); } }
		private byte X { get => _x; set { _x = SetFlagsOnLoad(value); } }
		private byte Y { get => _y; set { _y = SetFlagsOnLoad(value); } }
		private byte _a = 0;
		private byte _x = 0;
		private byte _y = 0;

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

		private byte SetFlagsOnLoad(byte val)
		{
			P.Zero = val == 0;
			//bit 7 can be seen as sign in 2's complement numbers
			//since the type is unsigned byte, it's fastest to check if it's over 128 (rather than isolating the bit)
			P.Negative = val >= 128;
			return val;
		}

		private void SetFlagsOnCompare(byte reg, byte val)
		{
			P.Carry = reg >= val;
			P.Zero = reg == val;
			P.Negative = reg - val < 0;
		}

		private void SetFlagsOnADC(byte val)
		{

		}

		private void SetFlagsOnSBC(byte val)
		{

		}
	}
}
