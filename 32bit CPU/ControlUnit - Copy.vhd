library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity ControlUnit is 
	Port(Clk : in std_logic;
		DataIn : in std_logic_vector(31 downto 0);
		Control :in std_logic_vector(3 downto 0);
		Counter : out std_logic_vector(4 downto 0);
		MyIR : out std_logic_vector(63 downto 0);
		ALU_OP:out std_logic_vector(3 downto 0);
		HLT,B_OE,A_OE,B_WE,A_WE,D_OE,C_OE,C_WE,D_WE,IP_OE,IP_WE,SP_OE,BP_OE,SP_WE,BP_WE,MAR_OE,MAR_WE,MBRO_OE,MBRO_WE,RAM_WE,MBRI_OE,MBRI_WE,ALU_OE,ALU_A_WE,ALU_B_WE,SevenSig_WE,DataInject: out std_logic);
End Entity;
Architecture CPU of ControlUnit is
Signal finalout : std_logic_vector(26 downto 0);
Signal MicroCounter : std_logic_vector(4 downto 0):= (others=>'0');
Signal IR : std_logic_vector(63 downto 0):= (others=>'0');
Signal CounterReset,IR_WE : std_logic :='0';
Begin
	Process(Clk,IR_WE)
	Begin
		if Clk'event and Clk='0' and IR_WE = '1' Then
			IR <= IR(31 downto 0) & DataIn;
		End If;
	End Process;
	MyIR <= IR;
	Process(Clk)
	Begin
		if Clk'event and Clk='1' Then
			if CounterReset='1' then
				MicroCounter <= (others => '0');
			else 
				MicroCounter <= MicroCounter+1;
			end if;
		End If;
	End Process;
	--CounterReset <= '0' when CounterReset = '1';
	Counter <= MicroCounter;
	Process (MicroCounter)
	Begin
		-- Fetch Cycle
		If MicroCounter=0 Then 	  -- MAR <- PC
			CounterReset <= '0';
			finalout <= "000000000010000000100000000";
		ElsIf MicroCounter=1 Then -- IP <- MBR
			--CounterReset <= '0';
			finalout <= "000000000001000000000000000";
		ElsIf MicroCounter=2 Then
			finalout <= "000000001001000000000000000";
		ElsIf MicroCounter=3 Then
			finalout <= "000000000001000000000000000";
		ElsIf MicroCounter=4 Then
			finalout <= "000000001001000000000000000";
		ElsIf MicroCounter=5 Then
			finalout <= "000000000101000000000000000";
			IR_WE <= '1';
		ElsIf MicroCounter=6 Then -- ALU_A <- PC
			finalout <= "000010000000000000100000000";
			IR_WE <= '0';
		ElsIf MicroCounter=7 Then -- MAR <- ALU_A + 4 : PC + 4
			finalout <= "000001000010000000000000000";
			ALU_OP <= x"B";
		-- Second Part Of Instruction
		ElsIf MicroCounter=8 Then -- IP <- MBR
			finalout <= "000000000001000000000000000";
		ElsIf MicroCounter=9 Then
			finalout <= "000000001001000000000000000";
		ElsIf MicroCounter=10 Then
			finalout <= "000000000001000000000000000";
		ElsIf MicroCounter=11 Then
			finalout <= "000000001001000000000000000";
		ElsIf MicroCounter=12 Then
			finalout <= "000000000101000000000000000";
			IR_WE <= '1';
		ElsIf MicroCounter=13 Then -- ALU_A <- PC
			finalout <= "000010000000000000100000000";
			IR_WE <= '0';
		ElsIf MicroCounter=14 Then -- PC <- ALU_A + 1 : PC + 1
			-- TODO : add by instruction size
			finalout <= "000001000000000001000000000";
			ALU_OP <= x"9";	
		Else
		
		-- Execute Cycle
			If IR(63 downto 60) = "1" then
				If MicroCounter=15 Then 
					finalout <= "110000000000000000000000000";
				ElsIf MicroCounter=16 Then
					finalout <= "110000000000000000000000000";
				ElsIf MicroCounter=17 Then
					finalout <= "001000000000000000000000000";
					CounterReset <= '1';
				End If;
			End If;
			
		End If;
		
	End Process;

	
	A_OE <= finalout(0);
	A_WE <= finalout(1);
	B_OE <= finalout(2);
	B_WE <= finalout(3);
	C_OE <= finalout(4);
	C_WE <= finalout(5);
	D_OE <= finalout(6);
	D_WE <= finalout(7);
	IP_OE <= finalout(8);
	IP_WE <= finalout(9);
	BP_OE <= finalout(10);
	BP_WE <= finalout(11);
	SP_OE <= finalout(12);
	SP_WE <= finalout(13);
	RAM_WE <= finalout(14);
	MAR_OE <= finalout(15);
	MAR_WE <= finalout(16);
	MBRO_OE <= finalout(17);
	MBRO_WE <= finalout(18);
	MBRI_OE <= finalout(19);
	MBRI_WE <= finalout(20);
	ALU_OE <= finalout(21);
	ALU_A_WE <= finalout(22);
	ALU_B_WE <= finalout(23);	
	HLT <= finalout(24);
	SevenSig_WE <= finalout(25);
	DataInject <= finalout(26);
	
End Architecture;
