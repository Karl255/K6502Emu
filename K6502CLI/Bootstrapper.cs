﻿using K6502Emu;
using System;
using System.IO;

namespace K6502CLI
{
	class Bootstrapper
	{
		// source: https://github.com/Klaus2m5/6502_65C02_functional_tests
		//private static readonly string BinaryPath = "6502_functional_test.bin";
		// "Ruud Baltissen's 8k test ROM" from http://www.6502.org/tools/emu/
		private static readonly string BinaryPath = "ttl6502.bin";
		//private static readonly string BinaryPath = "rom.bin";
		private static readonly int SkipToAddress = 0xf5d5;
		private static readonly bool DoSkip = true;

		private static K6502Emu.Memory<byte> Memory;
		private static K6502 Cpu;
		private static byte Page;

		private static readonly ConsoleColor FgColorStatic = ConsoleColor.Blue;
		private static readonly ConsoleColor FgColorDynamic = ConsoleColor.Gray;
		private static readonly ConsoleColor AccentColor = ConsoleColor.Red;

		static void Main()
		{
			InitUI();
			InitSystem(DoSkip);

			while (true)
			{
				UpdateUI();

				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.Spacebar:
						Cpu.Tick();
						break;

					case ConsoleKey.LeftArrow:
						Page--;
						break;

					case ConsoleKey.RightArrow:
						Page++;
						break;

					case ConsoleKey.R:
						InitSystem(DoSkip);
						break;

					case ConsoleKey.N: // do 10 ticks
						for (int i = 0; i < 10; i++)
							Cpu.Tick();
						break;

					case ConsoleKey.M: // do 100 ticks
						for (int i = 0; i < 100; i++)
							Cpu.Tick();
						break;

					default:
						continue;
				}
			}
		}

		private static void InitSystem(bool doSkip = false)
		{
			Memory = new(64 * 1024, File.ReadAllBytes(BinaryPath));
			Cpu = new K6502(Memory);

			if (doSkip)
				while (Cpu.GetPC != SkipToAddress)
					Cpu.Tick();
		}

		private static void InitUI()
		{
			Console.CursorVisible = false;
			Console.ForegroundColor = FgColorStatic;

			for (int i = 0; i < 16; i++)
			{
				Console.SetCursorPosition((i + 1) * 3, 0);
				Console.Write($"{i:X2}");

				Console.SetCursorPosition(0, i + 1);
				Console.Write($"{i:X}0");
			}

			// page index (higher memory address byte)
			Console.SetCursorPosition(54, 1);
			Console.Write($"Current page:");

			// program counter
			Console.SetCursorPosition(54, 3);
			Console.Write($"PC:");

			// current opcode and cycle
			Console.SetCursorPosition(54, 4);
			Console.Write($"OpCode:    Cycle:");

			// A register
			Console.SetCursorPosition(54, 5);
			Console.Write($"A:");

			// X register
			Console.SetCursorPosition(54, 6);
			Console.Write($"X:");

			// Y register
			Console.SetCursorPosition(54, 7);
			Console.Write($"Y:");

			// stack register
			Console.SetCursorPosition(54, 8);
			Console.Write($"S:");

			// status register
			Console.SetCursorPosition(63, 7);
			Console.Write("NV-BDIZC");

			Console.ForegroundColor = FgColorDynamic;

			Page = (byte)(SkipToAddress >> 8);
		}

		private static void UpdateUI()
		{
			// selected page contents
			for (int i = 0; i < 256; i++)
			{
				if ((Cpu.GetPC >> 8) == Page && (Cpu.GetPC & 0xff) == i)
				{
					Console.ForegroundColor = AccentColor;
				}

				Console.SetCursorPosition((i % 16 + 1) * 3, i / 16 + 1);
				Console.Write($"{Memory[Page * 0x0100 + i]:X2}");

				if ((Cpu.GetPC >> 8) == Page && (Cpu.GetPC & 0xff) == i)
				{
					Console.ForegroundColor = FgColorDynamic;
				}
			}

			// page index (higher memory address byte)
			Console.SetCursorPosition(68, 1);
			Console.Write($"{Page:X2}xx");

			// program counter
			Console.SetCursorPosition(58, 3);
			Console.Write($"{Cpu.GetPC:X4}");

			// current opcode and cycle
			Console.SetCursorPosition(62, 4);
			Console.Write(Cpu.GetCycle > 0 ? $"{Cpu.GetOpCode:X2}" : "??");
			Console.SetCursorPosition(72, 4);
			Console.Write($"{Cpu.GetCycle}");

			// A register
			Console.SetCursorPosition(57, 5);
			Console.Write($"{Cpu.GetA:X2}");

			// X register
			Console.SetCursorPosition(57, 6);
			Console.Write($"{Cpu.GetX:X2}");

			// Y register
			Console.SetCursorPosition(57, 7);
			Console.Write($"{Cpu.GetY:X2}");

			// stack register
			Console.SetCursorPosition(57, 8);
			Console.Write($"{Cpu.GetS:X2}");

			// status register
			Console.SetCursorPosition(63, 8);
			Console.Write(FormatStatus(Cpu.GetP));
		}

		private static string FormatStatus(byte status)
		{
			string r = "";

			for (int i = 0; i < 8; i++)
			{
				r += i == 2 ? '-' : (status & 0x80) != 0 ? '1' : '0';
				status <<= 1;
			}

			return r;
		}
	}
}
