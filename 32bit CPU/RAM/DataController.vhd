library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity DataController is 
	Port(MBRO_WE,MBRO_OE,CLK : in std_logic;
		AddressIn : in std_logic_vector(31 downto 0);
		DataIn : in std_logic_vector(31 downto 0);
		DataOut : out std_logic_vector(31 downto 0));
End Entity;
Architecture CPU of DataController is
signal InternalMemory: std_logic_vector(63 downto 0) := (others=>'0');
Begin 

	Process(CLK) is
	Begin
		if CLK'event and CLK= '0'and MBRO_WE = '1' Then
			InternalMemory <= InternalMemory(31 downto 0) & DataIn;
		End If;
	End Process;
	
	Process(InternalMemory,MBRO_OE,AddressIn) is
	Begin
		if MBRO_OE = '1' Then
			if AddressIn(1 downto 0) = 0 then
				DataOut <= InternalMemory(63 downto 32);
			elsif AddressIn(1 downto 0) = 1 then
				DataOut <= InternalMemory(55 downto 24);
			elsif AddressIn(1 downto 0) = 2 then
				DataOut <= InternalMemory(47 downto 16);
			elsif AddressIn(1 downto 0) = 3 then
				DataOut <= InternalMemory(39 downto 8);
			Else
				DataOut <= (others=>'Z');
			End if;
		else
			DataOut <= (others=>'Z');
		End If;
	End Process;
	
					
End Architecture;
