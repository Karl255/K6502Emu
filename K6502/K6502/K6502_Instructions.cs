/*
 * instruction references used:
 * - http://wiki.nesdev.com/w/index.php/CPU_unofficial_opcodes (for method names and per-cycle operations)
 * - http://nesdev.com/6502_cpu.txt
 * - https://www.masswerk.at/6502/6502_instruction_set.html (for comments and deatils about each instruction)
 * - http://6502.org/tutorials/6502opcodes.html
 * 
 * instructions prefixed with * are unofficial/undocumented
 */

using System;

namespace K6502Emu
{
	public partial class K6502
	{
		private void InitInstructions()
		{
			//control instructions (mostly)
			Instructions[0x00] = new Action[] { CYCLE_0, BRK_1   , BRK_2   , BRK_3   , BRK_4,    BRK_5,    BRK_6     };
			Instructions[0x04] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x08] = new Action[] { CYCLE_0, PHP_1   , PHP_2                                             };
			Instructions[0x0C] = new Action[] { CYCLE_0, NOP_a_1 , NOP_a_2 , NOP_a_3                                 };
			Instructions[0x10] = new Action[] { CYCLE_0, BPL_1   , BPL_2   , BPL_3                                   };
			Instructions[0x14] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x18] = new Action[] { CYCLE_0, CLC                                                         };
			Instructions[0x1C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0x20] = new Action[] { CYCLE_0, JSR_1   , JSR_2   , JSR_3   , JSR_4   , JSR_5               };
			Instructions[0x24] = new Action[] { CYCLE_0, BIT_d_1 , BIT_d_2                                           };
			Instructions[0x28] = new Action[] { CYCLE_0, PLP_1   , PLP_2   , PLP_3                                   };
			Instructions[0x2C] = new Action[] { CYCLE_0, BIT_a_1 , BIT_a_2 , BIT_a_3                                 };
			Instructions[0x30] = new Action[] { CYCLE_0, BMI_1   , BMI_2   , BMI_3                                   };
			Instructions[0x34] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x38] = new Action[] { CYCLE_0, SEC                                                         };
			Instructions[0x3C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0x40] = new Action[] { CYCLE_0, RTI_1   , RTI_2   , RTI_3   , RTI_4   , RTI_5               };
			Instructions[0x44] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x48] = new Action[] { CYCLE_0, PHA_1   , PHA_2                                             };
			Instructions[0x4C] = new Action[] { CYCLE_0, JMP_a_1 , JMP_a_2                                           };
			Instructions[0x50] = new Action[] { CYCLE_0, BVC_1   , BVC_2   , BVC_3                                   };
			Instructions[0x54] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x58] = new Action[] { CYCLE_0, CLI                                                         };
			Instructions[0x5C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };
			
			Instructions[0x60] = new Action[] { CYCLE_0, RTS_1   , RTS_2   , RTS_3   , RTS_4   , RTS_5               };
			Instructions[0x64] = new Action[] { CYCLE_0, NOP_d_1 , NOP_d_2                                           };
			Instructions[0x68] = new Action[] { CYCLE_0, PLA_1   , PLA_2   , PLA_3                                   };
			Instructions[0x6C] = new Action[] { CYCLE_0, JMP_i_1 , JMP_i_2 , JMP_i_3 , JMP_i_4                       };
			Instructions[0x70] = new Action[] { CYCLE_0, BVS_1   , BVS_2   , BVS_3                                   };
			Instructions[0x74] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0x78] = new Action[] { CYCLE_0, SEI                                                         };
			Instructions[0x7C] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0x80] = new Action[] { CYCLE_0, NOP_im                                                      };
			Instructions[0x84] = new Action[] { CYCLE_0, STY_d_1 , STY_d_2                                           };
			Instructions[0x88] = new Action[] { CYCLE_0, DEY                                                         };
			Instructions[0x8C] = new Action[] { CYCLE_0, STY_a_1 , STY_a_2 , STY_a_3                                 };
			Instructions[0x90] = new Action[] { CYCLE_0, BCC_1   , BCC_2   , BCC_3                                   };
			Instructions[0x94] = new Action[] { CYCLE_0, STY_dx_1, STY_dx_2, STY_dx_3                                };
			Instructions[0x98] = new Action[] { CYCLE_0, TYA                                                         };
			Instructions[0x9C] = new Action[] { CYCLE_0, SHY_ax_1, SHY_ax_2, SHY_ax_3, SHY_ax_4                      };

			Instructions[0xA0] = new Action[] { CYCLE_0, LDY_im                                                      };
			Instructions[0xA4] = new Action[] { CYCLE_0, LDY_d_1 , LDY_d_2                                           };
			Instructions[0xA8] = new Action[] { CYCLE_0, TAY                                                         };
			Instructions[0xAC] = new Action[] { CYCLE_0, LDY_a_1 , LDY_a_2 , LDY_a_3                                 };
			Instructions[0xB0] = new Action[] { CYCLE_0, BCS_1   , BCS_2   , BCS_3                                   };
			Instructions[0xB4] = new Action[] { CYCLE_0, LDY_dx_1, LDY_dx_2, LDY_dx_3                                };
			Instructions[0xB8] = new Action[] { CYCLE_0, CLV                                                         };
			Instructions[0xBC] = new Action[] { CYCLE_0, LDY_ax_1, LDY_ax_2, LDY_ax_3, LDY_ax_4                      };

			Instructions[0xC0] = new Action[] { CYCLE_0, CPY_im                                                      };
			Instructions[0xC4] = new Action[] { CYCLE_0, CPY_d_1 , CPY_d_2                                           };
			Instructions[0xC8] = new Action[] { CYCLE_0, INY                                                         };
			Instructions[0xCC] = new Action[] { CYCLE_0, CPY_a_1 , CPY_a_2 , CPY_a_3                                 };
			Instructions[0xD0] = new Action[] { CYCLE_0, BNE_1   , BNE_2   , BNE_3                                   };
			Instructions[0xD4] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0xD8] = new Action[] { CYCLE_0, CLD                                                         };
			Instructions[0xDC] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			Instructions[0xE0] = new Action[] { CYCLE_0, CPX_im                                                      };
			Instructions[0xE4] = new Action[] { CYCLE_0, CPX_d_1 , CPX_d_2                                           };
			Instructions[0xE8] = new Action[] { CYCLE_0, INX                                                         };
			Instructions[0xEC] = new Action[] { CYCLE_0, CPX_a_1 , CPX_a_2 , CPX_a_3                                 };
			Instructions[0xF0] = new Action[] { CYCLE_0, BEQ_1   , BEQ_2   , BEQ_3                                   };
			Instructions[0xF4] = new Action[] { CYCLE_0, NOP_dx_1, NOP_dx_2, NOP_dx_3                                };
			Instructions[0xF8] = new Action[] { CYCLE_0, SED                                                         };
			Instructions[0xFC] = new Action[] { CYCLE_0, NOP_ax_1, NOP_ax_2, NOP_ax_3, NOP_ax_4                      };

			//ALU instructions (mostly)
			Instructions[0x01] = new Action[] { CYCLE_0, ORA_xi_1, ORA_xi_2, ORA_xi_3, ORA_xi_4, ORA_xi_5            };
			Instructions[0x05] = new Action[] { CYCLE_0, ORA_d_1 , ORA_d_2                                           };
			Instructions[0x09] = new Action[] { CYCLE_0, ORA_im                                                      };
			Instructions[0x0D] = new Action[] { CYCLE_0, ORA_a_1 , ORA_a_2 , ORA_a_3                                 };
			Instructions[0x11] = new Action[] { CYCLE_0, ORA_iy_1, ORA_iy_2, ORA_iy_3, ORA_iy_4, ORA_iy_5            };
			Instructions[0x15] = new Action[] { CYCLE_0, ORA_dx_1, ORA_dx_2, ORA_dx_3                                };
			Instructions[0x19] = new Action[] { CYCLE_0, ORA_ay_1, ORA_ay_2, ORA_ay_3, ORA_ay_4                      };
			Instructions[0x1D] = new Action[] { CYCLE_0, ORA_ax_1, ORA_ax_2, ORA_ax_3, ORA_ax_4                      };

			Instructions[0x21] = new Action[] { CYCLE_0, AND_xi_1, AND_xi_2, AND_xi_3, AND_xi_4, AND_xi_5            };
			Instructions[0x25] = new Action[] { CYCLE_0, AND_d_1 , AND_d_2                                           };
			Instructions[0x29] = new Action[] { CYCLE_0, AND_im                                                      };
			Instructions[0x2D] = new Action[] { CYCLE_0, AND_a_1 , AND_a_2 , AND_a_3                                 };
			Instructions[0x31] = new Action[] { CYCLE_0, AND_iy_1, AND_iy_2, AND_iy_3, AND_iy_4, AND_iy_5            };
			Instructions[0x35] = new Action[] { CYCLE_0, AND_dx_1, AND_dx_2, AND_dx_3                                };
			Instructions[0x39] = new Action[] { CYCLE_0, AND_ay_1, AND_ay_2, AND_ay_3, AND_ay_4                      };
			Instructions[0x3D] = new Action[] { CYCLE_0, AND_ax_1, AND_ax_2, AND_ax_3, AND_ax_4                      };

			Instructions[0x41] = new Action[] { CYCLE_0, EOR_xi_1, EOR_xi_2, EOR_xi_3, EOR_xi_4, EOR_xi_5            };
			Instructions[0x45] = new Action[] { CYCLE_0, EOR_d_1 , EOR_d_2                                           };
			Instructions[0x49] = new Action[] { CYCLE_0, EOR_im                                                      };
			Instructions[0x4D] = new Action[] { CYCLE_0, EOR_a_1 , EOR_a_2 , EOR_a_3                                 };
			Instructions[0x51] = new Action[] { CYCLE_0, EOR_iy_1, EOR_iy_2, EOR_iy_3, EOR_iy_4, EOR_iy_5            };
			Instructions[0x55] = new Action[] { CYCLE_0, EOR_dx_1, EOR_dx_2, EOR_dx_3                                };
			Instructions[0x59] = new Action[] { CYCLE_0, EOR_ay_1, EOR_ay_2, EOR_ay_3, EOR_ay_4                      };
			Instructions[0x5D] = new Action[] { CYCLE_0, EOR_ax_1, EOR_ax_2, EOR_ax_3, EOR_ax_4                      };

			Instructions[0x61] = new Action[] { CYCLE_0, ADC_xi_1, ADC_xi_2, ADC_xi_3, ADC_xi_4, ADC_xi_5            };
			Instructions[0x65] = new Action[] { CYCLE_0, ADC_d_1 , ADC_d_2                                           };
			Instructions[0x69] = new Action[] { CYCLE_0, ADC_im                                                      };
			Instructions[0x6D] = new Action[] { CYCLE_0, ADC_a_1 , ADC_a_2 , ADC_a_3                                 };
			Instructions[0x71] = new Action[] { CYCLE_0, ADC_iy_1, ADC_iy_2, ADC_iy_3, ADC_iy_4, ADC_iy_5            };
			Instructions[0x75] = new Action[] { CYCLE_0, ADC_dx_1, ADC_dx_2, ADC_dx_3                                };
			Instructions[0x79] = new Action[] { CYCLE_0, ADC_ay_1, ADC_ay_2, ADC_ay_3, ADC_ay_4                      };
			Instructions[0x7D] = new Action[] { CYCLE_0, ADC_ax_1, ADC_ax_2, ADC_ax_3, ADC_ax_4                      };

			Instructions[0x81] = new Action[] { CYCLE_0, STA_xi_1, STA_xi_2, STA_xi_3, STA_xi_4, STA_xi_5            };
			Instructions[0x85] = new Action[] { CYCLE_0, STA_d_1 , STA_d_2                                           };
			Instructions[0x89] = new Action[] { CYCLE_0, NOP_im                                                      };
			Instructions[0x8D] = new Action[] { CYCLE_0, STA_a_1 , STA_a_2 , STA_a_3                                 };
			Instructions[0x91] = new Action[] { CYCLE_0, STA_iy_1, STA_iy_2, STA_iy_3, STA_iy_4, STA_iy_5            };
			Instructions[0x95] = new Action[] { CYCLE_0, STA_dx_1, STA_dx_2, STA_dx_3                                };
			Instructions[0x99] = new Action[] { CYCLE_0, STA_ay_1, STA_ay_2, STA_ay_3, STA_ay_4                      };
			Instructions[0x9D] = new Action[] { CYCLE_0, STA_ax_1, STA_ax_2, STA_ax_3, STA_ax_4                      };

			Instructions[0xA1] = new Action[] { CYCLE_0, LDA_xi_1, LDA_xi_2, LDA_xi_3, LDA_xi_4, LDA_xi_5            };
			Instructions[0xA5] = new Action[] { CYCLE_0, LDA_d_1 , LDA_d_2                                           };
			Instructions[0xA9] = new Action[] { CYCLE_0, LDA_im                                                      };
			Instructions[0xAD] = new Action[] { CYCLE_0, LDA_a_1 , LDA_a_2 , LDA_a_3                                 };
			Instructions[0xB1] = new Action[] { CYCLE_0, LDA_iy_1, LDA_iy_2, LDA_iy_3, LDA_iy_4, LDA_iy_5            };
			Instructions[0xB5] = new Action[] { CYCLE_0, LDA_dx_1, LDA_dx_2, LDA_dx_3                                };
			Instructions[0xB9] = new Action[] { CYCLE_0, LDA_ay_1, LDA_ay_2, LDA_ay_3, LDA_ay_4                      };
			Instructions[0xBD] = new Action[] { CYCLE_0, LDA_ax_1, LDA_ax_2, LDA_ax_3, LDA_ax_4                      };

			Instructions[0xC1] = new Action[] { CYCLE_0, CMP_xi_1, CMP_xi_2, CMP_xi_3, CMP_xi_4, CMP_xi_5            };
			Instructions[0xC5] = new Action[] { CYCLE_0, CMP_d_1 , CMP_d_2                                           };
			Instructions[0xC9] = new Action[] { CYCLE_0, CMP_im                                                      };
			Instructions[0xCD] = new Action[] { CYCLE_0, CMP_a_1 , CMP_a_2 , CMP_a_3                                 };
			Instructions[0xD1] = new Action[] { CYCLE_0, CMP_iy_1, CMP_iy_2, CMP_iy_3, CMP_iy_4, CMP_iy_5            };
			Instructions[0xD5] = new Action[] { CYCLE_0, CMP_dx_1, CMP_dx_2, CMP_dx_3                                };
			Instructions[0xD9] = new Action[] { CYCLE_0, CMP_ay_1, CMP_ay_2, CMP_ay_3, CMP_ay_4                      };
			Instructions[0xDD] = new Action[] { CYCLE_0, CMP_ax_1, CMP_ax_2, CMP_ax_3, CMP_ax_4                      };

			Instructions[0xE1] = new Action[] { CYCLE_0, SBC_xi_1, SBC_xi_2, SBC_xi_3, SBC_xi_4, SBC_xi_5            };
			Instructions[0xE5] = new Action[] { CYCLE_0, SBC_d_1 , SBC_d_2                                           };
			Instructions[0xE9] = new Action[] { CYCLE_0, SBC_im                                                      };
			Instructions[0xED] = new Action[] { CYCLE_0, SBC_a_1 , SBC_a_2 , SBC_a_3                                 };
			Instructions[0xF1] = new Action[] { CYCLE_0, SBC_iy_1, SBC_iy_2, SBC_iy_3, SBC_iy_4, SBC_iy_5            };
			Instructions[0xF5] = new Action[] { CYCLE_0, SBC_dx_1, SBC_dx_2, SBC_dx_3                                };
			Instructions[0xF9] = new Action[] { CYCLE_0, SBC_ay_1, SBC_ay_2, SBC_ay_3, SBC_ay_4                      };
			Instructions[0xFD] = new Action[] { CYCLE_0, SBC_ax_1, SBC_ax_2, SBC_ax_3, SBC_ax_4                      };

			//RMW instructions (mostly)
			Instructions[0x02] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x06] = new Action[] { CYCLE_0, ASL_z_1 , ASL_z_2 , ASL_z_3 , ASL_z_4                       };
			Instructions[0x0A] = new Action[] { CYCLE_0, ASL                                                         };
			Instructions[0x0E] = new Action[] { CYCLE_0, ASL_a_1 , ASL_a_2 , ASL_a_3 , ASL_a_4 , ASL_a_5             };
			Instructions[0x12] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x16] = new Action[] { CYCLE_0, ASL_zx_1, ASL_zx_2, ASL_zx_3, ASL_zx_4, ASL_zx_5            };
			Instructions[0x1A] = new Action[] { CYCLE_0, NOP                                                         };
			Instructions[0x1E] = new Action[] { CYCLE_0, ASL_ax_1, ASL_ax_2, ASL_ax_3, ASL_ax_4, ASL_ax_5, ASL_ax_6  };

			Instructions[0x22] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x26] = new Action[] { CYCLE_0, ROL_z_1 , ROL_z_2 , ROL_z_3 , ROL_z_4                       };
			Instructions[0x2A] = new Action[] { CYCLE_0, ROL                                                         };
			Instructions[0x2E] = new Action[] { CYCLE_0, ROL_a_1 , ROL_a_2 , ROL_a_3 , ROL_a_4 , ROL_a_5             };
			Instructions[0x32] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x36] = new Action[] { CYCLE_0, ROL_zx_1, ROL_zx_2, ROL_zx_3, ROL_zx_4, ROL_zx_5            };
			Instructions[0x3A] = new Action[] { CYCLE_0, NOP                                                         };
			Instructions[0x3E] = new Action[] { CYCLE_0, ROL_ax_1, ROL_ax_2, ROL_ax_3, ROL_ax_4, ROL_ax_5, ROL_ax_6  };
			
			Instructions[0x42] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x46] = new Action[] { CYCLE_0, LSR_z_1 , LSR_z_2 , LSR_z_3 , LSR_z_4                       };
			Instructions[0x4A] = new Action[] { CYCLE_0, LSR                                                         };
			Instructions[0x4E] = new Action[] { CYCLE_0, LSR_a_1 , LSR_a_2 , LSR_a_3 , LSR_a_4 , LSR_a_5             };
			Instructions[0x52] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x56] = new Action[] { CYCLE_0, LSR_zx_1, LSR_zx_2, LSR_zx_3, LSR_zx_4, LSR_zx_5            };
			Instructions[0x5A] = new Action[] { CYCLE_0, NOP                                                         };
			Instructions[0x5E] = new Action[] { CYCLE_0, LSR_ax_1, LSR_ax_2, LSR_ax_3, LSR_ax_4, LSR_ax_5, LSR_ax_6  };
			
			Instructions[0x62] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x66] = new Action[] { CYCLE_0, ROR_z_1 , ROR_z_2 , ROR_z_3 , ROR_z_4                       };
			Instructions[0x6A] = new Action[] { CYCLE_0, ROR                                                         };
			Instructions[0x6E] = new Action[] { CYCLE_0, ROR_a_1 , ROR_a_2 , ROR_a_3 , ROR_a_4 , ROR_a_5             };
			Instructions[0x72] = new Action[] { CYCLE_0, STP                                                         };
			Instructions[0x76] = new Action[] { CYCLE_0, ROR_zx_1, ROR_zx_2, ROR_zx_3, ROR_zx_4, ROR_zx_5            };
			Instructions[0x7A] = new Action[] { CYCLE_0, NOP                                                         };
			Instructions[0x7E] = new Action[] { CYCLE_0, ROR_ax_1, ROR_ax_2, ROR_ax_3, ROR_ax_4, ROR_ax_5, ROR_ax_6  };




		}

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
		private void BRK_4() => Memory[0x0100 + S--] = P.Byte;                    //push P on stack
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
		private void CLC() => P.Carry = false; //clear carry

		//1C, 3C, 5C, 7C, DC, FC
		//*NOP abs,x
		private void NOP_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void NOP_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void NOP_ax_3()
		{
			_ = Memory[Address.Whole];                                 //read from effective address (throw away)
			if (X > Address.Lower)                                     //if page crossed, fix high byte of address
				Address.Upper++;
		}
		private void NOP_ax_4()
		{
			if (X > Address.Lower)
				_ = Memory[Address.Whole];                             //re-read from effective address (throw away) and add a cycle
			else
				CYCLE_0();                                             //don't add cycle, do cycle 0 of next instruction
		}

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
			byte op = Memory[Address.Whole];                          //read from effective address
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
		private void SEC() => P.Carry = true; //set carry

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
		private void CLI() => P.Interrupt = false;

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
		private void SEI() => P.Interrupt = true; //set interrupt disable flag

		//80 *NOP #
		private void NOP_im() => _ = Memory[PC.Whole++]; //read operand (throw away)

		//84 STY zpg
		private void STY_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void STY_d_2() => Memory[Address.Lower] = Y;          //stora Y at zpg address

		//88 DEY
		private void DEY() => Y--; //decrement Y

		//8C STY abs
		private void STY_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch low address byte, inc. PC
		private void STY_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch high address byte, inc. PC
		private void STY_a_3() => Memory[Address.Whole] = Y;          //store Y at address

		//90 BCC rel
		private void BCC_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BCC_2()
		{
			if (!P.Carry)                      //if carry clear
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BCC_3()
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

		//94 STY zpg,X
		private void STY_dx_1() => Operand = Memory[PC.Whole++];                //fetch address, inc. PC
		private void STY_dx_2() => Address.Lower = (byte)(Memory[Operand] + X); //read from address and add X to it
		private void STY_dx_3() => Memory[Address.Lower] = Y;                   //store Y at effective address

		//98 TYA
		private void TYA() => A = Y; //transfer Y to A

		//9C *SHY abs,X
		private void SHY_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void SHY_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void SHY_ax_3()
		{
			_ = Memory[Address.Whole];                                 //read from effective address (throw away)
			if (X > Address.Lower)                                     //page crossed, must inc. high byte of address and repeat read
				Address.Upper++;
		}
		private void SHY_ax_4() =>
			Memory[Address.Whole] = (byte)(Y & (Address.Upper + 1));   //write to effective address

		//A0 LDY #
		private void LDY_im() => Y = Memory[PC.Whole++]; //load Y from operand, inc. PC

		//A4 LDY zpg
		private void LDY_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void LDY_d_2() => Y = Memory[Address.Lower];          //load Y from zpg address

		//A8 TAY
		private void TAY() => Y = A; //transfer A to Y

		//AC LDY abs
		private void LDY_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void LDY_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void LDY_a_3() => Y = Memory[Address.Whole];          //load Y from effective address

		//B0 BCS rel
		private void BCS_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BCS_2()
		{
			if (P.Carry)                       //if carry set
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BCS_3()
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

		//B4 LDY zpg,X
		private void LDY_dx_1() => Operand = Memory[PC.Whole++];                //fetch address, inc. PC
		private void LDY_dx_2() => Address.Lower = (byte)(Memory[Operand] + X); //read from address and add X to it
		private void LDY_dx_3() => Memory[Address.Lower] = Y;                   //load Y from effective address

		//B8 CLV
		private void CLV() => P.Overflow = false;

		//BC LDY abs,X
		private void LDY_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void LDY_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void LDY_ax_3()
		{
			Y = Memory[Address.Whole];                                 //load Y from effective address
			if (X > Address.Lower)                                     //page crossed, must inc. high byte of address and repeat read
				Address.Upper++;
		}
		private void LDY_ax_4() => Y = Memory[Address.Whole];          //re-load Y from effective address

		//C0 CPY #
		private	void CPY_im() => DoCompare(Y, Memory[PC.Whole++]); //compare Y and immediate operand

		//C4 CPY zpg
		private void CPY_d_1() => Address.Lower = Memory[PC.Whole++];          //fetch zpg address
		private void CPY_d_2() => DoCompare(Y, Memory[Address.Lower]); //compare Y and value at zpg address

		//C8 INY
		private void INY() => Y++; //increment Y

		//CC CPY abs
		private void CPY_a_1() => Address.Lower = Memory[PC.Whole++];
		private void CPY_a_2() => Address.Upper = Memory[PC.Whole++];
		private void CPY_a_3() => DoCompare(Y, Memory[Address.Whole]);

		//D0 BNE rel
		private void BNE_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BNE_2()
		{
			if (!P.Zero)                       //if zero flag clear
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BNE_3()
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

		//D8 CLD
		private void CLD() => P.Decimal = false;

		//E0 CPX #
		private void CPX_im() => DoCompare(X, Memory[PC.Whole++]); //compare X and immediate operand

		//E4 CPX zpg
		private void CPX_d_1() => Address.Lower = Memory[PC.Whole++];          //fetch zpg address
		private void CPX_d_2() => DoCompare(X, Memory[Address.Lower]); //compare X and value at zpg address

		//E8 INX
		private void INX() => X++; //increment X

		//EC CPX abs
		private void CPX_a_1() => Address.Lower = Memory[PC.Whole++];
		private void CPX_a_2() => Address.Upper = Memory[PC.Whole++];
		private void CPX_a_3() => DoCompare(X, Memory[Address.Whole]);

		//F0 BEQ rel
		private void BEQ_1() => Operand = Memory[PC.Whole++]; //fetch operand, inc. PC
		private void BEQ_2()
		{
			if (P.Zero)                        //if zero flag set
				PC.Lower++;                    //increment only lower byte of PC
			else
				CYCLE_0();                     //don't branch and do cycle 0 of next instruction
		}
		private void BEQ_3()
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

		//F8 SED
		private void SED() => P.Decimal = true;


		/* ALU operations */

		//01 ORA x,ind
		private void ORA_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void ORA_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void ORA_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void ORA_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void ORA_xi_5() => A |= Memory[Address.Whole];            //read from effective address and OR with A

		//05 ORA zpg
		private void ORA_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void ORA_d_2() => A |= Memory[Address.Lower];         //read from zpg address and OR with A

		//09 ORA #
		private void ORA_im() => A |= Memory[PC.Whole++]; //read operand, inc. PC and OR with A

		//0D ORA abs
		private void ORA_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void ORA_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void ORA_a_3() => A |= Memory[Address.Whole];         //read from effective address and OR with A

		//11 ORA ind,y
		private void ORA_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void ORA_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void ORA_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void ORA_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				A |= Memory[Address.Whole];                         //read from effective address and OR with A
			}
		}
		private void ORA_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A |= Memory[Address.Whole];                         //add another cycle, read from effective address and OR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//15 ORA zpg,x
		private void ORA_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void ORA_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void ORA_dx_3() => A |= Memory[Address.Lower];                        //read from address and OR with A

		//19 ORA abs,y
		private void ORA_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ORA_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void ORA_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A |= Memory[Address.Whole];                            //read from effective address and OR with A
			}
		}
		private void ORA_ay_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A |= Memory[Address.Whole];                         //add another cycle, read from effective address and OR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//1D ORA abs,x
		private void ORA_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ORA_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void ORA_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A |= Memory[Address.Whole];                            //read from effective address and OR with A
			}
		}
		private void ORA_ax_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A |= Memory[Address.Whole];                         //add another cycle, read from effective address and OR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}


		//NOTE: the following code (until the RMW instructions) was automatically generated

		//21 AND x,ind
		private void AND_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void AND_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void AND_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void AND_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void AND_xi_5() => A &= Memory[Address.Whole];            //read from effective address and AND with A

		//25 AND zpg
		private void AND_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void AND_d_2() => A &= Memory[Address.Lower];         //read from zpg address and AND with A

		//29 AND #
		private void AND_im() => A &= Memory[PC.Whole++]; //read operand, inc. PC and AND with A

		//2D AND abs
		private void AND_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void AND_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void AND_a_3() => A &= Memory[Address.Whole];         //read from effective address and AND with A

		//31 AND ind,y
		private void AND_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void AND_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void AND_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void AND_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				A &= Memory[Address.Whole];                         //read from effective address and AND with A
			}
		}
		private void AND_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A &= Memory[Address.Whole];                         //add another cycle, read from effective address and AND with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//35 AND zpg,x
		private void AND_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void AND_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void AND_dx_3() => A &= Memory[Address.Lower];                        //read from address and AND with A

		//39 AND abs,y
		private void AND_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void AND_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void AND_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A &= Memory[Address.Whole];                            //read from effective address and AND with A
			}
		}
		private void AND_ay_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A &= Memory[Address.Whole];                         //add another cycle, read from effective address and AND with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//3D AND abs,x
		private void AND_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void AND_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void AND_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A &= Memory[Address.Whole];                            //read from effective address and AND with A
			}
		}
		private void AND_ax_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A &= Memory[Address.Whole];                         //add another cycle, read from effective address and AND with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}


		//41 EOR x,ind
		private void EOR_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void EOR_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void EOR_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void EOR_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void EOR_xi_5() => A ^= Memory[Address.Whole];            //read from effective address and XOR with A

		//45 EOR zpg
		private void EOR_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void EOR_d_2() => A ^= Memory[Address.Lower];         //read from zpg address and XOR with A

		//49 EOR #
		private void EOR_im() => A ^= Memory[PC.Whole++]; //read operand, inc. PC and XOR with A

		//4D EOR abs
		private void EOR_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void EOR_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void EOR_a_3() => A ^= Memory[Address.Whole];         //read from effective address and XOR with A

		//51 EOR ind,y
		private void EOR_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void EOR_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void EOR_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void EOR_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				A ^= Memory[Address.Whole];                         //read from effective address and XOR with A
			}
		}
		private void EOR_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A ^= Memory[Address.Whole];                         //add another cycle, read from effective address and XOR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//55 EOR zpg,x
		private void EOR_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void EOR_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void EOR_dx_3() => A ^= Memory[Address.Lower];                        //read from address and XOR with A

		//59 EOR abs,y
		private void EOR_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void EOR_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void EOR_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A ^= Memory[Address.Whole];                            //read from effective address and XOR with A
			}
		}
		private void EOR_ay_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A ^= Memory[Address.Whole];                         //add another cycle, read from effective address and XOR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//5D EOR abs,x
		private void EOR_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void EOR_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void EOR_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A ^= Memory[Address.Whole];                            //read from effective address and XOR with A
			}
		}
		private void EOR_ax_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A ^= Memory[Address.Whole];                         //add another cycle, read from effective address and XOR with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}


		//61 ADC x,ind
		private void ADC_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void ADC_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void ADC_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void ADC_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void ADC_xi_5() => DoADC(Memory[Address.Whole]);          //read from effective address and perform addition

		//65 ADC zpg
		private void ADC_d_1() => Address.Lower = Memory[PC.Whole++];   //fetch zpg address
		private void ADC_d_2() => DoADC(Memory[Address.Lower]);         //read from zpg address and perform addition

		//69 ADC #
		private void ADC_im() => DoADC(Memory[PC.Whole++]); //read operand, inc. PC and perform addition

		//6D ADC abs
		private void ADC_a_1() => Address.Lower = Memory[PC.Whole++];   //fetch address lower
		private void ADC_a_2() => Address.Upper = Memory[PC.Whole++];   //fetch address upper
		private void ADC_a_3() => DoADC(Memory[Address.Whole]);         //read from effective address and perform addition

		//71 ADC ind,y
		private void ADC_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void ADC_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void ADC_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void ADC_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				DoADC(Memory[Address.Whole]);                       //read from effective address and perform addition
			}
		}
		private void ADC_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				DoADC(Memory[Address.Whole]);                       //add another cycle, read from effective address and perform addition
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//75 ADC zpg,x
		private void ADC_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void ADC_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void ADC_dx_3() => DoADC(Memory[Address.Lower]);                      //read from address and perform addition

		//79 ADC abs,y
		private void ADC_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ADC_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void ADC_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoADC(Memory[Address.Whole]);                          //read from effective address and perform addition
			}
		}
		private void ADC_ay_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoADC(Memory[Address.Whole]);                          //add another cycle, read from effective address and perform addition
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}

		//7D ADC abs,x
		private void ADC_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ADC_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void ADC_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoADC(Memory[Address.Whole]);                          //read from effective address and perform addition
			}
		}
		private void ADC_ax_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoADC(Memory[Address.Whole]);                          //add another cycle, read from effective address and perform addition
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}


		//81 STA x,ind
		private void STA_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void STA_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void STA_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void STA_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void STA_xi_5() => Memory[Address.Whole] = A;             //store A at effective address

		//85 STA zpg
		private void STA_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void STA_d_2() => Memory[Address.Lower] = A;          //store A at zpg address

		/* *NOP #
		//89 STA #
		private void STA_im() => Memory[PC.Whole++] = A; //read operand, inc. PC and STORE with A
		*/

		//8D STA abs
		private void STA_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void STA_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void STA_a_3() => Memory[Address.Whole] = A;          //store A at effective address

		//91 STA ind,y
		private void STA_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void STA_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void STA_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void STA_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				Memory[Address.Whole] = A;                          //store A at effective address
			}
		}
		private void STA_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				Memory[Address.Whole] = A;                          //add another cycle and store A effective address
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//95 STA zpg,x
		private void STA_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void STA_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void STA_dx_3() => Memory[Address.Lower] = A;                         //store A at address

		//99 STA abs,y
		private void STA_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void STA_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void STA_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				Memory[Address.Whole] = A;                            //read from effective address and STORE with A
			}
		}
		private void STA_ay_4()
		{
			if (Y > Address.Lower)                                    //if page crossed
				Memory[Address.Whole] = A;                            //add another cycle and store A at effective address
			else
				CYCLE_0();                                            //don't add another cycle, do cycle 0 of next instruction
		}

		//9D STA abs,x
		private void STA_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void STA_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void STA_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				Memory[Address.Whole] = A;                             //store A at effective address
			}
		}
		private void STA_ax_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				Memory[Address.Whole] = A;                             //add another cycle and store A at effective address
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}


		//A1 LDA x,ind
		private void LDA_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void LDA_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void LDA_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void LDA_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void LDA_xi_5() => A = Memory[Address.Whole];             //load A from effective address

		//A5 LDA zpg
		private void LDA_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void LDA_d_2() => A = Memory[Address.Lower];          //load A from from zpg address

		//A9 LDA #
		private void LDA_im() => A = Memory[PC.Whole++]; //load A from operand and inc. PC

		//AD LDA abs
		private void LDA_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void LDA_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void LDA_a_3() => A = Memory[Address.Whole];          //load A from effective address

		//B1 LDA ind,y
		private void LDA_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void LDA_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void LDA_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void LDA_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				A = Memory[Address.Whole];                          //load A from effective address
			}
		}
		private void LDA_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				A = Memory[Address.Whole];                          //add another cycle and load A from effective address
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//B5 LDA zpg,x
		private void LDA_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void LDA_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void LDA_dx_3() => A = Memory[Address.Lower];                         //load A from address

		//B9 LDA abs,y
		private void LDA_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void LDA_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void LDA_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A = Memory[Address.Whole];                             //load A from effective address
			}
		}
		private void LDA_ay_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				A = Memory[Address.Whole];                             //add another cycle and load A from effective address
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}

		//BD LDA abs,x
		private void LDA_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void LDA_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void LDA_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				A = Memory[Address.Whole];                             //load A from effective address
			}
		}
		private void LDA_ax_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				A = Memory[Address.Whole];                             //add another cycle and load A from effective address
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}


		//C1 CMP x,ind
		private void CMP_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void CMP_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void CMP_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void CMP_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void CMP_xi_5() => DoCompare(A, Memory[Address.Whole]);   //read from effective address and compare with A

		//C5 CMP zpg
		private void CMP_d_1() => Address.Lower = Memory[PC.Whole++];  //fetch zpg address
		private void CMP_d_2() => DoCompare(A, Memory[Address.Lower]); //read from zpg address and compare with A

		//C9 CMP #
		private void CMP_im() => DoCompare(A, Memory[PC.Whole++]); //read operand, inc. PC and compare with A

		//CD CMP abs
		private void CMP_a_1() => Address.Lower = Memory[PC.Whole++];  //fetch address lower
		private void CMP_a_2() => Address.Upper = Memory[PC.Whole++];  //fetch address upper
		private void CMP_a_3() => DoCompare(A, Memory[Address.Whole]); //read from effective address and compare with A

		//D1 CMP ind,y
		private void CMP_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void CMP_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void CMP_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void CMP_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				DoCompare(A, Memory[Address.Whole]);                //read from effective address and compare with A
			}
		}
		private void CMP_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				DoCompare(A, Memory[Address.Whole]);                //add another cycle, read from effective address and compare with A
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//D5 CMP zpg,x
		private void CMP_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void CMP_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void CMP_dx_3() => DoCompare(A, Memory[Address.Lower]);               //read from address and compare with A

		//D9 CMP abs,y
		private void CMP_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void CMP_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void CMP_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoCompare(A, Memory[Address.Whole]);                   //read from effective address and compare with A
			}
		}
		private void CMP_ay_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoCompare(A, Memory[Address.Whole]);                   //add another cycle, read from effective address and compare with A
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}

		//DD CMP abs,x
		private void CMP_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void CMP_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void CMP_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoCompare(A, Memory[Address.Whole]);                   //read from effective address and compare with A
			}
		}
		private void CMP_ax_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoCompare(A, Memory[Address.Whole]);                   //add another cycle, read from effective address and compare with A
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}


		//E1 SBC x,ind
		private void SBC_xi_1() => Operand = Memory[PC.Whole++];          //fetch pointer address, inc. PC
		private void SBC_xi_2() => Operand = (byte)(Memory[Operand] + X); //read from pointer and add X to it
		private void SBC_xi_3() => Address.Lower = Memory[Operand];       //fetch effective address low
		private void SBC_xi_4() => Address.Upper = Memory[Operand];       //fetch effective address high
		private void SBC_xi_5() => DoSBC(Memory[Address.Whole]);          //read from effective address and perform subtraction

		//E5 SBC zpg
		private void SBC_d_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address
		private void SBC_d_2() => DoSBC(Memory[Address.Lower]);       //read from zpg address and perform subtraction

		//E9 SBC #
		private void SBC_im() => DoSBC(Memory[PC.Whole++]); //read operand, inc. PC and perform subtraction

		//ED SBC abs
		private void SBC_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower
		private void SBC_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper
		private void SBC_a_3() => DoSBC(Memory[Address.Whole]);       //read from effective address and perform subtraction

		//F1 SBC ind,y
		private void SBC_iy_1() => Operand = Memory[PC.Whole++];    //fetch pointer address
		private void SBC_iy_2() => Address.Lower = Memory[Operand]; //fetch effective address lower
		private void SBC_iy_3()
		{
			Address.Upper = Memory[Operand + 1];                    //fetch effective address upper
			Address.Lower += Y;                                     //add Y to address lower
		}
		private void SBC_iy_4()
		{
			if (Y > Address.Lower)                                  //if page crossed
			{
				_ = Memory[Address.Whole];                          //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                    //fix upper byte of address
			}
			else
			{
				DoSBC(Memory[Address.Whole]);                       //read from effective address and perform subtraction
			}
		}
		private void SBC_iy_5()
		{
			if (Y > Address.Lower)                                  //if page crossed
				DoSBC(Memory[Address.Whole]);                       //add another cycle, read from effective address and perform subtraction
			else
				CYCLE_0();                                          //don't add another cycle, do cycle 0 of next instruction
		}

		//F5 SBC zpg,x
		private void SBC_dx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address for operand, inc. PC
		private void SBC_dx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read operand from address and add X to it
		private void SBC_dx_3() => DoSBC(Memory[Address.Lower]);                      //read from address and perform subtraction

		//F9 SBC abs,y
		private void SBC_ay_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void SBC_ay_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += Y;                                        //add Y to low address byte
		}
		private void SBC_ay_3()
		{
			if (Y > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoSBC(Memory[Address.Whole]);                          //read from effective address and perform subtraction
			}
		}
		private void SBC_ay_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoSBC(Memory[Address.Whole]);                          //add another cycle, read from effective address and perform subtraction
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}

		//FD SBC abs,x
		private void SBC_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void SBC_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address
			Address.Lower += X;                                        //add X to low address byte
		}
		private void SBC_ax_3()
		{
			if (X > Address.Lower)                                     //if page crossed
			{
				_ = Memory[Address.Whole];                             //read from effective address (throw away, because it's invalid)
				Address.Upper++;                                       //fix upper byte of address
			}
			else
			{
				DoSBC(Memory[Address.Whole]);                          //read from effective address and perform subtraction
			}
		}
		private void SBC_ax_4()
		{
			if (Y > Address.Lower)                                     //if page crossed
				DoSBC(Memory[Address.Whole]);                          //add another cycle, read from effective address and perform subtraction
			else
				CYCLE_0();                                             //don't add another cycle, do cycle 0 of next instruction
		}




		/* RMW operations */

		//02, 12, 22, 32, 42, 52, 62, 72, 92, B2, D2, F2
		//*STP TODO: implement
		private void STP() => throw new NotImplementedException();

		//06 ASL zpg
		private void ASL_z_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address, inc. PC
		private void ASL_z_2() => Operand = Memory[Address.Lower];    //read from zpg address
		private void ASL_z_3()
		{
			Memory[Address.Lower] = Operand;                          //write same value back to zpg address
			Operand = DoASL(Operand);                                 //perform ASL operation
		}

		private void ASL_z_4() => Memory[Address.Lower] = Operand;    //write new value back to zpg address

		//0A ASL
		private void ASL()
		{
			_ = Memory[PC.Whole]; //read next instruction byte (thorw away)
			A = DoASL(A);         //perform ASL
		}

		//0E ASL abs
		private void ASL_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower, inc. PC
		private void ASL_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper, inc. PC
		private void ASL_a_3() => Operand = Memory[Address.Whole];    //read from effective address
		private void ASL_a_4()
		{
			Memory[Address.Whole] = Operand;                          //write same value back to effective address
			Operand = DoASL(Operand);                                 //perform ASL operation
		}
		private void ASL_a_5() => Memory[Address.Whole] = Operand;    //write new value back to effective address

		//16 ASL zpg,x
		private void ASL_zx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address, inc. PC
		private void ASL_zx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read from addres, add X to it
		private void ASL_zx_3() => Operand = Memory[Address.Lower];                   //read from effective address
		private void ASL_zx_4()
		{
			Memory[Address.Whole] = Operand;                                          //write same value back to effective address
			Operand = DoASL(Operand);                                                 //perform ASL operation
		}
		private void ASL_zx_5() => Memory[Address.Lower] = Operand;                   //write new value back to effective address

		//1A, 3A, 5A, 7A, DA, EA, FA
		//*NOP
		private void NOP() => _ = Memory[PC.Whole]; //read next instruction byte (throw away)

		//1E ASL abs,x
		private void ASL_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ASL_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address, inc. PC
			Address.Lower += X;                                        //add X to low address byte
		}
		private void ASL_ax_3()
		{
			Operand = Memory[Address.Whole];                           //read from effective address (invalid)
			if (X > Address.Lower)                                     //if page crossed
				Address.Upper++;                                       //fix upper byte of address
		}
		private void ASL_ax_4()
		{
			Operand = Memory[Address.Whole];                           //re-read from effective address
		}
		private void ASL_ax_5()
		{
			Memory[Address.Whole] = Operand;                           //write some value back to effective address
			Operand = DoASL(Operand);                                  //perform ASL operation
		}
		private void ASL_ax_6() => Memory[Address.Lower] = Operand;    //write new value back to effective address


		//26 ROL zpg
		private void ROL_z_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address, inc. PC
		private void ROL_z_2() => Operand = Memory[Address.Lower];    //read from zpg address
		private void ROL_z_3()
		{
			Memory[Address.Lower] = Operand;                          //write same value back to zpg address
			Operand = DoROL(Operand);                                 //perform ROL operation
		}

		private void ROL_z_4() => Memory[Address.Lower] = Operand;    //write new value back to zpg address

		//2A ROL
		private void ROL()
		{
			_ = Memory[PC.Whole]; //read next instruction byte (thorw away)
			A = DoROL(A);         //perform ROL
		}

		//2E ROL abs
		private void ROL_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower, inc. PC
		private void ROL_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper, inc. PC
		private void ROL_a_3() => Operand = Memory[Address.Whole];    //read from effective address
		private void ROL_a_4()
		{
			Memory[Address.Whole] = Operand;                          //write same value back to effective address
			Operand = DoROL(Operand);                                 //perform ROL operation
		}
		private void ROL_a_5() => Memory[Address.Whole] = Operand;    //write new value back to effective address

		//36 ROL zpg,x
		private void ROL_zx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address, inc. PC
		private void ROL_zx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read from addres, add X to it
		private void ROL_zx_3() => Operand = Memory[Address.Lower];                   //read from effective address
		private void ROL_zx_4()
		{
			Memory[Address.Whole] = Operand;                                          //write same value back to effective address
			Operand = DoROL(Operand);                                                 //perform ROL operation
		}
		private void ROL_zx_5() => Memory[Address.Lower] = Operand;                   //write new value back to effective address

		//3E ROL abs,x
		private void ROL_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ROL_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address, inc. PC
			Address.Lower += X;                                        //add X to low address byte
		}
		private void ROL_ax_3()
		{
			Operand = Memory[Address.Whole];                           //read from effective address (invalid)
			if (X > Address.Lower)                                     //if page crossed
				Address.Upper++;                                       //fix upper byte of address
		}
		private void ROL_ax_4()
		{
			Operand = Memory[Address.Whole];                           //re-read from effective address
		}
		private void ROL_ax_5()
		{
			Memory[Address.Whole] = Operand;                           //write some value back to effective address
			Operand = DoROL(Operand);                                  //perform ROL operation
		}
		private void ROL_ax_6() => Memory[Address.Lower] = Operand;    //write new value back to effective address


		//46 LSR zpg
		private void LSR_z_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address, inc. PC
		private void LSR_z_2() => Operand = Memory[Address.Lower];    //read from zpg address
		private void LSR_z_3()
		{
			Memory[Address.Lower] = Operand;                          //write same value back to zpg address
			Operand = DoLSR(Operand);                                 //perform LSR operation
		}

		private void LSR_z_4() => Memory[Address.Lower] = Operand;    //write new value back to zpg address

		//4A LSR
		private void LSR()
		{
			_ = Memory[PC.Whole]; //read next instruction byte (thorw away)
			A = DoLSR(A);         //perform LSR
		}

		//4E LSR abs
		private void LSR_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower, inc. PC
		private void LSR_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper, inc. PC
		private void LSR_a_3() => Operand = Memory[Address.Whole];    //read from effective address
		private void LSR_a_4()
		{
			Memory[Address.Whole] = Operand;                          //write same value back to effective address
			Operand = DoLSR(Operand);                                 //perform LSR operation
		}
		private void LSR_a_5() => Memory[Address.Whole] = Operand;    //write new value back to effective address

		//56 LSR zpg,x
		private void LSR_zx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address, inc. PC
		private void LSR_zx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read from addres, add X to it
		private void LSR_zx_3() => Operand = Memory[Address.Lower];                   //read from effective address
		private void LSR_zx_4()
		{
			Memory[Address.Whole] = Operand;                                          //write same value back to effective address
			Operand = DoLSR(Operand);                                                 //perform LSR operation
		}
		private void LSR_zx_5() => Memory[Address.Lower] = Operand;                   //write new value back to effective address

		//5E LSR abs,x
		private void LSR_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void LSR_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address, inc. PC
			Address.Lower += X;                                        //add X to low address byte
		}
		private void LSR_ax_3()
		{
			Operand = Memory[Address.Whole];                           //read from effective address (invalid)
			if (X > Address.Lower)                                     //if page crossed
				Address.Upper++;                                       //fix upper byte of address
		}
		private void LSR_ax_4()
		{
			Operand = Memory[Address.Whole];                           //re-read from effective address
		}
		private void LSR_ax_5()
		{
			Memory[Address.Whole] = Operand;                           //write some value back to effective address
			Operand = DoLSR(Operand);                                  //perform LSR operation
		}
		private void LSR_ax_6() => Memory[Address.Lower] = Operand;    //write new value back to effective address



		//66 ROR zpg
		private void ROR_z_1() => Address.Lower = Memory[PC.Whole++]; //fetch zpg address, inc. PC
		private void ROR_z_2() => Operand = Memory[Address.Lower];    //read from zpg address
		private void ROR_z_3()
		{
			Memory[Address.Lower] = Operand;                          //write same value back to zpg address
			Operand = DoROR(Operand);                                 //perform ROR operation
		}

		private void ROR_z_4() => Memory[Address.Lower] = Operand;    //write new value back to zpg address

		//6A ROR
		private void ROR()
		{
			_ = Memory[PC.Whole]; //read next instruction byte (thorw away)
			A = DoROR(A);         //perform ROR
		}

		//6E ROR abs
		private void ROR_a_1() => Address.Lower = Memory[PC.Whole++]; //fetch address lower, inc. PC
		private void ROR_a_2() => Address.Upper = Memory[PC.Whole++]; //fetch address upper, inc. PC
		private void ROR_a_3() => Operand = Memory[Address.Whole];    //read from effective address
		private void ROR_a_4()
		{
			Memory[Address.Whole] = Operand;                          //write same value back to effective address
			Operand = DoROR(Operand);                                 //perform ROR operation
		}
		private void ROR_a_5() => Memory[Address.Whole] = Operand;    //write new value back to effective address

		//76 ROR zpg,x
		private void ROR_zx_1() => Address.Lower = Memory[PC.Whole++];                //fetch address, inc. PC
		private void ROR_zx_2() => Address.Lower = (byte)(Memory[Address.Lower] + X); //read from addres, add X to it
		private void ROR_zx_3() => Operand = Memory[Address.Lower];                   //read from effective address
		private void ROR_zx_4()
		{
			Memory[Address.Whole] = Operand;                                          //write same value back to effective address
			Operand = DoROR(Operand);                                                 //perform ROR operation
		}
		private void ROR_zx_5() => Memory[Address.Lower] = Operand;                   //write new value back to effective address

		//7E ROR abs,x
		private void ROR_ax_1() => Address.Lower = Memory[PC.Whole++]; //fetch low byte of address, inc. PC
		private void ROR_ax_2()
		{
			Address.Upper = Memory[PC.Whole++];                        //fetch high byte of address, inc. PC
			Address.Lower += X;                                        //add X to low address byte
		}
		private void ROR_ax_3()
		{
			Operand = Memory[Address.Whole];                           //read from effective address (invalid)
			if (X > Address.Lower)                                     //if page crossed
				Address.Upper++;                                       //fix upper byte of address
		}
		private void ROR_ax_4()
		{
			Operand = Memory[Address.Whole];                           //re-read from effective address
		}
		private void ROR_ax_5()
		{
			Memory[Address.Whole] = Operand;                           //write some value back to effective address
			Operand = DoROR(Operand);                                  //perform ROR operation
		}
		private void ROR_ax_6() => Memory[Address.Lower] = Operand;    //write new value back to effective address


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
