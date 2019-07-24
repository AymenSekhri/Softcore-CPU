library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;
Entity PWM_Generator is 
	Port(CLK : in std_logic;
		DeviceControl : in std_logic_vector(7 downto 0);
		PWM_Control : in std_logic_vector(31 downto 0);
		PWM : out std_logic_vector(7 downto 0));
End Entity;
Architecture CPU of PWM_Generator is
signal Counter: std_logic_vector(7 downto 0) := x"00";
Signal Control_Register0 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register1 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register2 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register3 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register4 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register5 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register6 : std_logic_vector(7 downto 0) := x"00";
Signal Control_Register7 : std_logic_vector(7 downto 0) := x"00";
Signal Slow_Clk : Std_logic;
Signal Slow_Counter : std_logic_vector(26 downto 0) := (others => '0');
Begin 
	
	
	Process(Clk) is
	Begin
		if Counter = 255 Then
			Counter <= (others => '0');
		Elsif Clk'event and Clk= '0' Then
			If DeviceControl <= 8 AND DeviceControl /= 0 Then
				Case DeviceControl is
					When x"01" =>
						Control_Register0 <= PWM_Control(7 downto 0);
					When x"02" =>
						Control_Register1 <= PWM_Control(7 downto 0);
					When x"03" =>
						Control_Register2 <= PWM_Control(7 downto 0);
					When x"04" =>
						Control_Register3 <= PWM_Control(7 downto 0);
					When x"05" =>
						Control_Register4 <= PWM_Control(7 downto 0);
					When x"06" =>
						Control_Register5 <= PWM_Control(7 downto 0);
					When x"07" =>
						Control_Register6 <= PWM_Control(7 downto 0);
					When x"08" =>
						Control_Register7 <= PWM_Control(7 downto 0);
					When Others =>
				End Case;
				Counter <= x"00";
			Else
				If Counter < Control_Register0 Then
					PWM(0) <='1';
				Else
					PWM(0) <='0';
				End if;
				If Counter < Control_Register1 Then
					PWM(1) <='1';
				Else
					PWM(1) <='0';
				End if;
				If Counter < Control_Register2 Then
					PWM(2) <='1';
				Else
					PWM(2) <='0';
				End if;
				If Counter < Control_Register3 Then
					PWM(3) <='1';
				Else
					PWM(3) <='0';
				End if;
				If Counter < Control_Register4 Then
					PWM(4) <='1';
				Else
					PWM(4) <='0';
				End if;
				If Counter < Control_Register5 Then
					PWM(5) <='1';
				Else
					PWM(5) <='0';
				End if;
				If Counter < Control_Register6 Then
					PWM(6) <='1';
				Else
					PWM(6) <='0';
				End if;
				If Counter < Control_Register7 Then
					PWM(7) <='1';
				Else
					PWM(7) <='0';
				End if;
				
				Counter <= Counter + 1;
			End if;		
		End if;
	End Process;
	
End Architecture;
