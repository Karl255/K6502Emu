using System;

namespace K6502Emu
{
	public partial class K6502
	{
		// TODO: remove the SetFlagsZN calls
		// the flags zero and negative are set/cleared when X/Y get loaded in any way
		// A doesn't have that because of decimal mode
		protected byte A { get; set; } = 0;
		protected byte X { get => _x; set => _x = SetFlagsZN(value); }
		protected byte Y { get => _y; set => _y = SetFlagsZN(value); }
		private byte _x = 0;
		private byte _y = 0;

		protected DoubleRegister Address; // memory address register
		protected DoubleRegister PC = new DoubleRegister(0xfffc); // program counter
		protected StatusRegister P = new StatusRegister(0b0011_0100); // status register: N V - B D I Z C
		protected byte S = 0xFD; // stack pointer

		// properties for getting the internal state from outside, useful for debugging and getting insight about the execution
		// TODO: give better names to these
		public byte GetA => A;
		public byte GetX => X;
		public byte GetY => Y;
		public byte GetP => (byte)P.Byte;
		public byte GetS => S;
		public ushort GetPC => PC.Whole;
		public int GetCycle => OpCodeCycle;
		public ushort GetOpCode => OpCode;
		public IAddressable<byte> GetMemory => Memory;

		protected Action[][] Instructions = new Action[258][];
		protected IAddressable<byte> Memory;

		protected byte Operand = 0; // a register where instructions store internal data
		private int OpCodeCycle = 1;
		private ushort OpCode = 0x4C; // JMP abs at cycle 1
		protected bool instructionEnded = false;
		private bool IRQLevelDetector = false;
		private bool IRQSignal = false;
		private bool NMIEdgeDetector = false;
		private bool NMISignal = false;

		// configuration fields
		private readonly bool DecimalModeEnabled;

		/// <summary>
		/// Initializes the 6502 emulator class.
		/// </summary>
		/// <param name="addressableComponent">The <see cref="IAddressable{TDataSize}"/> connected to the data and address bus of the 6502.</param>
		/// <param name="enableDecimalMode">If decimal mode should be enabled.</param>
		public K6502(IAddressable<byte> addressableComponent, bool enableDecimalMode = true)
		{
			DecimalModeEnabled = enableDecimalMode;
			Memory = addressableComponent;

			InitInstructions();
		}

		public void Tick()
		{
			Instructions[OpCode][OpCodeCycle]();

			int t = Instructions[OpCode].Length;

			// poll for interrupts
			if (OpCode <= 255                        // don't poll if interrupt sequence
				&& ((t == 2 && OpCodeCycle == 1)     // for 2 cycle instructions, poll after 1st cycle
				|| (t > 2 && OpCodeCycle == t - 1))) // for >2 cycle instructions, poll on last cycle
			{
				IRQSignal = IRQLevelDetector;
				if (NMIEdgeDetector)
					NMISignal = true;
			}

			/*
			 * TODO: Implement "interrupt hijacknig" (NMI hijacking IRQ or BRK, IRQ hijacking BRK):
			 *       http://wiki.nesdev.com/w/index.php/CPU_interrupts#Interrupt_hijacking
			 *       Also make it configurable (enable/disable, similarly to decimal mode)
			 */

			if (OpCodeCycle == Instructions[OpCode].Length - 1 || instructionEnded)
			{
				OpCodeCycle = 0;
				instructionEnded = false;
			}
			else
				OpCodeCycle++;
		}

		public void SetIRQ() => IRQLevelDetector = true;
		public void ClearIRQ() => IRQLevelDetector = false;

		public void SetNMI() => NMIEdgeDetector = true;

		protected void EndInstruction() => instructionEnded = true;


		// helper methods for instructions
		private byte SetFlagsZN(byte val)
		{
			P.Zero = val == 0;
			// bit 7 can be seen as sign in 2's complement numbers
			// since the type is unsigned byte, it's fastest to check if it's over 128 (rather than isolating the bit)
			P.Negative = (val & 0x80) != 0;
			return val;
		}

		private void DoCompare(byte reg, byte val)
		{
			P.Carry = reg >= val;
			SetFlagsZN((byte)(reg - val));
		}

