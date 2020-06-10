/*
 * instruction references used:
 * - http://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes (for method names and per-cycle operations)
 * - http://nesdev.com/6502_cpu.txt
 * - https://www.masswerk.at/6502/6502_instruction_set.html (for comments and deatils about each instruction)
 * - http://6502.org/tutorials/6502opcodes.html
 */

using System;

namespace K6502Emu
{
	public partial class K6502
	{
		private void InitInstructions()
		{
			Instructions[0x00] = new Action[] { CYCLE_0, BRK_1   , BRK_2   , BRK_3   , BRK_4,    BRK_5,    BRK_6     };
			Instructions[0x04] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x08] = new Action[] { CYCLE_0, PHP_1   , PHP_2                                             };
			Instructions[0x0C] = new Action[] { CYCLE_0, NOP_a_1 , NOP_a_2 , NOP_a_3                                 };
			Instructions[0x10] = new Action[] { CYCLE_0, BPL_1   , BPL_2   , BPL_3                                   };
			Instructions[0x14] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x18] = new Action[] { CYCLE_0, CLC_1                                                       };
			Instructions[0x1C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0x20] = new Action[] { CYCLE_0, JSR_1   , JSR_2   , JSR_3   , JSR_4   , JSR_5               };
			Instructions[0x24] = new Action[] { CYCLE_0, BIT_d_1 , BIT_d_2                                           };
			Instructions[0x28] = new Action[] { CYCLE_0, PLP_1   , PLP_2   , PLP_3                                   };
			Instructions[0x2C] = new Action[] { CYCLE_0, BIT_a_1 , BIT_a_2 , BIT_a_3                                 };
			Instructions[0x30] = new Action[] { CYCLE_0, BMI_1   , BMI_2   , BMI_3                                   };
			Instructions[0x34] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x38] = new Action[] { CYCLE_0, SEC_1                                                       };
			Instructions[0x3C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0x40] = new Action[] { CYCLE_0, RTI_1   , RTI_2   , RTI_3   , RTI_4   , RTI_5               };
			Instructions[0x44] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x48] = new Action[] { CYCLE_0, PHA_1   , PHA_2                                             };
			Instructions[0x4C] = new Action[] { CYCLE_0, JMP_a_1 , JMP_a_2                                           };
			Instructions[0x50] = new Action[] { CYCLE_0, BVC_1   , BVC_2   , BVC_3                                   };
			Instructions[0x54] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x58] = new Action[] { CYCLE_0, CLI_1                                                       };
			Instructions[0x5C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };
			
			Instructions[0x60] = new Action[] { CYCLE_0, RTS_1   , RTS_2   , RTS_3   , RTS_4   , RTS_5               };
			Instructions[0x64] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x68] = new Action[] { CYCLE_0, PLA_1   , PLA_2   , PLA_3                                   };
			Instructions[0x6C] = new Action[] { CYCLE_0, JMP_i_1 , JMP_i_2 , JMP_i_3 , JMP_i_4                       };
			Instructions[0x70] = new Action[] { CYCLE_0, BVS_1   , BVS_2   , BVS_3                                   };
			Instructions[0x74] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x78] = new Action[] { CYCLE_0, SEI_1                                                       };
			Instructions[0x7C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };


		}
		//instructions prefixed with * are unofficial/undocumented

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

		//1C, 3C, 5C, 7C, DC, FC
		//*NOP abs,x //TODO: fix cycles
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
		private void JSR_1() => Address.Lower = Memory[PC.Whole++];                       //fetch low address byte, inc. PC
		private void JSR_2() { }                                                          //internal operation (predecrement S), don't emulate
		private void JSR_3() => Memory[0x0100 + S--] = PC.Upper;                          //push PCH on stack
		private void JSR_4() => Memory[0x0100 + S--] = PC.Lower;                          //push PCL on stack
		private void JSR_5() => (PC.Lower, PC.Upper) = (Address.Lower, Memory[PC.Whole]); //copy adr. low to PCL, fetch adr. high into PCH

		//24 BIT zpg
		private void BIT_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch address
		private void BIT_d_2()
		{
			byte op = Memory[Address.Lower];                          //read from zpg address
			P.Overflow = (op & (1 << 6)) > 0;                         //set V flag to bit 6 of operand
			P.Negative = (op & (1 << 7)) > 0;                         //set N flag to bit 7 of operand
			P.Zero = (op & A) == 0;                                   //and operand with A and update zero flag
		}

		//28 PLP
		private void PLP_1() => _ = Memory[PC.Whole++];      //fetch next instruction byte (throw away)
		private void PLP_2() => S++;                         //increment S
		private void PLP_3() => P.Byte = Memory[0x0100 + S]; //pull P from stack

		//2C BIT abs
		private void BIT_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void BIT_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void BIT_a_3()
		{
			byte op = Memory[Address.Whole];                          //read from zpg address
			P.Overflow = (op & (1 << 6)) > 0;                         //set V flag to bit 6 of operand
			P.Negative = (op & (1 << 7)) > 0;                         //set N flag to bit 7 of operand
			P.Zero = (op & A) == 0;                                   //and operand with A and update zero flag
		}

		//30 BMI rel
		private void BMI_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BMI_2()
		{
			if (P.Negative)                    //if negative
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BMI_3()
		{
			int t = PC.Lower - (sbyte)Operand; //check if page got crossed by doing the reverse
			if (t < 0 || t > 255)              //if t is outside 0..255 then page was crossed
			{
				if (t > 0)
					PC.Upper--;                //move down 1 page
				else
					PC.Upper++;                //move up 1 page
			}
			else
				CYCLE_0();                     //if PC doesn't need adjustments, do cycle 0 of next instruction 
		}

		//38 SEC
		private void SEC_1() => P.Carry = true; //set carry

		//40 RTI
		private void RTI_1() => _ = Memory[PC.Whole];            //read next instruction byte (throw away)
		private void RTI_2() => S++;                             //increment S
		private void RTI_3() => P.Byte = Memory[0x0100 + S++];   //pull P from stack
		private void RTI_4() => PC.Lower = Memory[0x0100 + S++]; //pull PCL from stack
		private void RTI_5() => PC.Upper = Memory[0x0100 + S];   //pull PCH from stack

		//48 PHA
		private void PHA_1() => _ = Memory[PC.Whole];     //read next instruction byte (throw away)
		private void PHA_2() => Memory[0x0100 + S--] = A; //push P on stack

		//4C JMP abs
		private void JMP_a_1() => Address.Lower = Memory[PC.Whole++];
		private void JMP_a_2() => (PC.Lower, PC.Upper) = (Address.Lower, Memory[PC.Whole]);

		//50 BVC rel
		private void BVC_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BVC_2()
		{
			if (!P.Overflow)                   //if overflow clear
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BVC_3()
		{
			int t = PC.Lower - (sbyte)Operand; //check if page got crossed by doing the reverse
			if (t < 0 || t > 255)              //if t is outside 0..255 then page was crossed
			{
				if (t > 0)
					PC.Upper--;                //move down 1 page
				else
					PC.Upper++;                //move up 1 page
			}
			else
				CYCLE_0();                     //if PC doesn't need adjustments, do cycle 0 of next instruction 
		}

		//58 CLI
		private void CLI_1() => P.Interrupt = false;

		//60 RTS
		private void RTS_1() => _ = Memory[PC.Whole];            //read next instruction byte (throw away)
		private void RTS_2() => S++;                             //increment S
		private void RTS_3() => PC.Lower = Memory[0x0100 + S++]; //pull PCL from stack
		private void RTS_4() => PC.Upper = Memory[0x0100 + S];   //pull PCH from stack
		private void RTS_5() => PC.Whole++;

		//68 PLA
		private void PLA_1() => _ = Memory[PC.Whole++]; //fetch next instruction byte (throw away)
		private void PLA_2() => S++;                    //increment S
		private void PLA_3() => A = Memory[0x0100 + S]; //pull P from stack


		//6C JMP ind
		private void JMP_i_1() => Address.Lower = Memory[PC.Whole++];
		private void JMP_i_2() => Address.Upper = Memory[PC.Whole++];
		private void JMP_i_3() => Operand = Memory[Address.Whole];
		private void JMP_i_4() => (PC.Lower, PC.Upper) = (Operand, Memory[Address.Whole + 1]);


		//70 BVS rel
		private void BVS_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BVS_2()
		{
			if (P.Overflow)                    //if overflow set
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BVS_3()
		{
			int t = PC.Lower - (sbyte)Operand; //check if page got crossed by doing the reverse
			if (t < 0 || t > 255)              //if t is outside 0..255 then page was crossed
			{
				if (t > 0)
					PC.Upper--;                //move down 1 page
				else
					PC.Upper++;                //move up 1 page
			}
			else
				CYCLE_0();                     //if PC doesn't need adjustments, do cycle 0 of next instruction 
		}

		//78 SEI
		private void SEI_1() => P.Interrupt = true;

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
