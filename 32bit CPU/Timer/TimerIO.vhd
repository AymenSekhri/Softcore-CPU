library ieee;
use ieee.std_logic_1164.all;
use IEEE.numeric_std.ALL;
Entity TimerIO is 
	Port(CLK: in std_logic;
		DeviceControl : in std_logic_vector(7 downto 0);
		DataIn : in std_logic_vector(31 downto 0);
		DataOut : out std_logic_vector(3 downto 0));
End Entity;
Architecture CPU of TimerIO is
Signal TimerCounter1: Unsigned(19 downto 0) := (others=>'0');
Signal TimerCounter2: Unsigned(19 downto 0) := (others=>'0');
Signal Timer1: std_logic_vector(31 downto 0) := (others=>'0');
Signal Timer2: std_logic_vector(31 downto 0) := (others=>'0');
Signal Clk_div: unsigned(3 downto 0) := (others=>'0');
Signal Clk_Slow: std_logic;
Begin

	-- 3.125 MHz Clk
	Process(Clk) is
	Begin
		If Clk'event and Clk= '0' Then
			If DeviceControl = x"25" Then
				Timer1 <= DataIn;
			Elsif TimerCounter1 = unsigned(Timer1(31 downto 16) & "0000") Then
				TimerCounter1 <= (others=>'0');
			Else
				TimerCounter1 <= TimerCounter1 + 1;
			End if;
			If TimerCounter1 < unsigned(Timer1(15 downto 0) & "0000" ) - 1 Then
				DataOut(0) <= '1';
			Else
				DataOut(0) <= '0';
			End if;
		End if;
	End Process;
End Architecture;
