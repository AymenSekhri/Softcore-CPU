MOV SP,400H
MOV AX,38H
MOV BX,0H
CALL @SEND       //INTIATE LCD
MOV AX,FH
MOV BX,0H
CALL @SEND       //CLEAR LCD AND SETUP CURSOR
MOV AX,1H
MOV BX,0H
CALL @SEND
MOV CX,@STRING
NEXT:MOV AX,[CX] //CHARACTERS LOOP
ROR AX,18H
AND AX,FFH
JZ @SK           //CONTINUE IF IT IS NON-ZERO CHARACTER
MOV BX,1H
CALL @SEND       //PRINT CHARACTER
INC CX
JMP @NEXT
SK:NOP
HLT
STRING: DS "Aymen Sekhri" //NULL TERMINATED STRING
// RW : 0 WRITE      1 READ
// RS : 0 COMMAND    1 DATA
SEND:ROL AX,1H
ROL AX,1H
ROL AX,1H
OR AX,BX
OUT 21H,AX
CALL @SLEEP
OR AX,4H
OUT 21H,AX
CALL @SLEEP
AND AX,FFFBH
OUT 21H,AX
CALL @SLEEP
MOV AX,0H
OUT 21H,AX
CALL @SLEEP
RET
SLEEP:MOV DX,0H
RE:ADD DX,1H
CMP DX,1524H
JL @RE
RET