		// NOTE: BCD mode hasn't been tested too extensively
		// TODO: properly implement and extensively test BCD mode (ADC and SBC)
		private void DoADC(byte val)
		{
			byte carry = (byte)(P.Carry ? 1 : 0);

			if (DecimalModeEnabled && P.Decimal)
			{
				// decimal/BCD mode

				/*
				 * This code was taken from http://nesdev.com/6502_cpu.txt from the
				 * "Decimal mode in NMOS 6500 series" section (it's a .txt so use ctrl + f to locate it).
				 * Althogh large modifications have been made, not only because C code isn't always valid C# code,
				 * but also because it had some issues or I knew of a better way of doing it.
				 * Another useful resource for decimal mode implementation is
				 * http://www.6502.org/tutorials/decimal_mode.html#A
				 * 
				 * The results of this code has been tested against the results from
				 * http://visual6502.org/wiki/index.php?title=6502DecimalMode
				 */

				// lower nibble
				int low = (A & 0x0f) + (val & 0x0f) + carry;
				// upper nibble
				int high = (A >> 4) + (val >> 4) + (low > 0x09 ? 1 : 0);

				// the zero flag is set exactly like in decimal mode
				P.Zero = ((A + val + carry) & 0xff) == 0;

				// BCD fixup for lower nibble
				if (low > 9)
					low += 6;

				// these flags are set after the lower nibble fixup, but before the upper nibble fixup
				P.Negative = (high & 0x8) != 0;
				P.Overflow = ((((high << 4) ^ A) & 0x80) != 0) && !(((A ^ val) & 0x80) != 0);

				// BCD fixup for upper nibble
				if (high > 9)
					high += 6;

				P.Carry = high > 15;
				A = (byte)(((high & 0xf) << 4) | (low & 0xf)); // combining the lower and upper nibbles
			}
			else
			{
				// binary mode

				uint unsignedSum = (uint)A + val + carry;             // sum them all as unsigned ints (so the result isn't limited to the byte range)
				int signedSum = (sbyte)A + (sbyte)val + (sbyte)carry; // cast all to sbyte, then sum them as ints
				P.Carry = unsignedSum > 255;                          // if unisgned overflow
				P.Overflow = signedSum > 127 || signedSum < -128;     // if signed over/underflow
				A = SetFlagsZN((byte)(unsignedSum & 0xff));           // lowest 8 bits go into A, set flags Zero and Negative flags
			}
		}

		private void DoSBC(byte val)
		{
			byte borrow = (byte)(P.Carry ? 0 : 1);

			if (DecimalModeEnabled && P.Decimal)
			{
				// decimal/BCD mode

				/*
				 * This code was taken from http://nesdev.com/6502_cpu.txt from the
				 * "Decimal mode in NMOS 6500 series" section (it's a .txt so use ctrl + f to locate it).
				 * Another useful resource for decimal mode implementation is
				 * http://www.6502.org/tutorials/decimal_mode.html#A
				 *
				 * The results of this code has been tested against the results from
				 * http://visual6502.org/wiki/index.php?title=6502DecimalMode
				 */

				// lower nibble
				int low = (byte)((A & 0x0f) - (val & 0x0f) - borrow);

				// upper nibble
				int high = (byte)((A >> 4) - (val >> 4) - (low > 0xf ? 1 : 0));

				// BCD fixup for lower nibble
				if (low > 0xf)
					low -= 6;
				// BCD fixup of upper nibble
				if (high > 0xf)
					high -= 6;

				// all flags are set exactly like in binary mode
				int uT = A - val - borrow;
				int sT = (sbyte)A - (sbyte)val - (sbyte)borrow; // cast all to sbyte, then sum them as ints
				P.Carry = !(uT > 255 || uT < 0); // if unisgned over/underflow
				P.Overflow = sT > 127 || sT < -128; // if signed over/underflow

				// in decimal mode
				A = SetFlagsZN((byte)(((high & 0xf) << 4) | (low & 0xf)));
			}
			else
			{
				// binary

				int uT = A - val - borrow; // sum them all as ints (so the result isn't limited to the byte range)
				int sT = (sbyte)A - (sbyte)val - (sbyte)borrow; // cast all to sbyte, then sum them as ints
				P.Carry = !(uT > 255 || uT < 0); // if unisgned over/underflow
				P.Overflow = sT > 127 || sT < -128; // if signed over/underflow
				A = SetFlagsZN((byte)(uT & 0xff)); // lowest 8 bits go into A and set flags Zero and Negative
			}
		}

		private byte DoASL(byte val)
		{
			// shift left: C <- val <- 0
			int t = val << 1;
			P.Carry = (t & 0x100) > 0;
			val = (byte)(t & 0xff); // trimming with & just to be sure
			return SetFlagsZN(val);
		}

		private byte DoROL(byte val)
		{
			// rotate left with carry: C <- val <- C
			int t = (val << 1) | (P.Carry ? 1 : 0);
			P.Carry = (t & 0x100) > 0;
			val = (byte)(t & 0xff); // trimming with & just to be sure
			return SetFlagsZN(val);
		}

		private byte DoLSR(byte val)
		{
			// shift right: 0 -> val -> C
			int t = val >> 1;
			P.Carry = (val & 1) > 0;
			val = (byte)t;
			return SetFlagsZN(val);
		}

		private byte DoROR(byte val)
		{
			// rotate right with carry: C -> val -> C
			int t = (val >> 1) | (P.Carry ? 0x80 : 0x00);
			P.Carry = (val & 1) > 0;
			val = (byte)t;
			return SetFlagsZN(val);
		}
	}
}
