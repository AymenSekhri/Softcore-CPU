library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity RS232 is 
	generic (
		CLKS_PER_BIT : integer := 4;
		Half_CLKS_PER_BIT : integer := 2
    );
	Port(Clk : in std_logic;
		Data_in:  in Std_logic;
		Data_Ready,Prog_Mode:  out Std_logic;
		Dataout : out std_logic_vector(31 downto 0));
	End Entity;
Architecture CPU of RS232 is
TYPE State_Type IS (Start_Bit,Recieving_Data,Stop_Bit,Idle,Idle2);
Signal UART_State : State_Type := Idle;
Signal BitClksCounter : std_logic_vector(20 downto 0);
Signal ByteCounter : std_logic_vector(15 downto 0) := (others => '0');
Signal BitCounter : std_logic_vector(3 downto 0) := x"0";
Signal Parallel_Data : std_logic_vector(7 downto 0) := (others => '0');
Signal Full_Data : std_logic_vector(31 downto 0) := (others => '0');
Signal ProgMode : std_logic := '0';
Signal DataReady : std_logic := '0';

Begin
	Process(Clk)
	Begin
		if Clk'event and Clk='0'Then
			Case UART_State is
				When Idle =>
					-- Idle to Start_Bit without debouncing 
					DataReady <= '0';
					If Data_in = '0' Then
						UART_State <= Start_Bit;
					End if;
				When Start_Bit =>
					
					If BitClksCounter = Half_CLKS_PER_BIT - 1 Then
						If Data_in = '0' Then
							Parallel_Data <= x"00";
							UART_State <= Recieving_Data;
						Else
							UART_State <= Idle;
						End If;
						BitClksCounter <= (others => '0');
					Else
							BitClksCounter <= BitClksCounter + 1;
					End if;
				When Recieving_Data =>
					If BitClksCounter = CLKS_PER_BIT - 1 Then
						If BitCounter = 7 Then
							BitCounter <= x"0";
							UART_State <= Stop_Bit;
						Else
							BitCounter <= BitCounter + 1;
						End if;
						Parallel_Data <= Data_in & Parallel_Data(7 downto 1);
						BitClksCounter <= (others => '0');
					Else
						BitClksCounter <= BitClksCounter + 1;
					End if;
				When Stop_Bit =>
					
					If BitClksCounter = CLKS_PER_BIT - 1 Then
						BitClksCounter <= (others => '0');
						UART_State <= Idle;
						If ProgMode = '0' AND Parallel_Data = x"55" Then -- 0x55 To start Prog Mode
							ProgMode <= '1';
						Elsif ProgMode = '1' AND ByteCounter < 500 Then --	Write 500 Byte
							Full_Data <= Full_Data(23 downto 0) & Parallel_Data;
							ByteCounter <= ByteCounter + 1;
							if ByteCounter(1 downto 0) = "11" Then
								DataReady <= '1';
							End if;
						Elsif ProgMode = '1' AND ByteCounter = 500 Then
							ProgMode <= '0';
							ByteCounter <= x"0000";
							
						End if;
					Else
						BitClksCounter <= BitClksCounter + 1;
					End if;
				When idle2 =>
					DataReady <= '0';
				When Others =>
					UART_State <= Idle;
			End Case;
		End If;
	End Process;
	Prog_Mode <= ProgMode;
	Data_Ready <= DataReady;
	Dataout <= Full_Data When (DataReady = '1' AND ProgMode = '1') Else	
				(others => 'Z');
	
End Architecture;