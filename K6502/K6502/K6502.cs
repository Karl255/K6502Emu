namespace K6502Emu
{
	public class K6502
	{
		private byte A { get; set; } = 0;
		private byte X { get; set; } = 0;
		private byte Y { get; set; } = 0;
		private short PC { get; set; } = 0;
		private byte StackPointer { get; set; } = 0;
		public StatusRegister Status { get; private set; } = new StatusRegister();

		public class StatusRegister
		{
			public byte Byte { get; set; } = 0;

			public bool Negative  { get => (Byte & 1 << 0) > 0; set => Byte = (byte)(Byte & ~(1 << 0) | (value ? 1 : 0) << 0); }
			public bool Overflow  { get => (Byte & 1 << 1) > 0; set => Byte = (byte)(Byte & ~(1 << 1) | (value ? 1 : 0) << 1); }
			//bit 2 is unused
			public bool Break     { get => (Byte & 1 << 3) > 0; set => Byte = (byte)(Byte & ~(1 << 3) | (value ? 1 : 0) << 3); }
			public bool Decimal   { get => (Byte & 1 << 4) > 0; set => Byte = (byte)(Byte & ~(1 << 4) | (value ? 1 : 0) << 4); }
			public bool Interrupt { get => (Byte & 1 << 5) > 0; set => Byte = (byte)(Byte & ~(1 << 5) | (value ? 1 : 0) << 5); }
			public bool Zero      { get => (Byte & 1 << 6) > 0; set => Byte = (byte)(Byte & ~(1 << 6) | (value ? 1 : 0) << 6); }
			public bool Carry     { get => (Byte & 1 << 7) > 0; set => Byte = (byte)(Byte & ~(1 << 7) | (value ? 1 : 0) << 7); }
		}

		public K6502()
		{
			
		}
	}
}
