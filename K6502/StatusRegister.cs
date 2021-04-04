namespace K6502Emu
{
	public class StatusRegister
	{
		public bool Carry;
		public bool Zero;
		public bool Interrupt;
		public bool Decimal;
		// bit 4 ("break" flag) kind of exists, it is only ever 0 when pushed to the stack when jumping to an interrupt routine
		// bit 5 doesn't exist        
		public bool Overflow;
		public bool Negative;

		public int Byte
		{
			get => ((Carry     ? 1 : 0) << 0)
				|  ((Zero      ? 1 : 0) << 1)
				|  ((Interrupt ? 1 : 0) << 2)
				|  ((Decimal   ? 1 : 0) << 3)
				|  (                  1 << 4)
				|  (                  1 << 5)
				|  ((Overflow  ? 1 : 0) << 6)
				|  ((Negative  ? 1 : 0) << 7);

			set
			{
				Carry     = (value & 0b0000_0001) != 0;
				Zero      = (value & 0b0000_0010) != 0;
				Interrupt = (value & 0b0000_0100) != 0;
				Decimal   = (value & 0b0000_1000) != 0;
				// bit 4 ("break" flag) is readonly, because it isn't stored in a register
				// bit 5 doesn't exist
				Overflow  = (value & 0b0100_0000) != 0;
				Negative  = (value & 0b1000_0000) != 0;
			}
		}

		public StatusRegister(byte initial)
		{
			Carry     = (initial & 0b0000_0001) != 0;
			Zero      = (initial & 0b0000_0010) != 0;
			Interrupt = (initial & 0b0000_0100) != 0;
			Decimal   = (initial & 0b0000_1000) != 0;
			// bit 4 ("break" flag) is readonly, because it isn't stored in a register
			// bit 5 doesn't exist
			Overflow  = (initial & 0b0100_0000) != 0;
			Negative  = (initial & 0b1000_0000) != 0;
		}
	}
}
