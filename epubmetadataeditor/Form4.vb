Public Class Form4
    Dim charactersDisallowed As String = "/:*?<>|" + Chr(34)
    Dim SelectionText As String
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim insertText = "%Creator%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim insertText = "%CreatorFileAs%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim insertText = "%CreatorSurnameOnly%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim insertText = "%Title%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel5_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim insertText = "%TitleFileAs%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel6_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        Dim insertText = "%Date%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel7_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Dim insertText = "%Series%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel8_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        Dim insertText = "%SeriesIndex%"
        MakeInsertion(insertText)
    End Sub

    Private Sub LinkLabel9_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        Dim insertText As String = "%CreatorFirstInitial%"
        MakeInsertion(insertText)
    End Sub
    Private Sub MakeInsertion(ByVal insertText)
        Dim insertPos As Integer = TextBox1.SelectionStart
        TextBox1.SelectedText = ""
        If My.Computer.Keyboard.ShiftKeyDown Then
            TextBox1.Text = TextBox1.Text.Insert(insertPos, insertText.ToUpper)
        Else
            TextBox1.Text = TextBox1.Text.Insert(insertPos, insertText)
        End If

        TextBox1.SelectionStart = insertPos + insertText.Length
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Mid(TextBox1.Text, 1, 1) = "\" Then
            TextBox1.Text = Mid(TextBox1.Text, 2)
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        UpdateFilename()
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = "%" Then
            Label5.Visible = True
        Else
            Label5.Visible = False
        End If
    End Sub

    Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
        SelectionText = TextBox1.SelectedText
    End Sub

    Private Sub TextBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox1.MouseUp
        SelectionText = TextBox1.SelectedText
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Dim theText As String = TextBox1.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = TextBox1.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To TextBox1.Text.Length - 1
            Letter = TextBox1.Text.Substring(x, 1)
            If charactersDisallowed.Contains(Letter) Then
                If SelectionText = "" Then
                    theText = theText.Replace(Letter, String.Empty)
                    Change = 1
                    TextBox1.Text = theText
                    TextBox1.Select(SelectionIndex - Change, 0)
                Else
                    theText = theText.Replace(Letter, SelectionText)
                    Change = Len(SelectionText)
                    TextBox1.Text = theText
                    TextBox1.Select(SelectionIndex - 1, Change)
                End If
                Label4.Visible = True
                Label5.Visible = False
                Exit Sub
            End If
        Next
        Label4.Visible = False
    End Sub

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label4.Text = "The following characters are not allowed:" + Chr(10) + charactersDisallowed
        Label5.Text = "To output a single '%' in the filename," + Chr(10) + "use '%%' in the Template."
        Label4.Visible = False
        Label5.Visible = False
        If TextBox1.Text <> "" Then
            UpdateFilename()
        End If
    End Sub

    Public Sub UpdateFilename()
        Dim currpos, endpos, temppos As Integer
        Dim field, nextchar, insertText, newFileName As String
        newFileName = ""
        currpos = 0
        While (currpos < Len(TextBox1.Text))
            currpos = currpos + 1

            ' look for field marker
            If (Mid(TextBox1.Text, currpos, 1) = "%") Then
                If (Mid(TextBox1.Text, currpos + 1, 1) = "%") Then
                    ' found '%%' (replace with '%')
                    newFileName = newFileName + "%"
                    currpos = currpos + 1
                Else
                    ' look for end field marker
                    endpos = InStr(currpos + 1, TextBox1.Text, "%")
                    If (endpos <> 0) Then
                        ' end field marker found
                        field = Mid(TextBox1.Text, currpos + 1, endpos - currpos - 1)
                        insertText = ""
                        If field = "Creator" Then
                            insertText = Form1.TextBox2.Text
                        ElseIf field = "CREATOR" Then
                            insertText = Form1.TextBox2.Text.ToUpper
                        ElseIf field = "CreatorFileAs" Then
                            insertText = Form1.TextBox12.Text
                        ElseIf field = "CREATORFILEAS" Then
                            insertText = Form1.TextBox12.Text.ToUpper
                        ElseIf ((field = "CreatorSurnameOnly") Or (field = "CREATORSURNAMEONLY")) Then
                            insertText = Form1.TextBox2.Text
                            If InStr(insertText, " ") <> 0 Then
                                temppos = Len(insertText)
                                nextchar = Mid(insertText, temppos, 1)
                                While (nextchar <> " ")
                                    If temppos = 1 Then
                                        GoTo errortext
                                    End If
                                    temppos = temppos - 1
                                    nextchar = Mid(insertText, temppos, 1)
                                End While
                                insertText = Mid(insertText, temppos + 1)
                                If (Mid(Form1.TextBox2.Text, temppos - 1, 1) = ",") Then
                                    insertText = Form1.TextBox2.Text
                                    temppos = 1
                                    nextchar = Mid(insertText, temppos, 1)
                                    While (nextchar <> ",")
                                        If temppos = Len(insertText) Then
                                            GoTo errortext
                                        End If
                                        temppos = temppos + 1
                                        nextchar = Mid(insertText, temppos, 1)
                                    End While
                                    insertText = Mid(insertText, 1, temppos - 1)
                                End If
                            End If
                            If (field = "CREATORSURNAMEONLY") Then
                                insertText = insertText.ToUpper
                            End If
                        ElseIf ((field = "CreatorFirstInitial") Or (field = "CREATORFIRSTINITIAL")) Then
                            insertText = Form1.TextBox2.Text
                            If InStr(insertText, " ") <> 0 Then
                                temppos = Len(insertText)
                                nextchar = Mid(insertText, temppos, 1)
                                While (nextchar <> " ")
                                    If temppos = 1 Then
                                        GoTo errortext
                                    End If
                                    temppos = temppos - 1
                                    nextchar = Mid(insertText, temppos, 1)
                                End While
                                insertText = Mid(insertText, temppos + 1)
                                If (Mid(Form1.TextBox2.Text, temppos - 1, 1) = ",") Then
                                    insertText = Form1.TextBox2.Text
                                    temppos = 1
                                    nextchar = Mid(insertText, temppos, 1)
                                    While (nextchar <> ",")
                                        If temppos = Len(insertText) Then
                                            GoTo errortext
                                        End If
                                        temppos = temppos + 1
                                        nextchar = Mid(insertText, temppos, 1)
                                    End While
                                    insertText = Mid(insertText, 1, temppos - 1)
                                End If
                            End If
                            If Len(insertText) > 1 Then
                                insertText = Mid(insertText, 1, 1)
                            End If
                            If (field = "CREATORFIRSTINITIAL") Then
                                insertText = insertText.ToUpper
                            End If
                        ElseIf field = "Title" Then
                            insertText = Form1.TextBox1.Text
                        ElseIf field = "TITLE" Then
                            insertText = Form1.TextBox1.Text.ToUpper
                        ElseIf field = "TitleFileAs" Then
                            insertText = Form1.TextBox16.Text
                        ElseIf field = "TITLEFILEAS" Then
                            insertText = Form1.TextBox16.Text.ToUpper
                        ElseIf field = "Series" Then
                            insertText = Form1.TextBox15.Text
                        ElseIf field = "SERIES" Then
                            insertText = Form1.TextBox15.Text.ToUpper
                        ElseIf field = "SeriesIndex" Then
                            insertText = Form1.TextBox14.Text
                        ElseIf field = "SERIESINDEX" Then
                            insertText = Form1.TextBox14.Text.ToUpper
                        ElseIf field = "Date" Then
                            insertText = Form1.TextBox6.Text
                        ElseIf field = "DATE" Then
                            insertText = Form1.TextBox6.Text.ToUpper
                        Else
                            insertText = ""
                        End If
errortext:
                        newFileName = newFileName + insertText
                        currpos = endpos
                    End If
                End If
            Else
                newFileName = newFileName + Mid(TextBox1.Text, currpos, 1)
            End If
        End While

        'Replace illegal characters
        Dim Letter As String
        Dim x As Integer = 0
        While x < Len(newFileName)
            Letter = newFileName.Substring(x, 1)
            If charactersDisallowed.Contains(Letter) Then
                newFileName = newFileName.Replace(Letter, "-")
            End If
            x = x + 1
        End While

        'Replace html characters
        x = 1
        While x < Len(newFileName)
            If (Mid(newFileName, x, 5) = "&amp;") Then
                newFileName = newFileName.Replace("&amp;", "&")
            ElseIf (Mid(newFileName, x, 6) = "&quot;") Then
                newFileName = newFileName.Replace("&quot;", "'")
            End If
            x = x + 1
        End While

        TextBox2.Text = newFileName
    End Sub
End Class