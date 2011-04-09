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
        cbWPA.Enabled = True
        cbWPA2.Enabled = True
        cbWEP.Enabled = True
        cbOPEN.Enabled = True
        cbIBSS.Enabled = True
        btnApRemove.Enabled = True


        'Try

        'If Dir(OpenFileDialog1.FileName) <> "" Then
        'btnApplyFilter.Enabled = True
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

        If txtAPName.Text <> "" Then RemoveRecords(txtAPName.Text, "Placemark", "name", dsXML)
        '# bind to grid
        dg.DataMember = "Placemark"
        dg.DataSource = dsXML
        dg.Columns(1).Width = (dg.Width - dg.Columns(0).Width - 60)
        dg.Columns(2).Visible = False
    End Sub
    Function RemoveRecords(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet)

        Dim rc As New Collection '= dsXML.Tables("Placemark").Rows
        For Each r As DataRow In ds.Tables(sTable).Rows
            'If Not cbWEP.Checked Then
            If InStr(r(sColumn).ToString, sSearch) > 0 Then
                rc.Add(r)
            End If
            'End If
        Next
        For Each r As DataRow In rc
            r.Delete()
        Next
        ds.Tables(sTable).AcceptChanges()

    End Function
    'Private Sub btnApplyFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyFilter.Click
    'LoadFile()
    'End Sub

    Private Sub KML_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'btnApplyFilter.Enabled = False
        cbWPA.Enabled = False
        cbWPA2.Enabled = False
        cbWEP.Enabled = False
        cbOPEN.Enabled = False
        cbIBSS.Enabled = False
        btnApRemove.Enabled = False
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
        'If OpenFileDialog1.FileName <> "" Then LoadFile()
        If txtAPName.Text <> "" Then RemoveRecords(txtAPName.Text, "Placemark", "name", dsXML)
    End Sub
End Class