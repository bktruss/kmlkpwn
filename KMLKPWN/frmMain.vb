Imports System
Imports System.Data
Imports System.Data.SqlClient
Public Class frmMain
    Dim dsOriginal As New DataSet()
    Dim dsXML As New DataSet() '# holds current data
    Dim iCurrentFileTyoe As KMLFileTypes
    Public Enum KMLFileTypes
        Wardrive = 0
        WifiAgent = 1
    End Enum
    Dim bInLoad As Boolean = False
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        OpenFileDialog1.FileName = ""
        'OpenFileDialog1.Filter = "Android Wardrive KML files (*.kml)|*.kml|Android Wifi Agent KML files (*.kml)|*.kml" '# removed code until files are supported.
        OpenFileDialog1.Filter = "Android Wardrive KML files (*.kml)|*.kml"
        OpenFileDialog1.ShowDialog()

        'If OpenFileDialog1.FilterIndex = 1 Then '# wardrive '# removed if statement until files are supported.
        iCurrentFileTyoe = KMLFileTypes.Wardrive
        'ElseIf OpenFileDialog1.FilterIndex = 2 Then '# wifi agent 
        'iCurrentFileTyoe = KMLFileTypes.WifiAgent
        'End If

        If OpenFileDialog1.FileName.Trim <> "" Then
            If OpenFileDialog1.CheckFileExists Then
                If Dir(OpenFileDialog1.FileName) <> "" Then LoadFile() Else MessageBox.Show("Error opening file check file exists and is not corrupt.", "Error opening file.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

    End Sub
    Sub LoadFile()
        bInLoad = True
        Dim sSecurityTypes(0) As String
        sSecurityTypes(0) = ""

        Try
            '# clear dataset
            dsXML = Nothing
            dsXML = New DataSet
            dsXML.Clear()

            '# reload/load information
            dsXML.ReadXml(OpenFileDialog1.FileName)
            dsOriginal = dsXML.Copy

            ''# bind dataset
            dg.DataMember = "Placemark"
            dg.DataSource = dsXML
        Catch ex As Exception
            MessageBox.Show("Error loading KML file." & ex.Message)
        End Try

        Try
            '# add extra data columns to display data in grid
            Dim c As New DataColumn
            c.ColumnName = "Security"
            dsXML.Tables("placemark").Columns.Add(c)


            c = New DataColumn
            c.ColumnName = "BSSID"
            dsXML.Tables("placemark").Columns.Add(c)

            c = New DataColumn
            c.ColumnName = "Channel"
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

            c = New DataColumn
            c.ColumnName = "Location"
            dsXML.Tables("placemark").Columns.Add(c)
        Catch ex As Exception
            MessageBox.Show("Error adding extra columns to XML file. : " & ex.Message)
        End Try
      

        Select Case iCurrentFileTyoe
            Case KMLFileTypes.Wardrive
                

                Try

                    '# TMC	BSSID: <b>00:a0:f8:c4:c5:a1</b><br/>Capabilities: <b>[WPA-?]</b><br/>Frequency: <b>2462</b><br/>Level: <b>-90</b><br/>Timestamp: <b>1302547121511</b><br/>Date: <b>11 Apr 2011 19:38:41</b>
                    '# extract data from description 
                    For Each r As DataRow In dsXML.Tables("placemark").Rows

                        Dim sReturn As String
                        'If r("description") Then

                        '# sets text in security column
                        sReturn = Mid(r("description"), InStr(r("description"), "Capabilities: <b>") + 17)
                        sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Frequency:") - 1)
                        If sReturn = "" Then sReturn = "[OPEN]"
                        r("Security") = sReturn.Trim

                        Dim bFound As Boolean = False
                        For Each s As String In sSecurityTypes
                            If s.Trim.ToUpper = sReturn.Trim.ToUpper Then
                                bFound = True
                                Exit For
                            End If
                        Next

                        If Not bFound Then
                            ReDim Preserve sSecurityTypes(sSecurityTypes.GetUpperBound(0) + 1)
                            sSecurityTypes(sSecurityTypes.GetUpperBound(0)) = sReturn
                            bFound = False
                        End If

                        '# Security BSSID Frequency Timestamp Date Level
                        sReturn = Mid(r("description"), InStr(r("description"), "BSSID: <b>") + 10)
                        sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Capabilities:") - 1)
                        r("BSSID") = sReturn.ToUpper.Trim

                        sReturn = Mid(r("description"), InStr(r("description"), "Frequency: <b>") + 14)
                        sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Level:") - 1)
                        r("Frequency") = sReturn.Trim
                        Try
                            r("Channel") = FrequencyToChannel(CInt(sReturn.Trim))
                        Catch ex As Exception
                            r("Channel") = "Frequency error."
                            Debug.Write("Error converting frequency to channel. " & ex.Message)
                        End Try

                        sReturn = Mid(r("description"), InStr(r("description"), "Timestamp: <b>") + 14)
                        sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Date:") - 1)
                        r("Timestamp") = sReturn.Trim

                        sReturn = Mid(r("description"), InStr(r("description"), "Date: <b>") + 9)
                        sReturn = Mid(sReturn, 1, sReturn.Length - 4)
                        r("Date") = sReturn.Trim

                        sReturn = Mid(r("description"), InStr(r("description"), "Level: <b>") + 10)
                        sReturn = Mid(sReturn, 1, InStr(sReturn, "</b><br/>Timestamp:") - 1)
                        r("Level") = sReturn.Trim
                    Next
                Catch ex As Exception
                    MessageBox.Show("Error loading file." & vbCrLf & ex.Message & vbCrLf & ex.StackTrace)
                End Try
        End Select

        lbSecurityTypes.Items.Clear()
        For Each s As String In sSecurityTypes
            If s.Trim <> "" Then
                lbSecurityTypes.Items.Add(s, True)
            End If
        Next
        

        Try
            For i As Integer = 0 To dsXML.Tables("point").Rows.Count - 1
                Try
                    dsXML.Tables("placemark").Rows(i).Item("Location") = dsXML.Tables("point").Rows(i).Item("coordinates")
                Catch ex As Exception
                    MessageBox.Show("Error importing location." & ex.Message)
                End Try

            Next
        Catch ex As Exception
            MessageBox.Show("Error importing locations." & ex.Message)
        End Try


        '# call stats and draw grid
        btnSave.Enabled = True
        DrawStats()
        DrawGrid()
        UpdateHtml()
        bInLoad = False

        '# duplicate data set to hold orginal values
        dsOriginal = dsXML.Copy

        '# enable controls 
        gbFilter.Enabled = True
        gbAP.Enabled = True
        gbTop1000.Enabled = True

    End Sub
    Sub DrawStats() '# draws stats & graph

        Try
            '# setup graph
            cData.Series.Clear()
            cData.Palette = DataVisualization.Charting.ChartColorPalette.Grayscale
            cData.Series.Add("WIFI")

            Dim i As Integer = 0 '# integer for counting stats

            '# print stats to lblstats
            lblStats.Text = "Stats " & vbCrLf & "Total : " & dsXML.Tables("Placemark").Rows.Count

            For Each item In lbSecurityTypes.Items
                i = Count(item, "Placemark", "Security", dsXML, True)
                If i > 0 Then
                    If CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) > 5 Then
                        cData.Series("WIFI").Points.AddXY(item, i)
                    End If
                    lblStats.Text = lblStats.Text & vbCrLf & item & " : " & i & " (" & CInt((100 / dsXML.Tables("Placemark").Rows.Count) * i) & "%)"
                End If
            Next
        Catch ex As Exception
            '# catch errors and write to console
            'Debug.Write("Statistical error : " & ex.Message)
        End Try



    End Sub
    Sub DrawGrid() '# draws data grid

        On Error Resume Next
        '# check for records
        If IsNothing(dsXML) Then Exit Sub
        If dg.CurrentRow.Index < 0 Then Exit Sub

        '# set column headings
        dg.Columns(0).Width = 180
        dg.Columns(0).HeaderText = "SSID"
        dg.Columns(1).Visible = False '# hides description
        dg.Columns(2).Visible = False '# hides style

        For i As Integer = 0 To dg.Rows.Count
            '# set row colours
            If dg.Rows(i).Cells("styleUrl").Value.ToString = "#green" Then
                dg.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf dg.Rows(i).Cells("styleUrl").Value.ToString = "#yellow" Then
                dg.Rows(i).DefaultCellStyle.BackColor = Color.Yellow
            ElseIf dg.Rows(i).Cells("styleUrl").Value.ToString = "#red" Then
                dg.Rows(i).DefaultCellStyle.BackColor = Color.OrangeRed
            End If
        Next

        On Error GoTo 0


    End Sub
    Function Count(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet, Optional ByVal bExact As Boolean = False)
        '# counts networks to produce statictics
        Dim iCount As Integer = 0
        Dim rc As New Collection
        For Each r As DataRow In ds.Tables(sTable).Rows
            If bExact Then
                If r(sColumn).ToString.Trim.ToUpper = sSearch.Trim.ToUpper Then
                    iCount = iCount + 1
                End If
            Else
                If InStr(r(sColumn).ToString, sSearch) > 0 Then
                    iCount = iCount + 1
                End If
            End If

        Next
        Return iCount
    End Function
    Sub RemoveRecords(ByVal sSearch As String, ByVal sTable As String, ByVal sColumn As String, ByRef ds As DataSet, _
                      Optional ByVal bExactMatch As Boolean = False)
        '# removes records from the dataset
        Dim rc As New Collection

        For Each r As DataRow In ds.Tables(sTable).Rows
            If Not bExactMatch Then
                If InStr(r(sColumn).ToString, sSearch) > 0 Then
                    rc.Add(r)
                End If
            Else
                If r(sColumn).ToString.ToUpper.Trim = sSearch.ToUpper.Trim Then
                    rc.Add(r)
                End If
            End If
        Next

        For Each r As DataRow In rc
            r.Delete()
        Next
        ds.Tables(sTable).AcceptChanges()
    End Sub
    Function FrequencyToChannel(ByVal iFrequency As Integer) As String
        Dim sReturn As String = "(??)"
        Dim iCount As Integer = 1
        For i As Integer = 2412 To 2484 Step 5
            If iFrequency = i Then
                sReturn = "(" & IIf(iCount >= 10, iCount, "0" & iCount) & ")"
                Exit For
            End If
            iCount = iCount + 1
        Next
        Return sReturn
    End Function
    Private Sub KML_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '# set tooltips
        tooltips.SetToolTip(gbFilter, "Changing these settings will reset all filters. (Start by selecting the security types here then apply additional filters below.)")
        tooltips.SetToolTip(btnLoad, "Click here to load KML file.")
        tooltips.SetToolTip(btnSave, "Click here to save KML file.")
        tooltips.SetToolTip(btnApRemove, "Click to remove SSIDs that contain the above CASE SENSITIVE string.")
        tooltips.SetToolTip(btnRemoveSelected, "Click here to remove selected rows from your data.")
        tooltips.SetToolTip(dg, "Double click to jump to location via. http://www.openstreetmap.org/")

        '# disable controls
        gbFilter.Enabled = False
        gbAP.Enabled = False
        gbTop1000.Enabled = False
        btnSave.Enabled = False

        '# setup graph
        cData.Series.Clear()
        cData.BackColor = Me.BackColor
        cData.BackSecondaryColor = Me.BackColor

        '# check to see if a file was passed in when exe was called
        For Each argument As String In My.Application.CommandLineArgs
            If InStr(argument.ToUpper, ".KML") > 0 Then
                OpenFileDialog1.FileName = argument
                LoadFile()
            End If
        Next

    End Sub
    Private Sub cbWEP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbWPA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbWPA2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbIBSS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub cbOPEN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If OpenFileDialog1.FileName <> "" Then LoadFile()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click'# save file
        Try

            SaveFileDialog1.AddExtension = True
            SaveFileDialog1.Filter = "KML files (*.kml)|*.kml"
            If SaveFileDialog1.ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                Dim dsSave As New DataSet '= dsXML
                dsSave = dsXML.Copy

                '# remove temp columns we added on load
                dsSave.Tables("placemark").Columns.Remove("Security")
                dsSave.Tables("placemark").Columns.Remove("BSSID")
                dsSave.Tables("placemark").Columns.Remove("Frequency")
                dsSave.Tables("placemark").Columns.Remove("Timestamp")
                dsSave.Tables("placemark").Columns.Remove("Date")
                dsSave.Tables("placemark").Columns.Remove("Level")
                dsSave.Tables("placemark").Columns.Remove("Location")
                dsSave.Tables("placemark").Columns.Remove("Channel")
                dsSave.WriteXml(SaveFileDialog1.FileName)
                dsSave = Nothing
                If MessageBox.Show("File saved. Load saved file? Click 'Yes' to load the file you just saved, click no to continue working with your original file.", "Load saved data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    OpenFileDialog1.FileName = SaveFileDialog1.FileName
                    LoadFile()
                End If
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
    Sub FilterSSIDs(ByVal bSpecific As Boolean)'# filter out ssids from top 1000
        '# set wait cursor
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        '# read top 1000 to txt file
        Dim TextLines() As String = My.Resources.SSID.Split(Environment.NewLine.ToCharArray, System.StringSplitOptions.RemoveEmptyEntries)

        '# collection of rows to be removed
        Dim rc As New Collection

        '# check ds exists
        If IsNothing(dsXML) Then Exit Sub

        '# find matching records
        For Each r As DataRow In dsXML.Tables("Placemark").Rows
            Dim bFound As Boolean = False
            For i As Integer = 0 To UBound(TextLines)
                If bSpecific Then '# specific match
                    If r("name").trim = TextLines(i).Trim Then
                        bFound = True
                        Exit For
                    End If
                Else '# none specific match
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

        '# delete records
        For Each r As DataRow In rc
            r.Delete()
        Next

        '# update dataset
        dsXML.Tables("Placemark").AcceptChanges()

        DrawStats()
        DrawGrid()

        '# reset cursor
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

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        '# extract coordinates
        Dim sCoords As String() = dg.Rows(e.RowIndex).Cells("Location").Value.Split(New Char() {","c})

        '# build url
        Dim sStreetMap As String = "http://www.openstreetmap.org/?mlat=" & sCoords(1) & "&mlon=" & sCoords(0) & "&zoom=16"

        '# open url in browser
        System.Diagnostics.Process.Start(sStreetMap)
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        '# redraw html when record changes
        UpdateHtml()
    End Sub
    Sub UpdateHtml()
        '# function to update html
        Dim rLocation As DataRow = Nothing
        If IsNothing(dg.CurrentRow) Then Exit Sub
        Try
            For i As Integer = 0 To dsXML.Tables("placemark").Rows.Count
                If dsXML.Tables("placemark").Rows(i).Item("description").ToString.ToUpper = dg.CurrentRow.Cells("description").Value.ToString.ToUpper Then
                    rLocation = dsXML.Tables("point").Rows(i)
                    Exit For
                End If
            Next
            If IsNothing(rLocation) Then Exit Sub
            wbDescription.DocumentText = "<SPAN STYLE='font-size: xx-small; font-family : Arial;'>" & dg.CurrentRow.Cells("description").Value.ToString & "<br>Location : <b>" & rLocation("coordinates") & "</b></SPAN>"
        Catch ex As Exception
            MessageBox.Show("Error reading kml file :" & ex.Message)
        End Try      
    End Sub
    Private Sub dg_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.Sorted
        '# draws grid on column sort
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
    Private Sub lbSecurityTypes_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lbSecurityTypes.KeyUp
        If e.KeyCode <> Keys.Space Then Exit Sub
        FilterSecurity()
    End Sub

    Private Sub lbSecurityTypes_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbSecurityTypes.MouseUp
        FilterSecurity()
    End Sub
    Sub FilterSecurity()
        If bInLoad = True Then Exit Sub
        dsXML = dsOriginal.Copy

        For item As Integer = 0 To lbSecurityTypes.Items.Count - 1
            If lbSecurityTypes.GetItemChecked(item) = False Then
                RemoveRecords(lbSecurityTypes.Items(item).ToString.Trim.ToUpper, "Placemark", "Security", dsXML, True)
            End If
        Next
        dg.DataMember = "Placemark"
        dg.DataSource = dsXML
        dg.Update()
        DrawGrid()
        DrawStats()
    End Sub
End Class