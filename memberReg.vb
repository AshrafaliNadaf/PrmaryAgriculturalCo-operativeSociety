'// Member Registration form
Imports MySql.Data.MySqlClient
Public Class mbrregfrm
    Dim gender As String
    Dim memname As String
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader
    Private Sub Rmale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rmale.CheckedChanged
        gender = "male"
    End Sub
    Private Sub Rfemale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rfemale.CheckedChanged
        gender = "female"
    End Sub
    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub Rother_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rother.CheckedChanged
        gender = "others"
    End Sub

    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click
       Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        If IsNothing(PictureBox1.Image) Or IsNothing(PictureBox2.Image) Then
            MsgBox("Please Provide images")
        ElseIf txtaddr.Text = "" Or txtadhar.Text = "" Or txtamt.Text = "" Or txtcity.Text = "" Or txtdate.Text = "" Or gender = "" Then
            MsgBox("All fields are mandatory", MsgBoxStyle.Information)
            Return
        ElseIf txtdate.Text.Length < 10 Then
            MsgBox("Enter valid date of birth ")
            txtdate.Focus()
        Else
            'conn.Open()
            Dim mstream As New System.IO.MemoryStream()
            Dim mstream1 As New System.IO.MemoryStream()
            PictureBox1.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg)
            PictureBox2.Image.Save(mstream1, System.Drawing.Imaging.ImageFormat.Jpeg)
            Dim arrImage() As Byte = mstream.GetBuffer()
            Dim arrImage1() As Byte = mstream1.GetBuffer()
            Dim Query As String
            Dim Query1 As String
            memname = txtname1.Text + " " + txtnname2.Text + " " + txtname3.Text
            mstream.Close()
            Query = "INSERT INTO pacs.member(mid,name,address,city,gender,dob,phone,amount,photo,sign)VALUES(@mid,@name,@address,@city,@gender,@dob,@phone,@amount,@photo,@sign)"
            Query1 = "insert into transaction("
            Try
                conn.Open()
                With command
                    .CommandText = Query
                    .Connection = conn
                    .Parameters.AddWithValue("@mid", txtadhar.Text)
                    .Parameters.AddWithValue("@name", memname)
                    .Parameters.AddWithValue("@address", txtaddr.Text)
                    .Parameters.AddWithValue("@city", txtcity.Text)
                    .Parameters.AddWithValue("@gender", gender)
                    .Parameters.AddWithValue("@dob", txtdate.Text)
                    .Parameters.AddWithValue("@phone", txtphn.Text)
                    .Parameters.AddWithValue("@amount", txtamt.Text)
                    .Parameters.AddWithValue("@photo", arrImage)
                    .Parameters.AddWithValue("@sign", arrImage1)
                    .ExecuteNonQuery()
                End With
                MsgBox("Member Created")
                conn.Close()
                txtaddr.Text = ""
                txtadhar.Text = ""
                txtamt.Text = "1050"
                txtcity.Text = ""
                txtname1.Text = ""
                txtname3.Text = ""
                txtnname2.Text = ""
                txtphn.Text = ""
                txtdate.Text = ""
                PictureBox1.Image = Nothing
                PictureBox2.Image = Nothing
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
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

    Private Sub mbrregfrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        mainfrm.Show()
        Me.Hide()
    End Sub

    Private Sub mbrregfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        txtamt.Text = "1050"
        Label12.Text = "@" & Login.txtUsername.Text
    End Sub

    Private Sub txtphn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtphn.LostFocus
        If txtphn.Text.Length < 10 Then
            MsgBox("Mobile Number should have 10 numbers", MsgBoxStyle.Exclamation)
            txtphn.Focus()
        End If
    End Sub

    Private Sub txtname3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtname3.LostFocus
        If txtname1.Text = "" Or txtnname2.Text = "" Or txtname3.Text = "" Then
            MsgBox("Please enter full name", MsgBoxStyle.Exclamation)
            txtname1.Focus()
        End If
    End Sub

    Private Sub txtadhar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtadhar.LostFocus
        Dim mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "server=localhost;userid=root;database=pacs"
        mysqlconn.Open()
        If Not IsNumeric(txtadhar.Text) Then
            MsgBox("enter Adhar number correctly", MsgBoxStyle.Information, "Input error")
            txtadhar.Clear()
            Return
        ElseIf txtadhar.Text.Length < 12 Then
            MsgBox("Adhar number shoud have minimum 12 numbers", MsgBoxStyle.Information, "Input error")
            txtadhar.Focus()
        Else
            Try
               
                Dim Query As String
                Query = "select mid from pacs.member where member.mid='" & txtadhar.Text & "'"
                command = New MySqlCommand(Query, mysqlconn)
                reader = command.ExecuteReader
                If reader.HasRows Then
                    MsgBox("Member already exist with this adhar", MsgBoxStyle.Exclamation)
                    txtadhar.Focus()
                    mysqlconn.Close()
                    reader.Close()
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
End Class