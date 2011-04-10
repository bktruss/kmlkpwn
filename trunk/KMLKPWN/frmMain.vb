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

        gbFilter.Enabled = True
        gbAP.Enabled = True
        gbTop1000.Enabled = True

        dsXML.Clear()
        dsXML.ReadXml(OpenFileDialog1.FileName)
        '# wep
        If Not cbWEP.Checked Then RemoveRecords("[WEP]", "Placemark", "description", dsXML)
        '# wpa
        If Not cbWPA.Checked Then RemoveRecords("[WPA-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-EAP-TKIP]", "Placemark", "description", dsXML)
        If Not cbWPA.Checked Then RemoveRecords("[WPA-PSK-TKIP]", "Placemark", "Description", dsXML)
        '# wpa2
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-CCMP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-TKIP]", "Placemark", "description", dsXML)
        If Not cbWPA2.Checked Then RemoveRecords("[WPA2-PSK-CCMP-preauth]", "Placemark", "description", dsXML)
        '# ibss
        If Not cbIBSS.Checked Then RemoveRecords("[IBSS]", "Placemark", "description", dsXML)
        '# open
        If Not cbOPEN.Checked Then RemoveRecords("Capabilities: <b></b>", "Placemark", "description", dsXML)
        dg.DataMember = "Placemark"
        dg.DataSource = dsXML

        DrawStats()
        DrawGrid()
    End Sub
    Sub DrawStats()

        Try
            'If dsXML.Tables.Count = 0 Then Exit Sub
            'If dsXML.Tables("Placemark").Rows.Count Then Exit Sub

            Dim i As Integer
            lblStats.Text = "Stats " & vbCrLf & "Total : " & dsXML.Tables("Placemark").Rows.Count

            i = Count("Capabilities: <b></b>", "Placemark", "description", dsXML)
            lblStats.Text = lblStats.Text & vbCrLf & "OPEN : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WEP]", "Placemark", "description", dsXML)
            lblStats.Text = lblStats.Text & vbCrLf & "WEP : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-EAP-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA-PSK-TKIP]", "Placemark", "description", dsXML)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[WPA2-PSK-TKIP+CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-TKIP]", "Placemark", "description", dsXML)
            i = i + Count("[WPA2-PSK-CCMP-preauth]", "Placemark", "description", dsXML)
            lblStats.Text = lblStats.Text & vbCrLf & "WPA2 : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"

            i = Count("[IBSS]", "Placemark", "description", dsXML)
            lblStats.Text = lblStats.Text & vbCrLf & "IBSS : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"
            '# bind to grid
            
        Catch ex As Exception

        End Try
        
    End Sub
    Sub DrawGrid()
        On Error Resume Next
        dg.Columns(0).Width = 180
        dg.Columns(1).HeaderText = "SSID"
        dg.Columns(1).Width = (dg.Width - dg.Columns(0).Width - 60)
        dg.Columns(1).HeaderText = "Description"
        dg.Columns(2).Visible = False
        On Error GoTo 0
    End Sub
    Function Count(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet)
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
        gbFilter.Enabled = False
        gbAP.Enabled = False
        gbTop1000.Enabled = False
        cbOPEN.ForeColor = Color.Green
        cbWEP.ForeColor = Color.GreenYellow
        cbWPA.ForeColor = Color.Orange
        cbWPA2.ForeColor = Color.Red
        cbIBSS.ForeColor = Color.DarkRed
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
        SaveFileDialog1.AddExtension = True
        SaveFileDialog1.Filter = "KML files (*.kml)|*.kml"
        If SaveFileDialog1.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then dsXML.WriteXml(SaveFileDialog1.FileName)
    End Sub
    Private Sub btnApRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApRemove.Click
        If txtAPName.Text <> "" Then RemoveRecords(txtAPName.Text, "Placemark", "name", dsXML)
        DrawStats()
        DrawGrid()
    End Sub
    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        DrawGrid()
    End Sub

    Private Sub btnTopSSIDs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopSSIDs.Click
        'Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        'Dim TextLines() As String = My.Resources.SSID.Split(Environment.NewLine.ToCharArray, System.StringSplitOptions.RemoveEmptyEntries)
        'Dim rc As New Collection
        'If IsNothing(dsXML) Then Exit Sub
        'For Each r As DataRow In dsXML.Tables("Placemark").Rows
        '    Dim bFound As Boolean = False
        '    For i As Integer = 0 To UBound(TextLines)
        '        'If InStr(r("name"), TextLines(i)) > 0 Then
        '        '    bFound = True
        '        '    Exit For
        '        'End If
        '        If r("name").trim = TextLines(i).Trim Then
        '            bFound = True
        '            Exit For
        '        End If
        '    Next
        '    If Not bFound Then
        '        rc.Add(r)
        '    End If
        '    bFound = False
        'Next

        'For Each r As DataRow In rc
        '    r.Delete()
        'Next
        'dsXML.Tables("Placemark").AcceptChanges()
        'DrawStats()
        'DrawGrid()
        'Me.Cursor = System.Windows.Forms.Cursors.Default
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
        FilterSSIDs(False)
    End Sub
End Class