library ieee;
use ieee.std_logic_1164.all;
Entity LCD is 
	Port(CLK: in std_logic;
		DeviceControl : in std_logic_vector(7 downto 0);
		DataIn : in std_logic_vector(31 downto 0);
		DataOut : out std_logic_vector(19 downto 0));
End Entity;
Architecture CPU of LCD is
Signal InternalMemory: std_logic_vector(19 downto 0) := (others=>'0');
Begin
	Process(CLK) is
	Begin
		If CLK'event and CLK= '0' Then
			If DeviceControl = x"21" Then
				InternalMemory <= DataIn(19 downto 0);
			End if;
		End if;
	End Process;
	DataOut <= InternalMemory;
End Architecture;
