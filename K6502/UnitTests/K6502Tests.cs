using K6502Emu;
using Xunit;

namespace UnitTests
{
	public class K6502Tests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterBitSetTrueTest(byte pos)
		{
			var sr = new StatusRegister();

			switch (pos)
			{
				case 0:
					sr.Negative = true;
					break;
				case 1:
					sr.Overflow = true;
					break;
				case 3:
					sr.Break = true;
					break;
				case 4:
					sr.Decimal = true;
					break;
				case 5:
					sr.Interrupt = true;
					break;
				case 6:
					sr.Zero = true;
					break;
				case 7:
					sr.Carry = true;
					break;
				default:
					break;
			}

			Assert.Equal((byte)(1 << pos) | 0b0000_0100, sr.Byte);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterBitSetFalseTest(byte pos)
		{
			var sr = new StatusRegister();
			sr.Byte = 0B1111_1111;

			switch (pos)
			{
				case 0:
					sr.Negative = false;
					break;
				case 1:
					sr.Overflow = false;
					break;
				case 3:
					sr.Break = false;
					break;
				case 4:
					sr.Decimal = false;
					break;
				case 5:
					sr.Interrupt = false;
					break;
				case 6:
					sr.Zero = false;
					break;
				case 7:
					sr.Carry = false;
					break;
				default:
					break;
			}

			Assert.Equal((byte)~(byte)(1 << pos), sr.Byte);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterBitGetTest(byte pos)
		{
			var sr = new StatusRegister();
			sr.Byte = (byte)(1 << pos);

			Assert.True(pos switch
			{
				0 => sr.Negative,
				1 => sr.Overflow,

				3 => sr.Break,
				4 => sr.Decimal,
				5 => sr.Interrupt,
				6 => sr.Zero,
				7 => sr.Carry,
				_ => true
			});
		}
	}
}
