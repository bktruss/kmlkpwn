<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.cbIBSS = New System.Windows.Forms.CheckBox()
        Me.cbOPEN = New System.Windows.Forms.CheckBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.cbWPA2 = New System.Windows.Forms.CheckBox()
        Me.cbWPA = New System.Windows.Forms.CheckBox()
        Me.gbFilter = New System.Windows.Forms.GroupBox()
        Me.cbWEP = New System.Windows.Forms.CheckBox()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.gbAP = New System.Windows.Forms.GroupBox()
        Me.lblAPContains = New System.Windows.Forms.Label()
        Me.btnApRemove = New System.Windows.Forms.Button()
        Me.txtAPName = New System.Windows.Forms.TextBox()
        Me.lblStats = New System.Windows.Forms.Label()
        Me.gbTop1000 = New System.Windows.Forms.GroupBox()
        Me.btnSSIDnonspecific = New System.Windows.Forms.Button()
        Me.btnTopSSIDs = New System.Windows.Forms.Button()
        Me.gbFilter.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbAP.SuspendLayout()
        Me.gbTop1000.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbIBSS
        '
        Me.cbIBSS.AutoSize = True
        Me.cbIBSS.Checked = True
        Me.cbIBSS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbIBSS.Location = New System.Drawing.Point(6, 116)
        Me.cbIBSS.Name = "cbIBSS"
        Me.cbIBSS.Size = New System.Drawing.Size(50, 17)
        Me.cbIBSS.TabIndex = 7
        Me.cbIBSS.Text = "IBSS"
        Me.cbIBSS.UseVisualStyleBackColor = True
        '
        'cbOPEN
        '
        Me.cbOPEN.AutoSize = True
        Me.cbOPEN.Checked = True
        Me.cbOPEN.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbOPEN.Location = New System.Drawing.Point(6, 19)
        Me.cbOPEN.Name = "cbOPEN"
        Me.cbOPEN.Size = New System.Drawing.Size(52, 17)
        Me.cbOPEN.TabIndex = 6
        Me.cbOPEN.Text = "Open"
        Me.cbOPEN.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Location = New System.Drawing.Point(642, 12)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(109, 23)
        Me.btnLoad.TabIndex = 5
        Me.btnLoad.Text = "&Load file"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'cbWPA2
        '
        Me.cbWPA2.AutoSize = True
        Me.cbWPA2.Checked = True
        Me.cbWPA2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbWPA2.Location = New System.Drawing.Point(6, 93)
        Me.cbWPA2.Name = "cbWPA2"
        Me.cbWPA2.Size = New System.Drawing.Size(57, 17)
        Me.cbWPA2.TabIndex = 5
        Me.cbWPA2.Text = "WPA2"
        Me.cbWPA2.UseVisualStyleBackColor = True
        '
        'cbWPA
        '
        Me.cbWPA.AutoSize = True
        Me.cbWPA.Checked = True
        Me.cbWPA.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbWPA.Location = New System.Drawing.Point(6, 65)
        Me.cbWPA.Name = "cbWPA"
        Me.cbWPA.Size = New System.Drawing.Size(51, 17)
        Me.cbWPA.TabIndex = 4
        Me.cbWPA.Text = "WPA"
        Me.cbWPA.UseVisualStyleBackColor = True
        '
        'gbFilter
        '
        Me.gbFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFilter.Controls.Add(Me.cbIBSS)
        Me.gbFilter.Controls.Add(Me.cbOPEN)
        Me.gbFilter.Controls.Add(Me.cbWPA2)
        Me.gbFilter.Controls.Add(Me.cbWPA)
        Me.gbFilter.Controls.Add(Me.cbWEP)
        Me.gbFilter.Location = New System.Drawing.Point(642, 70)
        Me.gbFilter.Name = "gbFilter"
        Me.gbFilter.Size = New System.Drawing.Size(109, 139)
        Me.gbFilter.TabIndex = 6
        Me.gbFilter.TabStop = False
        Me.gbFilter.Text = "Filter options"
        '
        'cbWEP
        '
        Me.cbWEP.AutoSize = True
        Me.cbWEP.Checked = True
        Me.cbWEP.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbWEP.Location = New System.Drawing.Point(6, 42)
        Me.cbWEP.Name = "cbWEP"
        Me.cbWEP.Size = New System.Drawing.Size(51, 17)
        Me.cbWEP.TabIndex = 3
        Me.cbWEP.Text = "WEP"
        Me.cbWEP.UseVisualStyleBackColor = True
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AllowUserToResizeColumns = False
        Me.dg.AllowUserToResizeRows = False
        Me.dg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Location = New System.Drawing.Point(12, 12)
        Me.dg.MultiSelect = False
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg.ShowCellErrors = False
        Me.dg.ShowCellToolTips = False
        Me.dg.ShowEditingIcon = False
        Me.dg.ShowRowErrors = False
        Me.dg.Size = New System.Drawing.Size(624, 465)
        Me.dg.TabIndex = 4
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(642, 41)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(109, 23)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&Save file as"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'gbAP
        '
        Me.gbAP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAP.Controls.Add(Me.lblAPContains)
        Me.gbAP.Controls.Add(Me.btnApRemove)
        Me.gbAP.Controls.Add(Me.txtAPName)
        Me.gbAP.Location = New System.Drawing.Point(642, 215)
        Me.gbAP.Name = "gbAP"
        Me.gbAP.Size = New System.Drawing.Size(109, 89)
        Me.gbAP.TabIndex = 8
        Me.gbAP.TabStop = False
        Me.gbAP.Text = "APNAME"
        '
        'lblAPContains
        '
        Me.lblAPContains.AutoSize = True
        Me.lblAPContains.Location = New System.Drawing.Point(3, 16)
        Me.lblAPContains.Name = "lblAPContains"
        Me.lblAPContains.Size = New System.Drawing.Size(85, 13)
        Me.lblAPContains.TabIndex = 2
        Me.lblAPContains.Text = "Contains string..."
        '
        'btnApRemove
        '
        Me.btnApRemove.Location = New System.Drawing.Point(6, 58)
        Me.btnApRemove.Name = "btnApRemove"
        Me.btnApRemove.Size = New System.Drawing.Size(97, 23)
        Me.btnApRemove.TabIndex = 1
        Me.btnApRemove.Text = "&Remove"
        Me.btnApRemove.UseVisualStyleBackColor = True
        '
        'txtAPName
        '
        Me.txtAPName.Location = New System.Drawing.Point(6, 32)
        Me.txtAPName.Name = "txtAPName"
        Me.txtAPName.Size = New System.Drawing.Size(97, 20)
        Me.txtAPName.TabIndex = 0
        '
        'lblStats
        '
        Me.lblStats.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStats.Location = New System.Drawing.Point(642, 385)
        Me.lblStats.Name = "lblStats"
        Me.lblStats.Size = New System.Drawing.Size(109, 92)
        Me.lblStats.TabIndex = 9
        '
        'gbTop1000
        '
        Me.gbTop1000.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTop1000.Controls.Add(Me.btnSSIDnonspecific)
        Me.gbTop1000.Controls.Add(Me.btnTopSSIDs)
        Me.gbTop1000.Location = New System.Drawing.Point(642, 310)
        Me.gbTop1000.Name = "gbTop1000"
        Me.gbTop1000.Size = New System.Drawing.Size(109, 72)
        Me.gbTop1000.TabIndex = 10
        Me.gbTop1000.TabStop = False
        Me.gbTop1000.Text = "Top 1000 SSIDs"
        '
        'btnSSIDnonspecific
        '
        Me.btnSSIDnonspecific.Location = New System.Drawing.Point(6, 19)
        Me.btnSSIDnonspecific.Name = "btnSSIDnonspecific"
        Me.btnSSIDnonspecific.Size = New System.Drawing.Size(97, 22)
        Me.btnSSIDnonspecific.TabIndex = 1
        Me.btnSSIDnonspecific.Text = "Filter containing"
        Me.btnSSIDnonspecific.UseVisualStyleBackColor = True
        '
        'btnTopSSIDs
        '
        Me.btnTopSSIDs.Location = New System.Drawing.Point(6, 44)
        Me.btnTopSSIDs.Name = "btnTopSSIDs"
        Me.btnTopSSIDs.Size = New System.Drawing.Size(97, 22)
        Me.btnTopSSIDs.TabIndex = 0
        Me.btnTopSSIDs.Text = "Filter specific"
        Me.btnTopSSIDs.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(763, 489)
        Me.Controls.Add(Me.gbTop1000)
        Me.Controls.Add(Me.lblStats)
        Me.Controls.Add(Me.gbAP)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.gbFilter)
        Me.Controls.Add(Me.dg)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "KML-Kpwn"
        Me.gbFilter.ResumeLayout(False)
        Me.gbFilter.PerformLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbAP.ResumeLayout(False)
        Me.gbAP.PerformLayout()
        Me.gbTop1000.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbIBSS As System.Windows.Forms.CheckBox
    Friend WithEvents cbOPEN As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents cbWPA2 As System.Windows.Forms.CheckBox
    Friend WithEvents cbWPA As System.Windows.Forms.CheckBox
    Friend WithEvents gbFilter As System.Windows.Forms.GroupBox
    Friend WithEvents cbWEP As System.Windows.Forms.CheckBox
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents gbAP As System.Windows.Forms.GroupBox
    Friend WithEvents btnApRemove As System.Windows.Forms.Button
    Friend WithEvents txtAPName As System.Windows.Forms.TextBox
    Friend WithEvents lblAPContains As System.Windows.Forms.Label
    Friend WithEvents lblStats As System.Windows.Forms.Label
    Friend WithEvents gbTop1000 As System.Windows.Forms.GroupBox
    Friend WithEvents btnTopSSIDs As System.Windows.Forms.Button
    Friend WithEvents btnSSIDnonspecific As System.Windows.Forms.Button
End Class
