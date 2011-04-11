Imports System
Imports System.Data
Imports System.Data.SqlClient
Public Class frmMain
    Dim dsXML As New DataSet()
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        If Dir(OpenFileDialog1.FileName) <> "" Then LoadFile() Else MessageBox.Show("Check file exists.")
    End Sub
    Sub LoadFile()

        '# enable controls 
        gbFilter.Enabled = True
        gbAP.Enabled = True
        gbTop1000.Enabled = True

        '# clear dataset
        dsXML.Clear()

        '# reload/load information
        dsXML.ReadXml(OpenFileDialog1.FileName)

        '# wep
        If Not cbWEP.Checked Then RemoveRecords("[WEP]", "Placemark", "description", dsXML)

        '# wpa
        If Not cbWPA.Checked Then RemoveRecords("[WPA-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-PSK-TKIP]", "Placemark", "Description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-EAP-TKIP]", "Placemark", "description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-PSK-CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-EAP-CCMP]", "Placemark", "description", dsXML)

        '# wpa2
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-TKIP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-CCMP-preauth]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-EAP-CCMP]", "Placemark", "description", dsXML)

        '# ibss
        If Not cbIBSS.Checked Then RemoveRecords("[IBSS]", "Placemark", "description", dsXML)

        '# open
        If Not cbOPEN.Checked Then RemoveRecords("Capabilities: <b></b>", "Placemark", "description", dsXML)
        dg.DataMember = "Placemark"
        dg.DataSource = dsXML

        '# call stats and draw grid
        DrawStats()
        DrawGrid()

    End Sub
    Sub DrawStats()

        Try
            cData.Series.Clear()
            cData.PaletteCustomColors = {Color.LightGreen, Color.Yellow, Color.Red, Color.OrangeRed, Color.Blue}
            cData.Palette = DataVisualization.Charting.ChartColorPalette.None
            cData.Series.Add("OPEN")
            cData.Series.Add("WEP")
            cData.Series.Add("WPA")
            cData.Series.Add("WPA2")
            cData.Series.Add("IBSS")

            Dim i As Integer
            '# print stats to lblstats
            lblStats.Text = "Stats " & vbCrLf & "Total : " & dsXML.Tables("Placemark").Rows.Count

            i = Count("Capabilities: <b></b>", "Placemark", "description", dsXML)
            cData.Series("OPEN").Points.AddXY("OPEN", i)
            lblStats.Text = lblStats.Text & vbCrLf & "OPEN : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WEP]", "Placemark", "description", dsXML)
            cData.Series("WEP").Points.AddXY("WEP", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WEP : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-EAP-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-PSK-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-PSK-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-EAP-CCMP]", "Placemark", "description", dsXML)
            cData.Series("WPA").Points.AddXY("WPA", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA2-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP-preauth]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-EAP-CCMP]", "Placemark", "description", dsXML)
            cData.Series("WPA2").Points.AddXY("WPA2", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA2 : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[IBSS]", "Placemark", "description", dsXML)
            cData.Series("IBSS").Points.AddXY("IBSS", i)
            lblStats.Text = lblStats.Text & vbCrLf & "IBSS : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"
        Catch ex As Exception
            '# catch errors and write to console
            'Debug.Write("Statistical error : " & ex.Message)
        End Try

    End Sub
    Sub DrawGrid()
        On Error Resume Next
        '# check for records
        If dg.CurrentRow.Index < 0 Then Exit Sub

        '# set column headings
        dg.Columns(0).Width = 180
        dg.Columns(0).HeaderText = "SSID"
        dg.Columns(1).Width = (dg.Width - dg.Columns(0).Width - 20)
        dg.Columns(1).HeaderText = "Description"
        dg.Columns(2).Visible = False

        'For Each r As DataRow In dsXML.Tables("").Rows("")

        'Next
        For i As Integer = 0 To dg.Rows.Count
            'If dg.Columns()("styleUrl").ToString = "#green" Then
            If dg.Rows(i).Cells("styleUrl").Value.ToString = "#green" Then
                '# colour row green
                dg.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf dg.Rows(i).Cells("styleUrl").Value.ToString = "#yellow" Then
                dg.Rows(i).DefaultCellStyle.BackColor = Color.GreenYellow
            ElseIf dg.Rows(i).Cells("styleUrl").Value.ToString = "#red" Then
                dg.Rows(i).DefaultCellStyle.BackColor = Color.OrangeRed

            End If
        Next
        On Error GoTo 0

    End Sub
    Function Count(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet)
        '# counts networks to produce statictics
        Dim iCount As Integer = 0
        Dim rc As New Collection
        For Each r As DataRow In ds.Tables(sTable).Rows
            If InStr(r(sColumn).ToString, sSearch) > 0 Then
                iCount = iCount + 1
            End If
        Next
        Return iCount
    End Function
    Sub RemoveRecords(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet)
        '# removes records from the dataset
        Dim rc As New Collection
        For Each r As DataRow In ds.Tables(sTable).Rows
            If InStr(r(sColumn).ToString, sSearch) > 0 Then
                rc.Add(r)
            End If
        Next
        For Each r As DataRow In rc
            r.Delete()
        Next
        ds.Tables(sTable).AcceptChanges()
    End Sub
    Private Sub KML_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '# form load events
        '# disable controls
        gbFilter.Enabled = False
        gbAP.Enabled = False
        gbTop1000.Enabled = False

        '# set check box colours
        cbOPEN.ForeColor = Color.Green
        cbWEP.ForeColor = Color.GreenYellow
        cbWPA.ForeColor = Color.Red
        cbWPA2.ForeColor = Color.Red
        cbIBSS.ForeColor = Color.DarkRed

        cData.Series.Clear()
        cData.BackColor = Me.BackColor
        cData.BackSecondaryColor = Me.BackColor
        'cData.
    End Sub
    Private Sub cbWEP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWEP.CheckedChanged
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbWPA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWPA.CheckedChanged
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbWPA2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWPA2.CheckedChanged
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbIBSS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIBSS.CheckedChanged
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbOPEN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOPEN.CheckedChanged
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        '# save file
        SaveFileDialog1.AddExtension = True
        SaveFileDialog1.Filter = "KML files (*.kml)|*.kml"
        If SaveFileDialog1.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then dsXML.WriteXml(SaveFileDialog1.FileName)
    End Sub
    Private Sub btnApRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApRemove.Click
        '# remove ap's
        If txtAPName.Text <> "" Then RemoveRecords(txtAPName.Text, "Placemark", "name", dsXML)
        '# update grid
        DrawStats()
        DrawGrid()
    End Sub
    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        '# draw grid on resize
        DrawGrid()
    End Sub
    Private Sub btnTopSSIDs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopSSIDs.Click
        '# filter list where ssid = ssid (from top 1000)
        FilterSSIDs(True)
    End Sub
    Sub FilterSSIDs(ByVal bSpecific As Boolean)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim TextLines() As String = My.Resources.SSID.Split(Environment.NewLine.ToCharArray, System.StringSplitOptions.RemoveEmptyEntries)
        Dim rc As New Collection
        If IsNothing(dsXML) Then Exit Sub
        For Each r As DataRow In dsXML.Tables("Placemark").Rows
            Dim bFound As Boolean = False
            For i As Integer = 0 To UBound(TextLines)
                If bSpecific Then
                    If r("name").trim = TextLines(i).Trim Then
                        bFound = True
                        Exit For
                    End If
                Else
                    If InStr(r("name"), TextLines(i)) > 0 Then
                        bFound = True
                        Exit For
                    End If
                End If
            Next
            If Not bFound Then
                rc.Add(r)
            End If
            bFound = False
        Next

        For Each r As DataRow In rc
            r.Delete()
        Next
        dsXML.Tables("Placemark").AcceptChanges()
        DrawStats()
        DrawGrid()
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub
    Private Sub btnSSIDnonspecific_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSSIDnonspecific.Click
        '# filter list where ssid contains ssid (from top 1000)
        FilterSSIDs(False)
    End Sub
    Private Sub dg_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellClick
        '# redraw html when record changes
        UpdateHtml()
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        '# redraw html when record changes
        UpdateHtml()
    End Sub
    Sub UpdateHtml()
        '# function to update html
        Dim r1 As DataRow
        Dim r2 As DataRow
        If dg.CurrentRow.Index < 0 Then Exit Sub
        r1 = dsXML.Tables("Placemark").Rows(dg.CurrentRow.Index)
        r2 = dsXML.Tables("point").Rows(dg.CurrentRow.Index)
        wbDescription.DocumentText = r1("description") & "<br>Location : <b>" & r2("coordinates") & "</b>"
    End Sub

    Private Sub dg_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.Sorted
        DrawGrid()
    End Sub

    Private Sub lblStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblStats.Click

    End Sub
End Class