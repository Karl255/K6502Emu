using K6502Emu;
using System;
using System.IO;

namespace K6502CLI
{
	class Bootstrapper
	{
		static void Main(string[] args)
		{
			//var data = File.ReadAllBytes("input.rom");
			var rng = new Random();
			byte[] data = new byte[0xffff + 1];
			for (int i = 0; i < data.Length; i++)
				data[i] = (byte)rng.Next();

			var mem = new K6502Emu.Memory<byte>(0xffff + 1, data);

			var bus = new Bus(0xffff + 1) // 64 kB, although it's completely irrelevant
			{
				{ 0, mem }
			};

			var cpu = new K6502(bus);

			Console.CursorVisible = false;

			for (int i = 0; i < 16; i++)
			{
				Console.SetCursorPosition((i + 1) * 3, 0);
				Console.Write($"{i:X2}");

				Console.SetCursorPosition(0, i + 1);
				Console.Write($"{i:X2}");
			}

			int page = 0;

			while (true)
			{
				// selected page
				for (int i = 0; i < 256; i++)
				{
					Console.SetCursorPosition((i % 16 + 1) * 3, i / 16 + 1);
					Console.Write($"{mem[page * 0x0100 + i]:X2}");
				}

				Console.SetCursorPosition(54, 1);
				Console.Write($"Current page: {page:X2}00");

				Console.SetCursorPosition(54, 3);
				Console.Write($"PC: {cpu.GetPC:X4}");

				Console.SetCursorPosition(54, 4);
				Console.Write($"OpCode: {cpu.GetOpCode:X2} Cycle: {cpu.GetCycle}");

				Console.SetCursorPosition(54, 5);
				Console.Write($"A: {cpu.GetA:X2}");

				Console.SetCursorPosition(54, 6);
				Console.Write($"X: {cpu.GetX:X2}");

				Console.SetCursorPosition(54, 7);
				Console.Write($"Y: {cpu.GetY:X2}");

				Console.SetCursorPosition(54, 8);
				Console.Write($"S: {cpu.GetS:X2}");

				Console.SetCursorPosition(54, 9);
				Console.Write($"P: {cpu.GetP:X2}");


				switch (Console.ReadKey().Key)
				{
					case ConsoleKey.Enter:
						cpu.Tick();
						break;
					case ConsoleKey.UpArrow:
						if (page > 0)
							page--;
						break;
					case ConsoleKey.DownArrow:
						if (page < 255)
							page++;
						break;
					default:
						continue;
				}
			}
		}
	}
}
