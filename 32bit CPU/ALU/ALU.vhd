library ieee;
use ieee.std_logic_1164.all;
use IEEE.numeric_std.ALL;
Entity ALU is 
	Port(Clk ,OutputEnable,ALU_A_WE,ALU_B_WE: in std_logic;
		DataIn : in std_logic_vector(31 downto 0);
		Oparation :in std_logic_vector(3 downto 0);
		DataOut : out std_logic_vector(31 downto 0);
		FlagsOut : out std_logic_vector(31 downto 0));
End Entity;
Architecture arch of ALU is
Signal Data1,Data2 : signed(31 downto 0);
Signal Flags : std_logic_vector(4 downto 0) := (Others=>'0');
Signal ALUOut: signed(32 downto 0);
Begin
	
	Process(Clk)
	Begin
		if Clk'event and Clk = '0' then
			if ALU_A_WE = '1' then 
				Data1<=signed(DataIn);
			end if;
			if ALU_B_WE = '1' then 
				Data2<=signed(DataIn);
			end if;
		End if;
	End Process;
	Process(OutputEnable,Oparation)
	Begin
		-- CMP ADD SUB AND OR NOT ROR ROL INC DEC INC4 DEC4
		--	ZF 	 SF    OF   	 GF     LF
		--	4	 3	   02   	 01     00
		-- Zero Sign Overflow Greater Lower
		if OutputEnable = '1' Then
			
			if Oparation = x"0" then --CMP
				
			Elsif Oparation = x"1" then--ADD
				ALUOut <= (Data1(31) & Data1) + (Data2(31) & Data2);
			Elsif Oparation = x"2" then --SUB
				ALUOut <= (Data1(31) & Data1) - (Data2(31) & Data2);
			Elsif Oparation = x"3" then --AND
				ALUOut(31 downto 0) <= Data1 and Data2;
			Elsif Oparation = x"4" then --OR
				ALUOut(31 downto 0) <= Data1 or Data2;
			Elsif Oparation = x"5" then --NOT
				ALUOut(31 downto 0) <= not Data1;
			Elsif Oparation = x"6" then --XOR
				ALUOut(31 downto 0) <= Data1 xor Data2;
			Elsif Oparation = x"7" then --ROR
				if Data2 = 1 Then
					ALUOut(31 downto 0) <=  Data1(0) & Data1(31 downto 1);
				Elsif Data2 = 8 Then
					ALUOut(31 downto 0) <=  Data1(7 downto 0) & Data1(31 downto 8);
				Elsif Data2 = 16 Then
					ALUOut(31 downto 0) <=  Data1(15 downto 0) & Data1(31 downto 16);
				Elsif Data2 = 24 Then
					ALUOut(31 downto 0) <=  Data1(23 downto 0) & Data1(31 downto 24);
				Else
					ALUOut(31 downto 0) <=  Data1(0) & Data1(31 downto 1);
				End if;		
			Elsif Oparation = x"8" then --ROL
				
				if Data2 = 1 Then
					ALUOut(31 downto 0) <=  Data1(30 downto 0) & Data1(31);
				Elsif Data2 = 8 Then
					ALUOut(31 downto 0) <=  Data1(23 downto 0) & Data1(31 downto 24);
				Elsif Data2 = 16 Then
					ALUOut(31 downto 0) <=  Data1(15 downto 0) & Data1(31 downto 16);
				Elsif Data2 = 24 Then
					ALUOut(31 downto 0) <=  Data1(7 downto 0) & Data1(31 downto 8);
				Else
					ALUOut(31 downto 0) <=  Data1(30 downto 0) & Data1(31);
				End if;
			Elsif Oparation = x"9" then--INC
				ALUOut <= (Data1(31) & Data1) + to_signed(1,33);
			Elsif Oparation = x"A" then--DEC
				ALUOut <= (Data1(31) & Data1) - to_signed(1,33);
			Elsif Oparation = x"B" then--INC4
				ALUOut(31 downto 0) <= Data1 + 4;
			Elsif Oparation = x"C" then--DEC4
				ALUOut(31 downto 0) <= Data1 - 4;
			Elsif Oparation = x"D" then --MUL
				ALUOut <= resize ((Data1(31) & Data1) * (Data2(31) & Data2),33);
			Elsif Oparation = x"E" then--ADD without flags
				ALUOut <= (Data1(31) & Data1) + (Data2(31) & Data2);
			End if;
				
			If Oparation = "0000" Then
				DataOut <= (others=>'Z');
			Else
				DataOut <= std_logic_vector(ALUOut(31 downto 0));
			End if;
			
		Else
			DataOut <= (others=>'Z');
		End If;
		
	End Process;	
	Process(Clk)
	Begin
		--	ZF 	 SF    OF   	 GF     LF
		--	04	 	 03  	 02   	 01     00
		-- Zero Sign Overflow Greater Lower
		
		if Clk'event AND Clk = '0' AND OutputEnable = '1' Then
			--Arithmatic
			IF Oparation = x"1" OR Oparation = x"2" OR Oparation = x"D" OR Oparation = x"9" OR Oparation = x"A" Then
				Flags <= (others=>'0');
				If ALUOut = 0 then
					Flags(4) <= '1';--ZF
				Elsif ALUOut < 0 Then
					Flags(3) <= '1';--SF
				End if;
				If ALUOut(32) /= ALUOut(31) Then
					Flags(2) <= '1';--OF
				Else
					Flags(2) <= '0';
				End if;
			--Logic
			Elsif Oparation = x"3" OR Oparation = x"4" OR Oparation = x"5" OR Oparation = x"6" OR Oparation = x"7" OR Oparation = x"8" Then
				Flags <= (others=>'0');
				If ALUOut = 0 then
					Flags(4) <= '1';--ZF
				End if;
			--Comparision
			Elsif Oparation = x"0" Then
				Flags <= (others=>'0');
				If Data1(31 downto 0) < Data2(31 downto 0) Then
					Flags(1) <= '1';
				Elsif Data1(31 downto 0) > Data2(31 downto 0) Then
					Flags(0) <= '1';
				Elsif Data1(31 downto 0) = Data2(31 downto 0) Then
					Flags(4) <= '1';
				End if;
			End if;
		End if;
	End Process;
	FlagsOut(31 downto 27) <= Flags(4 downto 0);
End Architecture;
