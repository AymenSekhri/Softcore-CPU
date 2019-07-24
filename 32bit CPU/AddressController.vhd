library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity AddressController is 
	Port(Clk ,OutputEnable: in std_logic;
		AddressIn : in std_logic_vector(31 downto 0);
		AddressOut : out std_logic_vector(31 downto 0));
End Entity;
Architecture arch of AddressController is
Signal Address : std_logic_vector(31 downto 0);
Signal Counter : std_logic_vector(1 downto 0) := "00";
Signal CounterStop : std_logic := '1';
Begin
	
	Process(Clk)
	Begin
		if Clk'event and Clk = '0' then
			Counter <= Counter+1;
		End if;
	End Process;
	Process(Counter,OutputEnable)
	Begin
		if OutputEnable'event and OutputEnable = '1' then
			CounterStop <= '0';
			Address <= AddressIn;
		End if;
		if Counter < 4 and CounterStop = '0' then
			Address <= Address + 1;
		Else
			CounterStop <= '1';
		End if;
	End Process;

	AddressOut <= Address when OutputEnable='1' else
				 (others => 'Z');
End Architecture;
