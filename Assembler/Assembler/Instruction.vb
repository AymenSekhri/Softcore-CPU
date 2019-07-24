Imports System.Text.RegularExpressions

Public Class Instruction
    Public Address As Integer
    Public Size As Integer
    Public MachineCode(6) As Byte
    Public MyError As InstructionError = InstructionError.GoodInstruction
    Public isLabled As Boolean = False
    Enum InstructionType
        R32Imm32 = 0
        R32R32 = 1
        R32M32 = 2
        ReadR32Indirect = 3
        WriteR32Indirect = 4
        WriteR32imm = 5
        Immediate = 6
        R32 = 7
        r32imm8 = 8
        None = 9
		InvalidType = 90
    End Enum
    Enum InstructionError
        UnknowInstructionType
        UnknowInstruction
        InvalidParam1
        InvalidParam2
        GoodInstruction
    End Enum
    Structure LableStrc
        Public LableName As String
        Public InstructerIndex As Integer
    End Structure
    Sub GetArgs(ByVal input As String, ByRef OpC As String, ByRef Parm1 As String, ByRef Parm2 As String)
        input = input.Replace(",", " ")
        Dim Word As Match = Regex.Match(input, "[^ ^\t]+")
        If Word.Success Then
            input = input.Substring(Word.Index + Word.Length)
            OpC = Word.Value
        End If

        Word = Regex.Match(input, "[^ ^\t]+")
        If Word.Success Then
            input = input.Substring(Word.Index + Word.Length)
            Parm1 = Word.Value
        End If

        Word = Regex.Match(input, "[^ ^\t]+")
        If Word.Success Then
            input = input.Substring(Word.Index + Word.Length)
            Parm2 = Word.Value
        End If

    End Sub
    Sub New(mnemonic As String, Labled As Boolean)
        Dim OpC As String = ""
        Dim Parm1 As String = ""
        Dim Parm2 As String = ""
        Dim IsDef As Match = Regex.Match(mnemonic, "[^ ^\t]+")
        If IsDef.Value = "DB" Or IsDef.Value = "DD" Or IsDef.Value = "DS" Then
            Array.Resize(MachineCode, 256)
            If IsDef.Value = "DB" Then
				For Each m As Match In Regex.Matches(mnemonic.Substring(IsDef.Index + 2), "[^ ^\t]+[hH]")

					If Regex.Match(m.Value.Remove(m.Value.Length - 1), "[A-Fa-f0-9]").Success Then
						If System.Convert.ToInt32(m.Value.Remove(m.Value.Length - 1), 16) < 256 Then
							MachineCode(Size) = System.Convert.ToInt32(m.Value.Remove(m.Value.Length - 1), 16)
							Size = Size + 1
						Else
							MyError = InstructionError.InvalidParam1
							Exit Sub
						End If
					Else
						MyError = InstructionError.InvalidParam1
						Exit Sub
					End If
				Next
            ElseIf IsDef.Value = "DD" Then
				For Each m As Match In Regex.Matches(mnemonic.Substring(IsDef.Index + 2), "[^ ^\t]+[hH]")
					If Regex.Match(m.Value.Remove(m.Value.Length - 1), "[A-Fa-f0-9]").Success Then
						Dim ImmData() As Byte = BitConverter.GetBytes(System.Convert.ToUInt32(m.Value.Remove(m.Value.Length - 1), 16))
						MachineCode(Size + 0) = ImmData(3)
						MachineCode(Size + 1) = ImmData(2)
						MachineCode(Size + 2) = ImmData(1)
						MachineCode(Size + 3) = ImmData(0)
						Size = Size + 4
					Else
						MyError = InstructionError.InvalidParam1
						Exit Sub
					End If
				Next
            ElseIf IsDef.Value = "DS" Then
                Dim str As String = ""

                Dim strmatch As Match = Regex.Match(mnemonic.Substring(IsDef.Index + 2), """.*""")
                If strmatch.Success Then
                    str = strmatch.Value.Replace("""", "")
                Else
                    MyError = InstructionError.InvalidParam1
                    Exit Sub
                End If
                For index = 0 To str.Length - 1
                    MachineCode(Size) = Asc(str(index))
                    Size = Size + 1
				Next
				Size += 1
				MachineCode(Size) = 0
            End If
        Else
            isLabled = Labled
            GetArgs(mnemonic, OpC, Parm1, Parm2)
            Dim instType As InstructionType = GetInstructionType(Parm1, Parm2)
            If instType = InstructionType.InvalidType Then
				MyError = InstructionError.UnknowInstructionType
                Exit Sub
            End If

            MachineCode(0) = FetchOpCode(OpC, instType)
            If MachineCode(0) = &HFF Then
                MyError = InstructionError.UnknowInstruction
                Exit Sub
            End If
            If instType = InstructionType.R32Imm32 Or instType = InstructionType.R32M32 Then
                Dim ImmData() As Byte = BitConverter.GetBytes(System.Convert.ToUInt32(Parm2.Replace("[", "").Replace("]", "").Replace("H", ""), 16))
                MachineCode(1) = GetRegisterCode(Parm1) << 4
                MachineCode(2) = ImmData(3)
                MachineCode(3) = ImmData(2)
                MachineCode(4) = ImmData(1)
                MachineCode(5) = ImmData(0)
            ElseIf instType = InstructionType.R32R32 Or instType = InstructionType.ReadR32Indirect Or instType = InstructionType.WriteR32Indirect Then
                MachineCode(1) = GetRegisterCode(Parm1) << 4 Or GetRegisterCode(Parm2)
            ElseIf instType = InstructionType.WriteR32imm Then
                Dim ImmData() As Byte = BitConverter.GetBytes(System.Convert.ToUInt32(Parm2.Replace("H", ""), 16))
                MachineCode(1) = GetRegisterCode(Parm1)
                MachineCode(2) = ImmData(3)
                MachineCode(3) = ImmData(2)
                MachineCode(4) = ImmData(1)
                MachineCode(5) = ImmData(0)
            ElseIf instType = InstructionType.R32 Then
                MachineCode(1) = GetRegisterCode(Parm1)
                If OpC = "DEC" Or OpC = "INC" Or OpC = "PUSH" Or OpC = "POP" Then
                    MachineCode(1) = MachineCode(1) << 4
                End If
            ElseIf instType = InstructionType.Immediate Then
                Dim ImmData() As Byte = BitConverter.GetBytes(System.Convert.ToUInt32(Parm1.Replace("H", ""), 16))
                MachineCode(1) = ImmData(3)
                MachineCode(2) = ImmData(2)
                MachineCode(3) = ImmData(1)
                MachineCode(4) = ImmData(0)
            ElseIf instType = InstructionType.r32imm8 Then
                Dim ImmData() As Byte = BitConverter.GetBytes(System.Convert.ToUInt32(Parm1.Replace("H", ""), 16))
                If ImmData(0) > 255 Then
                    MyError = InstructionError.InvalidParam1
                    Exit Sub
                End If
                MachineCode(1) = GetRegisterCode(Parm2)
                MachineCode(2) = ImmData(0)
            ElseIf instType <> InstructionType.None Then
                MyError = InstructionError.UnknowInstructionType
                Exit Sub
            End If
            Size = GetInstructionSize(MachineCode(0), instType)
            Dim stringdata As String = ""
            For index = 0 To Size - 1
                stringdata &= MachineCode(index).ToString("X2") & " "
            Next
        End If



        'MsgBox(stringdata)
    End Sub
    Function GetInstructionSize(OpCode As Byte, Type As InstructionType) As Byte
        If Type = InstructionType.R32 Then
            Return 2
        ElseIf Type = InstructionType.R32R32 Then
            Return 2
        ElseIf Type = InstructionType.Immediate Then
            Return 5
        ElseIf Type = InstructionType.R32Imm32 Then
            Return 6
        ElseIf Type = InstructionType.R32M32 Then
            Return 6
        ElseIf Type = InstructionType.ReadR32Indirect Then
            Return 2
        ElseIf Type = InstructionType.WriteR32imm Then
            Return 6
        ElseIf Type = InstructionType.WriteR32Indirect Then
            Return 2
        ElseIf Type = InstructionType.r32imm8 Then
            Return 3
        ElseIf Type = InstructionType.None Then
            Return 1
        End If
        Return 0
    End Function
    Function GetRegisterCode(register As String) As Byte
        register = register.Replace("[", "").Replace("]", "")
        If register = "AX" Then
            Return 0
        ElseIf register = "BX" Then
            Return 1
        ElseIf register = "CX" Then
            Return 2
        ElseIf register = "DX" Then
            Return 3
        ElseIf register = "IP" Then
            Return 4
        ElseIf register = "SP" Then
            Return 5
        ElseIf register = "BP" Then
            Return 6
        End If
    End Function
    Function FetchOpCode(OpCode As String, Type As InstructionType) As Byte
        If OpCode = "MOV" Then
            If Type = InstructionType.R32Imm32 Then
                Return &H1
            ElseIf Type = InstructionType.R32R32 Then
                Return &H2
            ElseIf Type = InstructionType.R32M32 Then
                Return &H3
            ElseIf Type = InstructionType.ReadR32Indirect Then
                Return &H4
            ElseIf Type = InstructionType.WriteR32Indirect Then
                Return &H5
            ElseIf Type = InstructionType.WriteR32imm Then
                Return &H14
            End If
        ElseIf OpCode = "ADD" Then
            If Type = InstructionType.R32R32 Then
                Return &H6
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &HC
            End If
        ElseIf OpCode = "SUB" Then
            If Type = InstructionType.R32R32 Then
                Return &H7
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &HD
            End If
        ElseIf OpCode = "MUL" Then
            If Type = InstructionType.R32R32 Then
                Return &H8
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &HE
            End If
        ElseIf OpCode = "AND" Then
            If Type = InstructionType.R32R32 Then
                Return &H9
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &HF
            End If
        ElseIf OpCode = "OR" Then
            If Type = InstructionType.R32R32 Then
                Return &HA
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &H10
            End If
        ElseIf OpCode = "ROR" Then
            If Type = InstructionType.R32Imm32 Then
                Return &H12
            End If
        ElseIf OpCode = "ROL" Then
            If Type = InstructionType.R32Imm32 Then
                Return &H13
            End If
        ElseIf OpCode = "CMP" Then
            If Type = InstructionType.R32R32 Then
                Return &HB
            ElseIf Type = InstructionType.R32Imm32 Then
                Return &H11
            End If
        ElseIf OpCode = "PUSH" Then
            If Type = InstructionType.R32 Then
                Return &H20
            ElseIf Type = InstructionType.Immediate Then
                Return &H21
            End If
        ElseIf OpCode = "POP" Then
            If Type = InstructionType.R32 Then
                Return &H22
            End If
        ElseIf OpCode = "JMP" Then
            If Type = InstructionType.R32 Then
                Return &H31
            ElseIf Type = InstructionType.Immediate Then
                Return &H30
            End If

        ElseIf OpCode = "JZ" Then
            If Type = InstructionType.Immediate Then
                Return &H40
            ElseIf Type = InstructionType.R32 Then
                Return &H48
            End If
        ElseIf OpCode = "JS" Then
            If Type = InstructionType.Immediate Then
                Return &H41
            ElseIf Type = InstructionType.R32 Then
                Return &H49
            End If
        ElseIf OpCode = "JO" Then
            If Type = InstructionType.Immediate Then
                Return &H42
            ElseIf Type = InstructionType.R32 Then
                Return &H4A
            End If
		ElseIf OpCode = "JL" Then
			If Type = InstructionType.Immediate Then
				Return &H43
			ElseIf Type = InstructionType.R32 Then
				Return &H4B
			End If
		ElseIf OpCode = "JG" Then
			If Type = InstructionType.Immediate Then
				Return &H44
			ElseIf Type = InstructionType.R32 Then
				Return &H4C
			End If
        ElseIf OpCode = "JNZ" Then
            If Type = InstructionType.Immediate Then
                Return &H45
            ElseIf Type = InstructionType.R32 Then
                Return &H4D
            End If
        ElseIf OpCode = "JNS" Then
            If Type = InstructionType.Immediate Then
                Return &H46
            ElseIf Type = InstructionType.R32 Then
                Return &H4E
            End If
        ElseIf OpCode = "JNO" Then
            If Type = InstructionType.Immediate Then
                Return &H47
            ElseIf Type = InstructionType.R32 Then
                Return &H4F
            End If
        ElseIf OpCode = "CALL" Then
            If Type = InstructionType.R32 Then
                Return &H24
            ElseIf Type = InstructionType.Immediate Then
                Return &H23
            End If
        ElseIf OpCode = "OUT" Then
            If Type = InstructionType.r32imm8 Then
                Return &H50
            End If
        ElseIf OpCode = "INC" Then
            If Type = InstructionType.R32 Then
                Return &H25
            End If
        ElseIf OpCode = "DEC" Then
            If Type = InstructionType.R32 Then
                Return &H26
            End If
        ElseIf OpCode = "HLT" Then
            If Type = InstructionType.None Then
				Return &HF0
            End If
        ElseIf OpCode = "NOP" Then
            If Type = InstructionType.None Then
                Return &H90
            End If
        End If
        Return &HFF
    End Function
    Function isHex(s As String) As Boolean
        Dim match As String = "ABCDEF0123456789"
        For index = 0 To s.Length - 1
            If match.Contains(s(index)) = False Then
                Return False
            End If
        Next
        Return True
    End Function
    Function isReg(s As String) As Boolean
        If s = "AX" Or s = "BX" Or s = "CX" Or s = "DX" Or s = "SP" Or s = "IP" Or s = "SP" Then
            Return True
        Else
            Return False
        End If
    End Function
    Function isRegIndirect(s As String) As Boolean
        If s = "[AX]" Or s = "[BX]" Or s = "[CX]" Or s = "[DX]" Or s = "[SP]" Or s = "[IP]" Or s = "[SP]" Then
            Return True
        Else
            Return False
        End If
    End Function
    Function isMemAddress(s As String) As Boolean
        If s.Length >= 4 Then
            If s.ToCharArray()(0) = "[" And s.ToCharArray()(s.Length - 1) = "]" And s.ToCharArray()(s.Length - 2) = "H" And isHex(s.Replace("[", "").Replace("]", "").Replace("H", "")) Then
                Return True
            End If
        End If
        Return False
    End Function
    Function isImmediate(s As String) As Boolean
        If s.Length >= 2 Then
            If s.ToCharArray()(s.Length - 1) = "H" And isHex(s.Replace("H", "")) Then
                Return True
            End If
        End If
        Return False
    End Function
    Function GetInstructionType(Param1 As String, Param2 As String) As InstructionType
        If isReg(Param1) And isRegIndirect(Param2) Then ' AX,[AX]
            Return InstructionType.ReadR32Indirect
        ElseIf isRegIndirect(Param1) And isReg(Param2) Then ' [AX],AX
            Return InstructionType.WriteR32Indirect
        ElseIf isRegIndirect(Param1) And isImmediate(Param2) Then ' [AX],1000H
            Return InstructionType.WriteR32imm
        ElseIf isReg(Param1) And isReg(Param2) Then 'AX,AX
            Return InstructionType.R32R32
        ElseIf isReg(Param1) And isMemAddress(Param2) Then 'AX,[1000H]
            Return InstructionType.R32M32
        ElseIf isReg(Param1) And isImmediate(Param2) Then 'AX,1000H
            Return InstructionType.R32Imm32
        ElseIf isImmediate(Param1) And isReg(Param2) Then 'AX,1000H
            Return InstructionType.r32imm8
        ElseIf isReg(Param1) And Param2 = "" Then 'AX
            Return InstructionType.R32
        ElseIf isImmediate(Param1) And Param2 = "" Then '1000H
            Return InstructionType.Immediate
        ElseIf Param1 = "" And Param2 = "" Then '1000H
            Return InstructionType.None
        End If
        Return InstructionType.InvalidType
    End Function
End Class
