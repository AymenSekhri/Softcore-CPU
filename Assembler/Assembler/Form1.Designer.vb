<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
		Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
		Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
		Me.SleepToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.LCDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.ServoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.FastColoredTextBox1 = New FastColoredTextBoxNS.FastColoredTextBox()
		Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.ToolStrip1.SuspendLayout()
		CType(Me.FastColoredTextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.StatusStrip1.SuspendLayout()
		Me.SuspendLayout()
		'
		'ToolStrip1
		'
		Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripComboBox1, Me.ToolStripButton3, Me.ToolStripDropDownButton1})
		Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
		Me.ToolStrip1.Name = "ToolStrip1"
		Me.ToolStrip1.Size = New System.Drawing.Size(855, 28)
		Me.ToolStrip1.TabIndex = 1
		Me.ToolStrip1.Text = "ToolStrip1"
		'
		'ToolStripButton1
		'
		Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
		Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton1.Name = "ToolStripButton1"
		Me.ToolStripButton1.Size = New System.Drawing.Size(69, 25)
		Me.ToolStripButton1.Text = "Compile"
		Me.ToolStripButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'ToolStripButton2
		'
		Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
		Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton2.Name = "ToolStripButton2"
		Me.ToolStripButton2.Size = New System.Drawing.Size(101, 25)
		Me.ToolStripButton2.Text = "Upload Code"
		'
		'ToolStripComboBox1
		'
		Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
		Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 28)
		'
		'ToolStripButton3
		'
		Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
		Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton3.Name = "ToolStripButton3"
		Me.ToolStripButton3.Size = New System.Drawing.Size(77, 25)
		Me.ToolStripButton3.Text = "Copy Hex"
		'
		'ToolStripDropDownButton1
		'
		Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SleepToolStripMenuItem, Me.LCDToolStripMenuItem, Me.ServoToolStripMenuItem})
		Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
		Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
		Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(111, 25)
		Me.ToolStripDropDownButton1.Text = "Add Function"
		'
		'SleepToolStripMenuItem
		'
		Me.SleepToolStripMenuItem.Name = "SleepToolStripMenuItem"
		Me.SleepToolStripMenuItem.Size = New System.Drawing.Size(121, 26)
		Me.SleepToolStripMenuItem.Text = "Sleep"
		'
		'LCDToolStripMenuItem
		'
		Me.LCDToolStripMenuItem.Name = "LCDToolStripMenuItem"
		Me.LCDToolStripMenuItem.Size = New System.Drawing.Size(121, 26)
		Me.LCDToolStripMenuItem.Text = "LCD"
		'
		'ServoToolStripMenuItem
		'
		Me.ServoToolStripMenuItem.Name = "ServoToolStripMenuItem"
		Me.ServoToolStripMenuItem.Size = New System.Drawing.Size(121, 26)
		Me.ServoToolStripMenuItem.Text = "Servo"
		'
		'FastColoredTextBox1
		'
		Me.FastColoredTextBox1.AutoCompleteBrackets = True
		Me.FastColoredTextBox1.AutoCompleteBracketsList = New Char() {Global.Microsoft.VisualBasic.ChrW(40), Global.Microsoft.VisualBasic.ChrW(41), Global.Microsoft.VisualBasic.ChrW(123), Global.Microsoft.VisualBasic.ChrW(125), Global.Microsoft.VisualBasic.ChrW(91), Global.Microsoft.VisualBasic.ChrW(93), Global.Microsoft.VisualBasic.ChrW(34), Global.Microsoft.VisualBasic.ChrW(34), Global.Microsoft.VisualBasic.ChrW(39), Global.Microsoft.VisualBasic.ChrW(39)}
		Me.FastColoredTextBox1.AutoScrollMinSize = New System.Drawing.Size(691, 936)
		Me.FastColoredTextBox1.BackBrush = Nothing
		Me.FastColoredTextBox1.BackColor = System.Drawing.Color.Black
		Me.FastColoredTextBox1.CharHeight = 18
		Me.FastColoredTextBox1.CharWidth = 10
		Me.FastColoredTextBox1.CurrentLineColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.FastColoredTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.FastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer))
		Me.FastColoredTextBox1.Font = New System.Drawing.Font("Courier New", 9.75!)
		Me.FastColoredTextBox1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
		Me.FastColoredTextBox1.IsReplaceMode = False
		Me.FastColoredTextBox1.Location = New System.Drawing.Point(1, 30)
		Me.FastColoredTextBox1.Name = "FastColoredTextBox1"
		Me.FastColoredTextBox1.Paddings = New System.Windows.Forms.Padding(0)
		Me.FastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
		Me.FastColoredTextBox1.ServiceColors = CType(resources.GetObject("FastColoredTextBox1.ServiceColors"), FastColoredTextBoxNS.ServiceColors)
		Me.FastColoredTextBox1.ShowFoldingLines = True
		Me.FastColoredTextBox1.Size = New System.Drawing.Size(846, 483)
		Me.FastColoredTextBox1.TabIndex = 0
		Me.FastColoredTextBox1.Text = resources.GetString("FastColoredTextBox1.Text")
		Me.FastColoredTextBox1.Zoom = 100
		'
		'StatusStrip1
		'
		Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
		Me.StatusStrip1.Location = New System.Drawing.Point(0, 516)
		Me.StatusStrip1.Name = "StatusStrip1"
		Me.StatusStrip1.Size = New System.Drawing.Size(855, 25)
		Me.StatusStrip1.TabIndex = 2
		Me.StatusStrip1.Text = "StatusStrip1"
		'
		'ToolStripStatusLabel1
		'
		Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
		Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(59, 20)
		Me.ToolStripStatusLabel1.Text = "Ready..."
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(855, 541)
		Me.Controls.Add(Me.StatusStrip1)
		Me.Controls.Add(Me.ToolStrip1)
		Me.Controls.Add(Me.FastColoredTextBox1)
		Me.Name = "Form1"
		Me.Text = "SoftCore CPU Assembler"
		Me.ToolStrip1.ResumeLayout(False)
		Me.ToolStrip1.PerformLayout()
		CType(Me.FastColoredTextBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.StatusStrip1.ResumeLayout(False)
		Me.StatusStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
    Friend WithEvents FastColoredTextBox1 As FastColoredTextBoxNS.FastColoredTextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripComboBox1 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents SleepToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LCDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
