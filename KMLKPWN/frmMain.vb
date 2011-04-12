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
        If Not cbWPA.Checked Then RemoveRecords("[WPA-?]", "Placemark", "description", dsXML)
        '[WPA-?]

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

        '# add extra data columns
        Dim c As New DataColumn
        c.ColumnName = "Security"
        dsXML.Tables("placemark").Columns.Add(c)
        c = New DataColumn
        c.ColumnName = "BSSID"
        dsXML.Tables("placemark").Columns.Add(c)

        c = New DataColumn
        c.ColumnName = "Frequency"
        dsXML.Tables("placemark").Columns.Add(c)

        c = New DataColumn
        c.ColumnName = "Timestamp"
        dsXML.Tables("placemark").Columns.Add(c)

        c = New DataColumn
        c.ColumnName = "Date"
        dsXML.Tables("placemark").Columns.Add(c)

        c = New DataColumn
        c.ColumnName = "Level"
        dsXML.Tables("placemark").Columns.Add(c)

        '# TMC	BSSID: <b>00:a0:f8:c4:c5:a1</b><br/>Capabilities: <b>[WPA-?]</b><br/>Frequency: <b>2462</b><br/>Level: <b>-90</b><br/>Timestamp: <b>1302547121511</b><br/>Date: <b>11 Apr 2011 19:38:41</b>
        For Each r As DataRow In dsXML.Tables("placemark").Rows
            Dim sReturn As String
            'If r("description") Then

            '# sets text in security column
            sReturn = Mid(r("description"), InStr(r("description"), "Capabilities: <b>") + 17)
            sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Frequency:") - 1)
            If sReturn = "" Then sReturn = "[OPEN]"
            r("Security") = sReturn
            '# Security BSSID Frequency Timestamp Date Level
            sReturn = Mid(r("description"), InStr(r("description"), "BSSID: <b>") + 10)
            sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Capabilities:") - 1)
            r("BSSID") = sReturn

            sReturn = Mid(r("description"), InStr(r("description"), "Frequency: <b>") + 14)
            sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Level:") - 1)
            r("Frequency") = sReturn

            sReturn = Mid(r("description"), InStr(r("description"), "Timestamp: <b>") + 14)
            sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Date:") - 1)
            r("Timestamp") = sReturn

            sReturn = Mid(r("description"), InStr(r("description"), "Date: <b>") + 9)
            sReturn = Mid(sReturn, 1, sReturn.Length - 4)
            r("Date") = sReturn

            sReturn = Mid(r("description"), InStr(r("description"), "Level: <b>") + 10)
            sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Timestamp:") - 1)
            r("Level") = sReturn
        Next
        

        '# call stats and draw grid
        DrawStats()
        DrawGrid()
        UpdateHtml()
    End Sub
    Sub DrawStats()

        Try
            cData.Series.Clear()
            'cData.PaletteCustomColors = {Color.LightGreen, Color.Yellow, Color.Red, Color.OrangeRed, Color.Blue}
            cData.Palette = DataVisualization.Charting.ChartColorPalette.Grayscale
            cData.Series.Add("WIFI")
            'cData.Series.Add("WEP")
            'cData.Series.Add("WPA")
            'cData.Series.Add("WPA2")
            'cData.Series.Add("IBSS")

            Dim i As Integer
            '# print stats to lblstats
            lblStats.Text = "Stats " & vbCrLf & "Total : " & dsXML.Tables("Placemark").Rows.Count

            i = Count("Capabilities: <b></b>", "Placemark", "description", dsXML)
            cData.Series("WIFI").Points.AddXY("OPEN", i)
            lblStats.Text = lblStats.Text & vbCrLf & "OPEN : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WEP]", "Placemark", "description", dsXML)
            cData.Series("WIFI").Points.AddXY("WEP", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WEP : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-EAP-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-PSK-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-PSK-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-EAP-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-?]", "Placemark", "description", dsXML)
            cData.Series("WIFI").Points.AddXY("WPA", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA2-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP-preauth]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-EAP-CCMP]", "Placemark", "description", dsXML)
            cData.Series("WIFI").Points.AddXY("WPA2", i)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA2 : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[IBSS]", "Placemark", "description", dsXML)
            cData.Series("WIFI").Points.AddXY("IBSS", i)
            lblStats.Text = lblStats.Text & vbCrLf & "IBSS : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"
        Catch ex As Exception
            '# catch errors and write to console
            'Debug.Write("Statistical error : " & ex.Message)
        End Try



    End Sub
    Sub DrawGrid()
        On Error Resume Next
        '# check for records
        If IsNothing(dsXML) Then Exit Sub
        If dg.CurrentRow.Index < 0 Then Exit Sub

        '# set column headings
        dg.Columns(0).Width = 180
        dg.Columns(0).HeaderText = "SSID"
        'dg.Columns(1).Width = (dg.Width - dg.Columns(0).Width - 20)
        'dg.Columns(1).HeaderText = "Description"
        dg.Columns(1).Visible = False
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
        For Each argument As String In My.Application.CommandLineArgs
            If InStr(argument.ToUpper, ".KML") > 0 Then
                OpenFileDialog1.FileName = argument
                LoadFile()
            End If
        Next
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
        Try
            SaveFileDialog1.AddExtension = True
            SaveFileDialog1.Filter = "KML files (*.kml)|*.kml"
            If SaveFileDialog1.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                Dim dsSave As DataSet = dsXML.Clone
                '# Security BSSID Frequency Timestamp Date Level
                dsSave.Tables("placemark").Columns.Remove("Security")
                dsSave.Tables("placemark").Columns.Remove("BSSID")
                dsSave.Tables("placemark").Columns.Remove("Frequency")
                dsSave.Tables("placemark").Columns.Remove("Timestamp")
                dsSave.Tables("placemark").Columns.Remove("Date")
                dsSave.Tables("placemark").Columns.Remove("Level")
                dsSave.AcceptChanges()
                dsSave.WriteXml(SaveFileDialog1.FileName)
                dsSave = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        

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
        Dim rLocation As DataRow
        'Dim r2 As DataRow



        If IsNothing(dg.CurrentRow) Then Exit Sub
        'If dg.CurrentRow.Index < 0 Then Exit Sub
        'r1 = dsXML.Tables("Placemark").Rows(dg.CurrentRow.Index)
        'r2 = dsXML.Tables("point").Rows(dg.CurrentRow.Index)
        'r1 = dsXML.Tables("placemark").Rows(dg.CurrentRow.Index)
        'r2 = dsXML.Tables("point").Rows(dg.CurrentRow.Index)

        'For Each r As DataRow In dsXML.Tables("placemark").Rows
        For i As Integer = 0 To dsXML.Tables("placemark").Rows.Count
            If dsXML.Tables("placemark").Rows(i).Item("description").ToString.ToUpper = dg.CurrentRow.Cells("description").Value.ToString.ToUpper Then
                rLocation = dsXML.Tables("point").Rows(i)
                Exit For
            End If
        Next

        wbDescription.DocumentText = dg.CurrentRow.Cells("description").Value.ToString & "<br>Location : <b>" & rLocation("coordinates") & "</b>"
    End Sub

    Private Sub dg_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.Sorted
        DrawGrid()
    End Sub

    Private Sub btnRemoveSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSelected.Click
        For Each rselected In dg.SelectedRows
            Dim sSearch As String = rselected.Cells("description").Value
            Dim rRemove As DataRow = Nothing
            For Each r As DataRow In dsXML.Tables("placemark").Rows
                If r("description") = sSearch Then
                    rRemove = r
                    Exit For
                End If
            Next
            If Not IsNothing(rRemove) Then
                rRemove.Delete()
                DrawGrid()
                DrawStats()
            End If
        Next
        
    End Sub
End Class