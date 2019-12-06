Imports System.Text.RegularExpressions

Imports System.IO.Ports
Imports System.Threading.Thread
Imports FastColoredTextBoxNS
Imports Assembler.Instruction

Public Class Form1
   
    Dim Code As New List(Of Instruction)
    Function ReConstruct(a As String) As String
		Dim s As String = a.Replace("RET", "POP IP")
        Dim lines As String() = s.Split(New String() {vbNewLine}, StringSplitOptions.RemoveEmptyEntries)
        For index = 0 To lines.Count - 1
            Dim ma As Match = Regex.Match(lines(index), "//.+")
            If ma.Success Then
                lines(index) = lines(index).Remove(ma.Index, ma.Length)
            End If
        Next

        Return String.Join(vbNewLine, lines)
    End Function
    Sub Compile()


        Dim Lables As New List(Of Instruction.LableStrc)
		FastColoredTextBox1.Text = FixText(FastColoredTextBox1.Text)
		Dim Lines As String() = ReConstruct(FastColoredTextBox1.Text).Split(New String() {vbNewLine}, StringSplitOptions.RemoveEmptyEntries)

        Code.Clear()

        For index = 0 To Lines.Count - 1
            Dim m As Match = Regex.Match(Lines(index), "\w+:")
            If m.Success Then
                Dim newlable As Instruction.LableStrc
                newlable.LableName = m.Value.Replace(":", "")
                newlable.InstructerIndex = index
                Lables.Add(newlable)
                Lines(index) = Lines(index).Replace(m.Value, "")
            End If
           
            Dim lb As Match = Regex.Match(Lines(index), "@[_A-Za-z0-9]+")
            Dim ins As Instruction
            If lb.Success Then
                ins = New Instruction(Lines(index).Replace(lb.Value, "0H"), lb.Success)
            Else
                ins = New Instruction(Lines(index), lb.Success)
            End If

            If ins.MyError = Instruction.InstructionError.GoodInstruction Then
                If Code.Count = 0 Then
                    ins.Address = 0
                Else
                    ins.Address = Code(Code.Count - 1).Address + Code(Code.Count - 1).Size
                End If
                Code.Add(ins)
            Else
				Dim Lines2 As String() = FastColoredTextBox1.Text.Split(New String() {vbNewLine}, StringSplitOptions.RemoveEmptyEntries)
                MsgBox(ins.MyError.ToString & " at line " & index + 1)
                FastColoredTextBox1.SelectionStart = String.Join(vbNewLine, Lines2, 0, index).Length
                FastColoredTextBox1.SelectionLength = Lines2(index).Length
				FastColoredTextBox1.SelectionColor = Color.Red
				Exit Sub
            End If
        Next

        Dim lbind As Integer = 0
        For index = 0 To Code.Count - 1
			If Code(index).isLabled Then
				Dim isDone As Boolean = False
				For index2 = 0 To Lables.Count - 1
					If Lines(index).Contains("@" & Lables(index2).LableName) Then
						Dim add As Integer = Code(index).Address
						Code(index) = New Instruction(Lines(index).Replace("@" & Lables(index2).LableName, Code(Lables(index2).InstructerIndex).Address.ToString("X") & "H"), False)
						Code(index).Address = add
						isDone = True
					End If
				Next
				If isDone = False Then
                    MsgBox(" Unknown label at " & index + 1)
                    Exit Sub
				End If

			End If
        Next


        Dim finalmachinecode As String = ""
        For index = 0 To Code.Count - 1
            finalmachinecode &= Code(index).Address.ToString("X4") & " :            "
            For index2 = 0 To Code(index).Size - 1
                finalmachinecode &= Code(index).MachineCode(index2).ToString("X2") & " "
            Next
            finalmachinecode &= vbNewLine
        Next
        MsgBox(finalmachinecode)
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Compile()
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetPorts()
    End Sub
    Dim SelectedPort As String = ""
    Sub GetPorts()
        ToolStripComboBox1.Items.Clear()
        ToolStripComboBox1.Text = ""
        For index = 0 To My.Computer.Ports.SerialPortNames().Count - 1
            ToolStripComboBox1.Items.Add(My.Computer.Ports.SerialPortNames(index))
        Next
        If ToolStripComboBox1.Items.Count > 0 Then
            SelectedPort = ToolStripComboBox1.Items(0).ToString
            ToolStripComboBox1.SelectedIndex = 0
        End If
    End Sub
    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        Compile()
        If ToolStripComboBox1.Text = "" Then
            MessageBox.Show("There is no COM port", "Assembler", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ToolStripStatusLabel1.Text = "There is no COM port."
            Exit Sub
        End If
        ToolStripStatusLabel1.Text = "Uploading to port " & ToolStripComboBox1.Text & " ..."
        Dim com As SerialPort = My.Computer.Ports.OpenSerialPort(SelectedPort, 5000)
        com.Write({&H55}, 0, 1) 'Start
        Dim bytecount As Integer = 0
        For index = 0 To Code.Count - 1
            For index2 = 0 To Code(index).Size - 1
                com.Write({Code(index).MachineCode(index2)}, 0, 1)
                bytecount = bytecount + 1
            Next
        Next
        For index = 1 To 500 - bytecount
            com.Write({&H0}, 0, 1)
        Next
        com.Write({&H0}, 0, 1) ' End
        com.Close()
        ToolStripStatusLabel1.Text = "The Program Has Been Uploaded !"
    End Sub

    Function FixText(input As String) As String

		Dim Lines As String() = input.Split(New String() {vbNewLine}, StringSplitOptions.RemoveEmptyEntries)
        For index = 0 To Lines.Count - 1
			If Regex.Match(Lines(index), """([^""]*)""").Success = False Then
				Lines(index) = Lines(index).ToUpper
			End If
        Next
        Return String.Join(vbNewLine, Lines)
    End Function

    Dim MovStyle As TextStyle = New TextStyle(Brushes.Aquamarine, Nothing, FontStyle.Regular)
    Dim ALUStyle As TextStyle = New TextStyle(Brushes.LightPink, Nothing, FontStyle.Regular)
    Dim StackStyle As TextStyle = New TextStyle(Brushes.LightBlue, Nothing, FontStyle.Regular)
    Dim CALLStyle As TextStyle = New TextStyle(Brushes.Yellow, Nothing, FontStyle.Regular)
    Dim JMPStyle As TextStyle = New TextStyle(Brushes.BlueViolet, Nothing, FontStyle.Regular)
    Dim NumStyle As TextStyle = New TextStyle(Brushes.Chartreuse, Nothing, FontStyle.Regular)
    Dim Comment As TextStyle = New TextStyle(Brushes.Green, Nothing, FontStyle.Regular)
	Dim One As TextStyle = New TextStyle(Brushes.Red, Nothing, FontStyle.Regular)
	Dim Str As TextStyle = New TextStyle(Brushes.BurlyWood, Nothing, FontStyle.Regular)


    Private Sub FastColoredTextBox1_Load(sender As Object, e As TextChangedEventArgs) Handles FastColoredTextBox1.TextChanged
        e.ChangedRange.ClearStyle(New Style() {Me.MovStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.ALUStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.StackStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.CALLStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.JMPStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.NumStyle})
        e.ChangedRange.ClearStyle(New Style() {Me.Comment})
		e.ChangedRange.ClearStyle(New Style() {Me.One})
		e.ChangedRange.ClearStyle(New Style() {Me.Str})

		e.ChangedRange.SetStyle(Me.Comment, "//.+")
		e.ChangedRange.SetStyle(Me.Str, """([^""]*)""")
        e.ChangedRange.SetStyle(Me.MovStyle, "\bMOV")
        e.ChangedRange.SetStyle(Me.ALUStyle, "\bADD|\bSUB|\bMUL|\bAND|\bOR|\bCMP|\bINC|\bDEC|\bROR|\bROL")
        e.ChangedRange.SetStyle(Me.StackStyle, "\bPUSH|\bPOP")
        e.ChangedRange.SetStyle(Me.CALLStyle, "\bCALL|\bRET")
		e.ChangedRange.SetStyle(Me.JMPStyle, "\bJ[ZSOGL]|\bJN[ZSOGL]|\bJMP")
        e.ChangedRange.SetStyle(Me.NumStyle, "[0-9a-fA-F]+[hH]")
		e.ChangedRange.SetStyle(Me.One, "\bNOP|\bHLT|\bOUT")

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim finalmachinecode As String = ""
        Dim insindex As Integer = 0
        For index = 0 To Code.Count - 1
            For index2 = 0 To Code(index).Size - 1
                If (insindex Mod 4) = 0 And insindex <> 0 Then
                    finalmachinecode &= vbTab
                End If
                insindex += 1
                finalmachinecode &= Code(index).MachineCode(index2).ToString("X2")
            Next
            'finalmachinecode &= vbNewLine
        Next
        For index = 1 To (insindex Mod 4)
            finalmachinecode &= "0"
        Next

        ToolStripStatusLabel1.Text = "Data Copied ! "

    End Sub

	Private Sub ToolStripComboBox1_Click(sender As Object, e As EventArgs) Handles ToolStripComboBox1.Click
		GetPorts()
	End Sub
End Class
