namespace K6502Emu
{
	public class StatusRegister
	{
		private byte _byte = 0b0011_0100; //power up value is supposedly 0x34
		public byte Byte { get => _byte; set => _byte = (byte)(value | 0b0010_0000); }

		public bool Negative  { get => (_byte & 1 << 7) > 0; set => _byte = (byte)(_byte & ~(1 << 7) | (value ? 1 : 0) << 7); }
		public bool Overflow  { get => (_byte & 1 << 6) > 0; set => _byte = (byte)(_byte & ~(1 << 6) | (value ? 1 : 0) << 6); }
		//bit 5 is unused
		public bool Break     { get => (_byte & 1 << 4) > 0; set => _byte = (byte)(_byte & ~(1 << 4) | (value ? 1 : 0) << 4); }
		public bool Decimal   { get => (_byte & 1 << 3) > 0; set => _byte = (byte)(_byte & ~(1 << 3) | (value ? 1 : 0) << 3); }
		public bool Interrupt { get => (_byte & 1 << 2) > 0; set => _byte = (byte)(_byte & ~(1 << 2) | (value ? 1 : 0) << 2); }
		public bool Zero      { get => (_byte & 1 << 1) > 0; set => _byte = (byte)(_byte & ~(1 << 1) | (value ? 1 : 0) << 1); }
		public bool Carry     { get => (_byte & 1 << 0) > 0; set => _byte = (byte)(_byte & ~(1 << 0) | (value ? 1 : 0) << 0); }
	}
}
