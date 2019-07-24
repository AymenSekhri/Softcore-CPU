library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity OutputRegister is 
	Port(Enable,CLK,HighSpeed_CLK: in std_logic;
		DataIn : in std_logic_vector(31 downto 0);
		SevenSigOut : out std_logic_vector(11 downto 0);
		DataOut : out std_logic_vector(31 downto 0)); -- 4:Select + 7:7Sig
End Entity;
Architecture CPU of OutputRegister is

Signal InternalMemory: std_logic_vector(31 downto 0) := x"EEEEEEEE";

Signal Counter: std_logic_vector (1 downto 0) := "00";
Signal Clk_div: std_logic_vector (26 downto 0) := (others=>'0');
Signal Clk_Slow: std_logic;
Signal CurrentDigit : std_logic_vector (3 downto 0);
Begin
	Process(Enable,CLK) is
	Begin
		If CLK'event and CLK= '0' and Enable = '1' then
			InternalMemory <= DataIn;
		End if;
	End Process;
	-- Slow Clock
	Process (HighSpeed_CLK)
	Begin
		if HighSpeed_CLK'event and HighSpeed_CLK='0' then
			Clk_div <= Clk_div + 1;
		End If;	
	End Process;
	Clk_Slow <= Clk_div(10);
	Process (Clk_Slow)
	Begin
		if Clk_Slow'event and Clk_Slow='0' then
			Counter <= Counter + 1;
		End If;	
	End Process;
	-- Select 7Sig Digit
	Process (Counter)
	Begin
		If Counter="00" then
			SevenSigOut(11 downto 8) <= "0111";
			CurrentDigit<= InternalMemory(15 downto 12);
		Elsif Counter="01" then
			SevenSigOut(11 downto 8) <= "1011";
			CurrentDigit<= InternalMemory(11 downto 8);
		Elsif Counter="10" then
			SevenSigOut(11 downto 8) <= "1101";
			CurrentDigit<= InternalMemory(7 downto 4);
		Elsif Counter="11" then
			SevenSigOut(11 downto 8) <= "1110";
			CurrentDigit<= InternalMemory(3 downto 0);
		End If;	
	End Process;
	-- Display Current Digit
	Process (CurrentDigit)
	Begin
		Case CurrentDigit is
			when "0000" => SevenSigOut(6 downto 0) <= "0000001"; -- "0"     
			when "0001" => SevenSigOut(6 downto 0) <= "1001111"; -- "1" 
			when "0010" => SevenSigOut(6 downto 0) <= "0010010"; -- "2" 
			when "0011" => SevenSigOut(6 downto 0) <= "0000110"; -- "3" 
			when "0100" => SevenSigOut(6 downto 0) <= "1001100"; -- "4" 
			when "0101" => SevenSigOut(6 downto 0) <= "0100100"; -- "5" 
			when "0110" => SevenSigOut(6 downto 0) <= "0100000"; -- "6" 
			when "0111" => SevenSigOut(6 downto 0) <= "0001111"; -- "7" 
			when "1000" => SevenSigOut(6 downto 0) <= "0000000"; -- "8"     
			when "1001" => SevenSigOut(6 downto 0) <= "0000100"; -- "9" 
			when "1010" => SevenSigOut(6 downto 0) <= "0000010"; -- A
			when "1011" => SevenSigOut(6 downto 0) <= "1100000"; -- B
			when "1100" => SevenSigOut(6 downto 0) <= "0110001"; -- C
			when "1101" => SevenSigOut(6 downto 0) <= "1000010"; -- D
			when "1110" => SevenSigOut(6 downto 0) <= "0110000"; -- E
			when "1111" => SevenSigOut(6 downto 0) <= "0111000"; -- F
		End Case;
	End Process;
	SevenSigOut(7) <= '1';
	DataOut <= InternalMemory;
End Architecture;
