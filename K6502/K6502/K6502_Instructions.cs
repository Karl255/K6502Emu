/*
 * instruction references used:
 * - http://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes
 * - http://nesdev.com/6502_cpu.txt
 * - https://www.masswerk.at/6502/6502_instruction_set.html
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
			Instructions[0x10] = new Action[] { CYCLE_0, BPL_1, BPL_2, BPL_3 };
			Instructions[0x14] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3 };
			Instructions[0x18] = new Action[] { CYCLE_0, CLC_1 };
			Instructions[0x1C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4 };

		}
		/*
		 * instructions prefixed with * are unofficial/undocumented
		 */

		//cycle 0 for all instructions
		private void CYCLE_0()
		{
			//TODO: handle interrupts here
			OpCode = Memory[PC.Whole++]; //fetch opcode, inc. PC
		}

		//control instructions

		//00 BRK
		private void BRK_1() => _ = Memory[PC.Whole++];                           //read next instruction byte (throw away), inc. PC
		private void BRK_2() { Memory[0x0100 + S--] = PC.Upper; P.Break = true; } //push PCH on stack, with B set
		private void BRK_3() => Memory[0x0100 + S--] = PC.Lower;                  //push PCL on stack
		private void BRK_4() => Memory[0x0100 - S--] = P.Byte;                    //push P on stack
		private void BRK_5() => PC.Lower = Memory[0xFFFE];                        //fetch PCL
		private void BRK_6() => PC.Upper = Memory[0xFFFF];                        //fetch PCH

		//04, 44, 64
		//*NOP zpg
		private void NOP_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address, inc. PC
		private void NOP_d_2() => _ = Memory[Address.Lower];          //read from address and throw away

		//08 PHP - push processor status on stack
		private void PHP_1() => _ = Memory[PC.Whole];          //read next instruction byte (throw away)
		private void PHP_2() => Memory[0x0100 + S--] = P.Byte; //push P on stack

		//0C *NOP abs
		private void NOP_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void NOP_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch high byte of address, inc. PC
		private void NOP_a_3() => _ = Memory[Address.Whole];          //read from abs address (throw away)

		//10 BPL rel
		private void BPL_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BPL_2()
		{
			if (!P.Negative)                                  //if positive
				PC.Lower++;                                   //increment only lower byte of PC
			else
				CYCLE_0();                                    //don't branch and do cycle 0 of next instruction
		}
		private void BPL_3()
		{
			int t = PC.Lower - (sbyte)Operand;                //check if page got crossed by doing the reverse
			if (t < 0 || t > 255)                             //if t is outside 0..255 then page was crossed
			{
				if (t > 0)
					PC.Upper--;                               //move down 1 page
				else
					PC.Upper++;                               //move up 1 page
			}
			else
				CYCLE_0();                                    //if PC doesn't need adjustments, do cycle 0 of next instruction 
		}

		//14, 34, 54, 74, D4, F4
		//*NOP zpg,X
		private void NOP_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void NOP_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void NOP_dx_3() => _ = Memory[Address.Lower];                         //read from address (throw away)

		//18 CLC
		private void CLC_1() => P.Carry = false; //clear carry

		//1C *NOP abs,x //TODO: fix cycles
		private void NOP_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void NOP_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void NOP_ax_3()
		{
			_ = Memory[Address.Whole];                                 //read from effective address (throw away)
			if (X > Address.Lower)                                     //page crossed, must inc. high byte of address and repeat read
				Address.Upper++;
		}
		private void NOP_ax_4() => _ = Memory[Address.Whole];          //re-read from effective address (throw away)

		//20 JSR abs
		//24 BIT zpg
		//28 PLP
		//2C BIT abs
		//30 BMI rel
		//34 *NOP zpg,X
		//38 SEC
		//3C *NOP abs,X
		//40 RTI
		//44 *NOP zpg
		//48 PHA
		//4C JMP abs
		//50 BVC rel
		//54 *NOP zpg,X
		//58 CLI
		//5C *NOP abs,X
		//60 RTS
		//64 *NOP zpg
		//68 PLA
		//6C JMP ind
		//70 BVS rel
		//74 *NOP zpg,X
		//78 SEI
		//7C *NOP abs,X
		//80 *NOP #
		//84 STY zpg
		//88 DEY
		//8C STY abs
		//90 BCC rel
		//94 STY zpg,X
		//98 TYA
		//9C *SHY abs,X
		//A0 LDY #
		//A4 LDY zpg
		//A8 TAY
		//AC LDY abs
		//B0 BCS rel
		//B4 LDY zpg,X
		//B8 CLV
		//BC LDY abs,X
		//C0 CPY #
		//C4 CPY zpg
		//C8 INY
		//CC CPY abs
		//D0 BNE rel
		//D4 *NOP zpg,X
		//D8 CLD
		//DC *NOP abs,X
		//E0 CPX #
		//E4 CPX zpg
		//E8 INX
		//EC CPX abs
		//F0 BEQ rel
		//F4 *NOP zpg,X
		//F8 SED
		//FC *NOP abs,X

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
