using K6502Emu;
using System;

namespace K6502CLI
{
	class Bootstrapper
	{
		static void Main(string[] args)
		{
			var bus = new ComponentBus
			{
				new MemoryComponent(0x0000..0xffff)
			};

			var cpu = new K6502(bus);

			while (true)
			{
				Console.WriteLine("Tick?");
				Console.ReadLine();
				cpu.Tick();
			}

		}
	}
}
