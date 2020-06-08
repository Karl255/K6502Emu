namespace K6502Emu
{
	public class K6502
	{
		private byte A { get; set; } = 0;
		private byte X { get; set; } = 0;
		private byte Y { get; set; } = 0;
		private short PC { get; set; } = 0;
		private byte StackPointer { get; set; } = 0;
		private StatusRegister Status = new StatusRegister();

		public K6502()
		{
			
		}
	}
}
