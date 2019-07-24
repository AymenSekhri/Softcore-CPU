library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity ClkControler is 
	Port(CLK,HLT: in std_logic;
		NewClk : out std_logic);
End Entity;
Architecture CPU of ClkControler is
Signal myclk :std_logic := '1';
Signal SlowClk :std_logic;
Signal Counter : std_logic_vector(27 downto 0) := (others=>'0');
Begin
	Process(Clk)
	Begin
		If Counter = 250000000 Then
			Counter <= (others=>'0');
		elsif CLK'event and CLK='1' then
				Counter <= Counter + 1;
		End if;
	End Process;
	NewClk <= Counter(22);

End Architecture;

