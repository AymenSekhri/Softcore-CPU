library ieee;
use ieee.std_logic_1164.all;
use IEEE.numeric_std.ALL;
Entity Dum is 
	Port(DataIn: in std_logic;
		DataOut : out std_logic_vector(7 downto 0));
End Entity;
Architecture arch of Dum is
Begin
	DataOut <= x"20" When DataIn = '1' Else
				 x"00";
End Architecture;
	