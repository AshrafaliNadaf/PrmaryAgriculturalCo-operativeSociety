'// Member view form
Imports MySql.Data.MySqlClient
Public Class memberView

    Dim conn As MySqlConnection
    Dim Command As MySqlCommand
    Dim dbDataSet As New DataTable
    Dim sda As New MySqlDataAdapter
    Dim bSource As New BindingSource
    Dim reader As MySqlDataReader
    Private Sub memberView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        mainfrm.Show()
        Me.Hide()
    End Sub

    Private Sub memberView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label3.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized

        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"

        Dim sda As New MySqlDataAdapter
        Dim bSource As New BindingSource
        conn.Open()
        Dim q As String = "select mid as 'Adhar No',name as 'Name' from member "
        Dim adpt As New MySqlDataAdapter(q, conn)
        Dim ds As New DataSet()
        adpt.Fill(ds, "member")
        gridview.DataSource = ds.Tables(0)
        conn.Close()
    End Sub

    Private Sub btnview_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        Dim reader As MySqlDataReader

        Try
            conn.Open()
            Dim query As String
            query = "SELECT * from member WHERE member.mid = '" & txtmid.Text & "'"
            Command = New MySqlCommand(query, conn)
            reader = Command.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    txtmid.Text = reader.GetUInt64("mid")
                    txtname.Text = reader.GetString("name")
                    txtadrss.Text = reader.GetString("address")
                    txtcity.Text = reader.GetString("city")
                    txtgender.Text = reader.GetString("gender")
                    txtphn.Text = reader.GetUInt64("phone")
                    txtdob.Text = reader.GetString("dob")
                    txtamount.Text = reader.GetDouble("amount")

                End While
                conn.Close()


                'to feth image
                Dim adapter As New MySqlDataAdapter
                adapter.SelectCommand = Command
                Dim data As DataTable
                Dim commandbuild As MySqlCommandBuilder
                data = New DataTable
                conn = New MySqlConnection
                conn.ConnectionString = "server=localhost;userid=root;database=pacs"
                conn.Open()
                adapter = New MySqlDataAdapter("select photo,sign from pacs.member where mid='" & txtmid.Text & "'", conn)

                commandbuild = New MySqlCommandBuilder(adapter)
                adapter.Fill(data)

                Dim lb() As Byte = data.Rows(0).Item("photo")
                Dim lb1() As Byte = data.Rows(0).Item("sign")
                Dim lstr As New System.IO.MemoryStream(lb)
                Dim lstr1 As New System.IO.MemoryStream(lb1)
                PictureBox1.Image = Image.FromStream(lstr)
                PictureBox2.Image = Image.FromStream(lstr1)
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
                lstr.Close()
                lstr1.Close()

            Else : MsgBox("NO DATA FOUND!!!")
                PictureBox1.Image = Nothing
                PictureBox2.Image = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

       
    End Sub

    Private Sub btnclear_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        txtadrss.Text = ""
        txtamount.Text = ""
        txtcity.Text = ""
        txtdob.Text = ""
        txtgender.Text = ""
        txtname.Text = ""
        txtmid.Text = ""
        txtphn.Text = ""
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        'DataGridView1.DataSource = Nothing
        'DataGridView1.Refresh()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        mainfrm.Show()
        Me.Close()
    End Sub

    Private Sub btnupdate_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdate.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        conn.Open()
            Dim query As String
            query = "SELECT * from member WHERE member.mid = '" & txtmid.Text & "'"
            Command = New MySqlCommand(query, conn)
            reader = Command.ExecuteReader
            If reader.HasRows Then
                Try
                    If txtmid.Text = "" Then
                        MsgBox("NO DATA FOUND!!!")
                        Return
                    ElseIf Not IsNumeric(txtphn.Text) Then
                        MsgBox("enter phone number in numbers only", MsgBoxStyle.Information)
                    Else
                        'conn.Open()
                        'Dim query As String
                        reader.Close()
                        query = "update pacs.member set mid='" & txtmid.Text & " ',name='" & txtname.Text & "',address='" & txtadrss.Text & "',gender='" & txtgender.Text & "',city='" & txtcity.Text & "',phone='" & txtphn.Text & "',dob='" & txtdob.Text & "',amount='" & txtamount.Text & "' where mid =' " & txtmid.Text & " ' "
                        Command = New MySqlCommand(query, conn)
                        reader = Command.ExecuteReader
                        MsgBox("data updated")
                        conn.Close()
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Else
                MsgBox(" NO DATA")
            End If
    End Sub


    Private Sub btndlt_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndlt.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        conn.Open()
        If txtmid.Text = "" Then
            MsgBox("NO DATA FOUND!!!")
            Return
        End If
        Dim query As String
        query = "select *from pacs.member where member.mid=' " & txtmid.Text & " '"
        Command = New MySqlCommand(query, conn)
        reader = Command.ExecuteReader
        If reader.HasRows Then
            reader.Close()

            Try
                Dim dialog As DialogResult
                dialog = MsgBox("Are you sure to delete member...?", MessageBoxButtons.YesNo)
                If dialog = DialogResult.No Then
                    Return
                Else
                    'conn.Open()
                    Command = New MySqlCommand("delete from pacs.member where mid='" & txtmid.Text & "'", conn)
                    reader = Command.ExecuteReader
                    MsgBox("Member Deleted")
                    conn.Close()
                End If
                reader.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
        MsgBox("NO DATA FOUND!!!")
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        End If
    End Sub

    Private Sub txtphn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtphn.LostFocus
        If txtphn.Text.Length < 10 Then
            MsgBox("enter minimum 10 digit phone number", MsgBoxStyle.Information)
            txtphn.Focus()
        ElseIf Not IsNumeric(txtphn.Text) Then
            MsgBox("enter phone number in numbers only", MsgBoxStyle.Information)
            txtphn.Focus()
        End If
    End Sub

    'Private Sub txtsearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    txtsearch.Text = ""
    'End Sub

    'Private Sub txtsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim dv As New DataView(dbDataSet)

    '    Try
    '        dv.RowFilter = String.Format(" name Like '%{0}%'", txtsearch.Text)
    '        gridview.DataSource = dv
    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message)
    '    End Try

    'End Sub

    'Private Sub btnshw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnshw.Click
    '    Dim adapter As New MySqlDataAdapter
    '    adapter.SelectCommand = Command
    '    Dim data As DataTable
    '    Dim commandbuild As MySqlCommandBuilder
    '    data = New DataTable
    '    conn = New MySqlConnection
    '    conn.ConnectionString = "server=localhost;userid=root;database=pacs"
    '    conn.Open()
    '    adapter = New MySqlDataAdapter("select photo,sign from pacs.member where mid='" & txtmid.Text & "'", conn)

    '    commandbuild = New MySqlCommandBuilder(adapter)
    '    adapter.Fill(data)

    '    Dim lb() As Byte = data.Rows(0).Item("photo")
    '    Dim lb1() As Byte = data.Rows(0).Item("sign")
    '    Dim lstr As New System.IO.MemoryStream(lb)
    '    Dim lstr1 As New System.IO.MemoryStream(lb1)
    '    PictureBox1.Image = Image.FromStream(lstr)
    '    PictureBox2.Image = Image.FromStream(lstr1)
    '    PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
    '    PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
    '    lstr.Close()
    '    lstr1.Close()
    'End Sub

   
    'Private Sub txtsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsearch.TextChanged
    '    Dim dv As New DataView(dbDataSet)
    '    Try
    '        dv.RowFilter = String.Format(" name Like '%{0}%'", txtsearch.Text)
    '        gridview.DataSource = dv
    '    Catch ex As Exception
    '        'MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    Private Sub gridview_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridview.CellContentClick

        Dim i As Integer
        i = gridview.CurrentRow.Index
        txtmid.Text = gridview.Item(0, i).Value

    End Sub

    Private Sub txtsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class