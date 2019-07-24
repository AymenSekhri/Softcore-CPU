library ieee;
use ieee.std_logic_1164.all;
Entity GRegister is 
	Port(OutputEnable,WriteEnable,CLK : in std_logic;
		DataIn : in std_logic_vector(31 downto 0);
		DataOut : out std_logic_vector(31 downto 0));
End Entity;
Architecture CPU of GRegister is
signal InternalMemory: std_logic_vector(31 downto 0) := x"00000000";
Begin 
	Process(CLK,OutputEnable) is
	Begin
		If OutputEnable = '1' Then
			DataOut <= InternalMemory;
		Elsif CLK'event and CLK= '0'and WriteEnable = '1' Then
			InternalMemory <= DataIn;
			DataOut <= (others=>'Z');
		Else
			DataOut <= (others=>'Z');
		End If;
	End Process;
	
End Architecture;
