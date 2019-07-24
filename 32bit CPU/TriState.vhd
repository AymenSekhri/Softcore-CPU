library ieee;
use ieee.std_logic_1164.all;
Entity TriState is 
	Port(Enable : in std_logic;
		DataIn : in std_logic_vector(31 downto 0);
		DataOut : out std_logic_vector(31 downto 0));
End Entity;
Architecture CPU of TriState is
Begin 
	DataOut <= DataIn when Enable = '1' else
				(others=>'Z');
End Architecture;
