namespace K6502Emu
{
	public interface IBus
	{
		public byte this[int address] { get; set; }
		public byte this[ushort address] { get; set; }
	}
}
