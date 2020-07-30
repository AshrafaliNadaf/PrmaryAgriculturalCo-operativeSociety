Imports MySql.Data.MySqlClient
Public Class employee
    Dim gender As String
    Dim memname As String
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader

    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"

        Dim mstream As New System.IO.MemoryStream()
        Dim mstream1 As New System.IO.MemoryStream()
        PictureBox1.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
        PictureBox2.Image.Save(mstream1, System.Drawing.Imaging.ImageFormat.Jpeg)
        Dim arrImage() As Byte = mstream.GetBuffer()
        Dim arrImage1() As Byte = mstream1.GetBuffer()

        If txtaddr.Text = "" Or txteid.Text = "" Or txtamt.Text = "" Or txtcity.Text = "" Or txtdate.Text = "" Then
            MsgBox("All fields are mandatory", MsgBoxStyle.Information)
            Return
            If Not IsNumeric(txtamt.Text) Then
                MsgBox("enter salary in numbers only", MsgBoxStyle.Information)
            End If
        ElseIf txtdate.Text.Length < 10 Then
            MsgBox("Enter valid date of birth ")
            txtdate.Focus()
            'ElseIf IsNothing(PictureBox1.Image) Then
            '    MsgBox("plaese provide a photo ", MsgBoxStyle.Information)
            'ElseIf IsNothing(PictureBox2.Image) Then
            '    MsgBox("plaese provide a photo ", MsgBoxStyle.Information)
        Else
            Dim empname As String
            mstream.Close()
            Dim Query As String
            empname = txtname1.Text + " " + txtnname2.Text + " " + txtname3.Text
            Query = "INSERT INTO pacs.employee(eid,ename,address,city,gender,dob,phone,salary,designation,photo,sign)VALUES(@eid,@name,@address,@city,@gender,@dob,@phone,@salary,@designation,@photo,@sign)"
                Try
                    conn.Open()
                    With command
                        .CommandText = Query
                        .Connection = conn
                    .Parameters.AddWithValue("@eid", txteid.Text)
                    .Parameters.AddWithValue("@name", empname)
                        .Parameters.AddWithValue("@address", txtaddr.Text)
                        .Parameters.AddWithValue("@city", txtcity.Text)
                        .Parameters.AddWithValue("@gender", gender)
                        .Parameters.AddWithValue("@dob", txtdate.Text)
                        .Parameters.AddWithValue("@phone", txtphn.Text)
                    .Parameters.AddWithValue("@salary", txtamt.Text)
                        .Parameters.AddWithValue("@designation", txttype.Text)
                        .Parameters.AddWithValue("@photo", arrImage)
                        .Parameters.AddWithValue("@sign", arrImage1)
                        .ExecuteNonQuery()
                    End With
                    MsgBox("data saved")
                    conn.Close()
                    txtaddr.Text = ""
                    txtamt.Text = ""
                    txtcity.Text = ""
                    txtname1.Text = ""
                    txtname3.Text = ""
                    txtnname2.Text = ""
                    txtphn.Text = ""
                txtdate.Text = ""
                txttype.Text = ""
                    PictureBox1.Image = Nothing
                    PictureBox2.Image = Nothing

                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
        End If
    End Sub

    Private Sub Rmale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rmale.CheckedChanged
        gender = "male"
    End Sub

    Private Sub Rfemale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rfemale.CheckedChanged
        gender = "female"
    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Close()
        mainfrm.Show()
        txteid.Text = ""
        txtaddr.Text = ""
        txtamt.Text = ""
        txtcity.Text = ""
        txtdate.Text = ""
        txtname1.Text = ""
        txtname3.Text = ""
        txtnname2.Text = ""
        txtphn.Text = ""
        txttype.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        txteid.Text = ""
        txtaddr.Text = ""
        txtamt.Text = ""
        txtcity.Text = ""
        txtdate.Text = ""
        txtname1.Text = ""
        txtname3.Text = ""
        txtnname2.Text = ""
        txtphn.Text = ""
        txttype.Text = ""
    End Sub

    Private Sub employee_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub employee_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label5.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
       mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.employee"
        command = New MySqlCommand(query, mysqlconn)
        mysqlconn.Open()
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txteid.Text = "0000" + count
        mysqlconn.Close()
    End Sub

    Private Sub txteid_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txteid.GotFocus
       mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.employee"
        command = New MySqlCommand(query, mysqlconn)
        mysqlconn.Open()
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txteid.Text = "0000" + count
        mysqlconn.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'to save image
        Dim opf As New OpenFileDialog
        opf.Filter = "choose image(*.jpg;*.png)|*.jpg;*.png"
        If opf.ShowDialog = DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(opf.FileName)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'to save image
        Dim opf As New OpenFileDialog
        opf.Filter = "Choose Image(*.jpg;*.png)|*.jpg;*.png"
        If opf.ShowDialog = DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(opf.FileName)
        End If
    End Sub

    Private Sub txtphn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtphn.LostFocus
        If txtphn.Text.Length < 10 Then
            MsgBox("Enter minimun 10 digit phone number", MsgBoxStyle.Exclamation)
        End If
    End Sub
End Class