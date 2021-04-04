using Xunit;

namespace K6502Emu.UnitTests
{
	public class StatusRegisterTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterSetBit1Test(byte pos)
		{
			var sr = new StatusRegister(0);

			_ = pos switch
			{
				0 => sr.Carry = true,
				1 => sr.Zero = true,
				2 => sr.Interrupt = true,
				3 => sr.Decimal = true,
				// readonly, almost always 1
				// doesn't exist
				6 => sr.Overflow = true,
				7 => sr.Negative = true,
				_ => true
			};

			Assert.Equal((byte)(1 << pos) | 0b0011_0000, sr.Byte);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterSetBit0Test(byte pos)
		{
			var sr = new StatusRegister(0xff);

			_ = pos switch
			{
				0 => sr.Carry = false,
				1 => sr.Zero = false,
				2 => sr.Interrupt = false,
				3 => sr.Decimal = false,
				// readonly, almost always 1
				// doesn't exist
				6 => sr.Overflow = false,
				7 => sr.Negative = false,
				_ => false
			};

			Assert.Equal((byte)~(1 << pos) | 0b0011_0000, sr.Byte);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterGetBit1Test(byte pos)
		{
			var sr = new StatusRegister((byte)(1 << pos));

			Assert.True(pos switch
			{
				0 => sr.Carry,
				1 => sr.Zero,
				2 => sr.Interrupt,
				3 => sr.Decimal,
				// readonly, almost always 1
				// doesn't exist
				6 => sr.Overflow,
				7 => sr.Negative,
				_ => true
			});
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(6)]
		[InlineData(7)]
		public void StatusRegisterGetBit0Test(byte pos)
		{
			var sr = new StatusRegister((byte)~(1 << pos));

			Assert.False(pos switch
			{
				0 => sr.Carry,
				1 => sr.Zero,
				2 => sr.Interrupt,
				3 => sr.Decimal,
				// readonly, almost always 1
				// doesn't exist
				6 => sr.Overflow,
				7 => sr.Negative,
				_ => false
			});
		}
	}
}
