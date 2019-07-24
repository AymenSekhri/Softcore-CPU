MOV SP,400H		//Setup the stack address.
MOV CX,0H		//Initiate The Counter To Zero
OUT 20H,CX		//Output The Current Value Of The Counter To The Seven Segment
CALL @SLEEP		//Call Sleep Subroutine
INC CX			//Increment The Counter By 1
JMP 0Ch
HLT
SLEEP:MOV AX,0H // Sleep Approximately 1 Second.
RE:ADD AX,1H
CMP AX,F4240H
JL @RE 			// Loop To Waste Time.
RET