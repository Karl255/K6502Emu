using K6502Emu;
using Xunit;

namespace UnitTests
{
	public class StatusRegisterTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterBitSetTrueTest(byte pos)
		{
			var sr = new StatusRegister
			{
				Byte = 0
			};

			switch (pos)
			{
				case 0:
					sr.Carry = true;
					break;
				case 1:
					sr.Zero = true;
					break;
				case 2:
					sr.Interrupt = true;
					break;
				case 3:
					sr.Decimal = true;
					break;
				case 4:
					sr.Break = true;
					break;
				case 6:
					sr.Overflow = true;
					break;
				case 7:
					sr.Negative = true;
					break;
				default:
					break;
			}

			Assert.Equal((byte)(1 << pos) | 0b0010_0000, sr.Byte);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterBitSetFalseTest(byte pos)
		{
			var sr = new StatusRegister
			{
				Byte = 0xff
			};

			switch (pos)
			{
				case 0:
					sr.Carry = false;
					break;
				case 1:
					sr.Zero = false;
					break;
				case 2:
					sr.Interrupt = false;
					break;
				case 3:
					sr.Decimal = false;
					break;
				case 4:
					sr.Break = false;
					break;
				case 6:
					sr.Overflow = false;
					break;
				case 7:
					sr.Negative = false;
					break;
				default:
					break;
			}

			Assert.Equal((byte)(~(byte)(1 << pos) | 0B0010_0000), sr.Byte);
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
				0 => sr.Carry,
				1 => sr.Zero,
				2 => sr.Interrupt,
				3 => sr.Decimal,
				4 => sr.Break,

				6 => sr.Overflow,
				7 => sr.Negative,
				_ => true
			});
		}
	}
}
