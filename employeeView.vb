Imports MySql.Data.MySqlClient

Public Class employeeView

    Dim conn As MySqlConnection
    Dim Command As MySqlCommand
    Dim dbDataSet As New DataTable
    Dim reader As MySqlDataReader
    Private Sub btnclr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclr.Click
        txtname.Text = ""
        txtadrss.Text = ""
        txtcity.Text = ""
        txtdob.Text = ""
        txtgender.Text = ""
        txtphn.Text = ""
        txtsalary.Text = ""
        txttype.Text = ""
        txtid.Text = ""
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
    End Sub

    Private Sub btndlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndlt.Click

        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        Try
            If txtid.Text = "" Then
                MsgBox("NO DATA FOUND!!!")
                Return
            Else
                conn.Open()
                Dim Query As String
                Query = "delete from pacs.employee where eid='" & txtid.Text & "'"
                Command = New MySqlCommand(Query, conn)
                reader = Command.ExecuteReader
                MsgBox("data deleted")
                conn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdate.Click

        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        Try
            If txtid.Text = "" Then
                MsgBox("NO DATA FOUND!!!")
                Return
            Else
                conn.Open()
                Dim Query As String
                Query = "update pacs.employee set eid='" & txtid.Text & " ',ename='" & txtname.Text & "',address='" & txtadrss.Text & "',gender='" & txtgender.Text & "',city='" & txtcity.Text & "',phone='" & txtphn.Text & "',dob='" & txtdob.Text & "',salary='" & txtsalary.Text & "',designation='" & txttype.Text & "' where eid =' " & txtid.Text & " ' "
                Command = New MySqlCommand(Query, conn)
                reader = Command.ExecuteReader
                MsgBox("data updated")
                conn.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        mainfrm.Show()
        txtname.Text = ""
        txtadrss.Text = ""
        txtcity.Text = ""
        txtdob.Text = ""
        txtgender.Text = ""
        txtphn.Text = ""
        txtsalary.Text = ""
        txttype.Text = ""
        txtid.Text = ""
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        Dim reader As MySqlDataReader
        Try
            conn.Open()
            Dim query As String
            query = "select * from pacs.employee where eid='" & txtid.Text & "'"
            Command = New MySqlCommand(Query, conn)
            reader = Command.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    txtid.Text = reader.GetString("eid")
                    txtname.Text = reader.GetString("ename")
                    txtadrss.Text = reader.GetString("address")
                    txtcity.Text = reader.GetString("city")
                    txtdob.Text = reader.GetString("dob")
                    txtgender.Text = reader.GetString("gender")
                    txtphn.Text = reader.GetUInt64("phone")
                    txtsalary.Text = reader.GetDouble("salary")
                    txttype.Text = reader.GetString("designation")
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
                adapter = New MySqlDataAdapter("select photo,sign from pacs.employee where eid='" & txtid.Text & "'", conn)

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
            Else
                MsgBox("NO DATA FOUND!!!")
                txtname.Text = ""
                txtadrss.Text = ""
                txtcity.Text = ""
                txtdob.Text = ""
                txtgender.Text = ""
                txtphn.Text = ""
                txtsalary.Text = ""
                txttype.Text = ""
                PictureBox1.Image = Nothing
                PictureBox2.Image = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try  
    End Sub

    Private Sub employeeView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub employeeView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label4.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub txtsalary_lostfocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsalary.LostFocus
        If Not IsNumeric(txtsalary.Text) Then
            MsgBox("enter salary in numbers only", MsgBoxStyle.Information)
            txtsalary.Focus()
        End If
    End Sub

    Private Sub txtphn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtphn.LostFocus
        If Not IsNumeric(txtphn.Text) Then
            MsgBox("please enter numbers", MsgBoxStyle.Information)
        ElseIf txtphn.Text.Length < 10 Then
            MsgBox("Enter minimum 10 digit phone number", MsgBoxStyle.Exclamation)
            txtphn.Focus()
        End If
    End Sub
End Class

