/*
 * instruction references used:
 * - http://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes
 * - http://nesdev.com/6502_cpu.txt
 */

using System;

namespace K6502Emu
{
	public partial class K6502
	{
		private void InitInstructions()
		{
			Instructions = new Action[256][];
			Instructions[0x00] = new Action[] { CYCLE_0, BRK_1, BRK_2, BRK_3, BRK_4, BRK_5, BRK_6 };
			Instructions[0x04] = new Action[] { CYCLE_0, NOP_d_1, NOP_d_2 };
			Instructions[0x08] = new Action[] { CYCLE_0, PHP_1, PHP_2 };
			Instructions[0x0C] = new Action[] { CYCLE_0, NOP_a_1, NOP_a_2, NOP_a_3 };
			//Instructions[0x10] = new Action[] { /* BPL */ };
			Instructions[0x14] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3 };
			Instructions[0x18] = new Action[] { CYCLE_0, CLC_1};
			Instructions[0x1C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4 };

		}

		/* cycle 0 for all instructions */
		private void CYCLE_0() => OpCode = Memory[PC++]; //fetch opcode, inc. PC

		/* control instructions */

		//00 BRK - break
		private void BRK_1() => _ = Memory[PC++];                                        //read next instruction byte (throw away), inc. PC
		private void BRK_2() { Memory[0x0100 + S--] = (byte)(PC >> 8); P.Break = true; } //push PCH on stack, with B set
		private void BRK_3() => Memory[0x0100 + S--] = (byte)(PC & 0xff);                //push PCL on stack
		private void BRK_4() => Memory[0x0100 - S--] = P.Byte;                           //push P on stack
		private void BRK_5() => PC = Memory[0xFFFE];                                     //fetch PCL
		private void BRK_6() => PC |= (ushort)( Memory[0xFFFF] << 8);                    //fetch PCH

		//04 NOP d - 3 cycle NOP
		private void NOP_d_1() => AddressL = Memory[PC++]; //fetch zpg address, inc. PC
		private void NOP_d_2() => _ = Memory[AddressL];    //read from address and throw away

		//08 PHP - push processor status on stack
		private void PHP_1() => _ = Memory[PC];                //read next instruction byte (throw away)
		private void PHP_2() => Memory[0x0100 + S--] = P.Byte; //push P on stack

		//0C NOP - a
		private void NOP_a_1() => AddressL = Memory[PC++];              //fetch low byte of address, inc. PC
		private void NOP_a_2() => AddressH = Memory[PC++];              //fetch high byte of address, inc. PC
		private void NOP_a_3() => _ = Memory[AddressL + AddressH << 8]; //read from abs address (throw away)

		//10 BPL
		/*
		private void BPL_1() => Operand = Memory[PC++]; //fetch operand, inc. PC
		private void BPL_2()
		{
			OpCode = Memory[PC++]; //fetch opcode, inc. PC
			//if positive, branch by relative address, otherwise inc. PC
			if (P.Negative) PC++;
			else PC += Operand;
		}
		*/

		//14 NOP d,x
		private void NOP_dx_1() => AddressL = Memory[PC++];                 //fetch address for operand, inc. PC
		private void NOP_dx_2() => AddressL = (byte)(Memory[AddressL] + X); //read operand from address and add X to it
		private void NOP_dx_3() => _ = Memory[AddressL];                    //read from address (throw away)

		//18 CLC
		public void CLC_1() => P.Carry = false;

		//1C NOP a,x
		private void NOP_ax_1() => AddressL = Memory[PC++]; //fetch low byte of address, inc. PC
		private void NOP_ax_2()
		{
			AddressH = Memory[PC++]; //fetch high byte of address
			AddressL += X;           //add X to low address byte
		}
		private void NOP_ax_3()
		{
			_ = Memory[AddressL + AddressH << 8]; //read from effective address (throw away)
			if (X > AddressL) //page crossed, must inc. high byte of address and repeat read
				AddressH++;
		}
		private void NOP_ax_4() => _ = Memory[AddressL + AddressH << 8]; //re-read from effective address (throw away)

		//20
		//24
		//28
		//2C
		//30
		//34
		//38
		//3C
		//40
		//44
		//48
		//4C
		//50
		//54
		//58
		//5C
		//60
		//64
		//68
		//6C
		//70
		//74
		//78
		//7C
		//80
		//84
		//88
		//8C
		//90
		//94
		//98
		//9C
		//A0
		//A4
		//A8
		//AC
		//B0
		//B4
		//B8
		//BC
		//C0
		//C4
		//C8
		//CC
		//D0
		//D4
		//D8
		//DC
		//E0
		//E4
		//E8
		//EC
		//F0
		//F4
		//F8
		//FC

		/* ALU operations */

		//01
		//05
		//09
		//0D
		//11
		//15
		//19
		//1D
		//21
		//25
		//29
		//2D
		//31
		//35
		//39
		//3D
		//41
		//45
		//49
		//4D
		//51
		//55
		//59
		//5D
		//61
		//65
		//69
		//6D
		//71
		//75
		//79
		//7D
		//81
		//85
		//89
		//8D
		//91
		//95
		//99
		//9D
		//A1
		//A5
		//A9
		//AD
		//B1
		//B5
		//B9
		//BD
		//C1
		//C5
		//C9
		//CD
		//D1
		//D5
		//D9
		//DD
		//E1
		//E5
		//E9
		//ED
		//F1
		//F5
		//F9
		//FD

		/* RMW operations */

		//02
		//06
		//0A
		//0E
		//12
		//16
		//1A
		//1E
		//22
		//26
		//2A
		//2E
		//32
		//36
		//3A
		//3E
		//42
		//46
		//4A
		//4E
		//52
		//56
		//5A
		//5E
		//62
		//66
		//6A
		//6E
		//72
		//76
		//7A
		//7E
		//82
		//86
		//8A
		//8E
		//92
		//96
		//9A
		//9E
		//A2
		//A6
		//AA
		//AE
		//B2
		//B6
		//BA
		//BE
		//C2
		//C6
		//CA
		//CE
		//D2
		//D6
		//DA
		//DE
		//E2
		//E6
		//EA
		//EE
		//F2
		//F6
		//FA
		//FE

		/* rest */

		//03
		//07
		//0B
		//0F
		//13
		//17
		//1B
		//1F
		//23
		//27
		//2B
		//2F
		//33
		//37
		//3B
		//3F
		//43
		//47
		//4B
		//4F
		//53
		//57
		//5B
		//5F
		//63
		//67
		//6B
		//6F
		//73
		//77
		//7B
		//7F
		//83
		//87
		//8B
		//8F
		//93
		//97
		//9B
		//9F
		//A3
		//A7
		//AB
		//AF
		//B3
		//B7
		//BB
		//BF
		//C3
		//C7
		//CB
		//CF
		//D3
		//D7
		//DB
		//DF
		//E3
		//E7
		//EB
		//EF
		//F3
		//F7
		//FB
		//FF

	}
}
