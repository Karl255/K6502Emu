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
			var cpu = new K6502();

			switch (pos)
			{
				case 0:
					cpu.Status.Negative = true;
					break;
				case 1:
					cpu.Status.Overflow = true;
					break;
				case 3:
					cpu.Status.Break = true;
					break;
				case 4:
					cpu.Status.Decimal = true;
					break;
				case 5:
					cpu.Status.Interrupt = true;
					break;
				case 6:
					cpu.Status.Zero = true;
					break;
				case 7:
					cpu.Status.Carry = true;
					break;
				default:
					break;
			}

			Assert.Equal((byte)(1 << pos), cpu.Status.Byte);
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
			var cpu = new K6502();
			cpu.Status.Byte = 0B1111_1111;

			switch (pos)
			{
				case 0:
					cpu.Status.Negative = false;
					break;
				case 1:
					cpu.Status.Overflow = false;
					break;
				case 3:
					cpu.Status.Break = false;
					break;
				case 4:
					cpu.Status.Decimal = false;
					break;
				case 5:
					cpu.Status.Interrupt = false;
					break;
				case 6:
					cpu.Status.Zero = false;
					break;
				case 7:
					cpu.Status.Carry = false;
					break;
				default:
					break;
			}

			Assert.Equal((byte)~(byte)(1 << pos), cpu.Status.Byte);
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
			var cpu = new K6502();
			cpu.Status.Byte = (byte)(1 << pos);

			Assert.True(pos switch
			{
				0 => cpu.Status.Negative,
				1 => cpu.Status.Overflow,

				3 => cpu.Status.Break,
				4 => cpu.Status.Decimal,
				5 => cpu.Status.Interrupt,
				6 => cpu.Status.Zero,
				7 => cpu.Status.Carry,
				_ => true
			});
		}
	}
}
