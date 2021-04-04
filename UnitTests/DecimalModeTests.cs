using Xunit;

namespace K6502Emu.UnitTests
{
	public class DecimalModeTests
	{
		private const bool DecimalModeEnabled = true;
		private readonly StatusRegister P = new StatusRegister(0b0000_1000);
		private byte A;

		/*
		 * yes I am aware that having to copy paste these methods is a bad way of doing this,
		 * I was just too lazy to do it the proper way
		 */

		private void DoADC(byte val)
		{
			byte carry = (byte)(P.Carry ? 1 : 0);

			if (DecimalModeEnabled && P.Decimal)
			{
				// decimal/BCD mode (for 6510)

				/*
				 * This code was taken from http://nesdev.com/6502_cpu.txt from the
				 * "Decimal mode in NMOS 6500 series" section (it's a .txt so use ctrl + f to locate it).
				 * Althogh large modifications have been made, not only because C code isn't always valid C# code,
				 * but also because it had some issues or I knew of a better way of doing it.
				 * Another useful resource for decimal mode implementation is
				 * http://www.6502.org/tutorials/decimal_mode.html#A
				 * 
				 * The results of this code has been tested against the values from
				 * http://visual6502.org/wiki/index.php?title=6502DecimalMode
				 */

				// lower nibble
				int low = (A & 0x0f) + (val & 0x0f) + carry;
				// upper nibble
				int high = (A >> 4) + (val >> 4) + (low > 0x09 ? 1 : 0);

				// the zero flag is set exactly like in decimal mode
				P.Zero = ((A + val + carry) & 0xff) != 0;

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
				int uT = A + val + carry; // sum them all as ints (so the result isn't limited to the byte range)
				int sT = (sbyte)A + (sbyte)val + (sbyte)carry; // cast all to sbyte, then sum them as ints
				P.Carry = uT > 255 || uT < 0; // if unisgned over/underflow
				P.Overflow = sT > 127 || sT < -128; // if signed over/underflow
				A = SetFlagsZN((byte)(uT & 0xff)); // lowest 8 bits go into A and set flags Zero and Negative
			}
		}

		private void DoSBC(byte val)
		{
			byte borrow = (byte)(P.Carry ? 0 : 1);

			if (DecimalModeEnabled && P.Decimal)
			{
				// decimal mode

				/*
				 * This code was taken from http://nesdev.com/6502_cpu.txt from the
				 * "Decimal mode in NMOS 6500 series" section (it's a .txt so use ctrl + f to locate it).
				 * Another useful resource for decimal mode implementation is
				 * http://www.6502.org/tutorials/decimal_mode.html#A
				 *
				 * The results of this code has been tested against the values from
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
				// binary/BCD mode
				int uT = A - val - borrow; // sum them all as ints (so the result isn't limited to the byte range)
				int sT = (sbyte)A - (sbyte)val - (sbyte)borrow; // cast all to sbyte, then sum them as ints
				P.Carry = !(uT > 255 || uT < 0); // if unisgned over/underflow
				P.Overflow = sT > 127 || sT < -128; // if signed over/underflow
				A = SetFlagsZN((byte)(uT & 0xff)); // lowest 8 bits go into A and set flags Zero and Negative
			}
		}

		private byte SetFlagsZN(byte val)
		{
			P.Zero = val == 0;
			// bit 7 can be seen as sign in 2's complement numbers
			// since the type is unsigned byte, it's fastest to check if it's over 128 (rather than isolating the bit)
			P.Negative = val >= 128;
			return val;
		}

		[Theory]
		[InlineData(0x00, 0x00, 0, 0x00, false, false, true , false)]
		[InlineData(0x79, 0x00, 1, 0x80, true , true , false, false)]
		[InlineData(0x24, 0x56, 0, 0x80, true , true , false, false)]
		[InlineData(0x93, 0x82, 0, 0x75, false, true , false, true )]
		[InlineData(0x89, 0x76, 0, 0x65, false, false, false, true )]
		[InlineData(0x89, 0x76, 1, 0x66, false, false, true , true )]
		[InlineData(0x80, 0xf0, 0, 0xd0, false, true , false, true )]
		[InlineData(0x80, 0xfa, 0, 0xe0, true , false, false, true )]
		[InlineData(0x2f, 0x4f, 0, 0x74, false, false, false, false)]
		[InlineData(0x6f, 0x00, 1, 0x76, false, false, false, false)]
		public void ADCTest(byte A, byte val, byte C, byte sum, bool negative, bool overflow, bool zero, bool carry)
		{
			this.A = A;
			P.Carry = C == 1;
			DoADC(val);

			Assert.Equal(sum.ToString("X"), this.A.ToString("X")); // convert to hex for easier reading
			Assert.Equal(negative, P.Negative);
			Assert.Equal(overflow, P.Overflow);
			Assert.Equal(zero, P.Zero);
			Assert.Equal(carry, P.Carry);
		}


		[Theory]
		[InlineData(0x00, 0x00, 0, 0x99, true , false, false, false)]
		[InlineData(0x00, 0x00, 1, 0x00, false, false, true , true )]
		[InlineData(0x00, 0x01, 1, 0x99, true , false, false, false)]
		[InlineData(0x0a, 0x00, 1, 0x0a, false, false, false, true )]
		[InlineData(0x0b, 0x00, 0, 0x0a, false, false, false, true )]
		[InlineData(0x9a, 0x00, 1, 0x9a, true , false, false, true )]
		[InlineData(0x9b, 0x00, 0, 0x9a, true , false, false, true )]
		public void SBCTest(byte A, byte val, byte C, byte diff, bool negative, bool overflow, bool zero, bool carry)
		{
			this.A = A;
			P.Carry = C == 1;
			DoSBC(val);

			Assert.Equal(diff.ToString("X"), this.A.ToString("X")); // convert to hex for easier reading
			Assert.Equal(negative, P.Negative);
			Assert.Equal(overflow, P.Overflow);
			Assert.Equal(zero, P.Zero);
			Assert.Equal(carry, P.Carry);
		}
	}
}
