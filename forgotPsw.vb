'// Forgot password form
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Media
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Public Class Forgot_password
    Dim conn As MySqlConnection
    Dim Command As MySqlCommand
    Dim dbDataSet As New DataTable
    Dim reader As MySqlDataReader
    Dim MyPlayer As New SoundPlayer()
    Public enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Public decryptor As ICryptoTransform


    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Hide()
        txtkey.Text = ""
        txtuser.Text = ""
        mainfrm.Close()
        Login.Show()
    End Sub

    Private Sub btnok2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnok3.Click

        Dim myString As String = txtpswd.Text
        If txtpswd.Text = "" Then
            MsgBox("You did not enter a Password !")
            Return

        ElseIf myString.Length < 6 Then

            MsgBox("too small")

            ErrorProvider1.SetError(txtpswd, "Password is so small")
            txtpswd.Text = ""
            txtrepswd.Text = ""
            txtpswd.Focus()
            Return

        ElseIf txtpswd.Text <> txtrepswd.Text Then
            MsgBox("Passwords do not match !")
            Return
        ElseIf Regex.IsMatch(myString, "^(.*[0-9]+.*[a-z]+.*[A-Z]+.*)|(.*[0-9]+.*[A-Z]+.*[a-z]+.*)|(.*[a-z]+.*[0-9]+.*[A-Z]+.*)|(.*[a-z]+.*[A-Z]+.*[0-9]+.*)|(.*[A-Z]+.*[a-z]+.*[0-9]+.*)|(.*[A-Z]+.*[0-9]+.*[a-z]+.*)$") Then
            'MsgBox("correct")
        Else

            MsgBox("Password should have at least one capital letter, one small letter and any one of !@#$% characters")
            ErrorProvider1.SetError(txtpswd, "Password should have at least one capital letter, one small letter and any one of !@#$% characters")
            Return
        End If

        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        'Dim reader As MySqlDataReader

        Try
            'for encrypting password
            Dim sPlainText As String = Me.txtpswd.Text

            If Not String.IsNullOrEmpty(sPlainText) Then
                Dim memoryStream As MemoryStream = New MemoryStream()
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.encryptor, CryptoStreamMode.Write)
                cryptoStream.Write(Me.enc.GetBytes(sPlainText), 0, sPlainText.Length)
                cryptoStream.FlushFinalBlock()
                Me.txtpswd.Text = Convert.ToBase64String(memoryStream.ToArray())
                memoryStream.Close()
                cryptoStream.Close()
            End If

            'to update
            conn.Open()
            Dim sqlOrder As New MySqlCommand()
            sqlOrder.Connection = conn
            sqlOrder.CommandText = "update pacs.login set Password='" & txtpswd.Text & "' where username='" & txtuser.Text & "'"
            sqlOrder.ExecuteNonQuery()
            txtkey.Text = ""
            txtpswd.Text = ""
            txtrepswd.Text = ""
            txtuser.Text = ""
            MsgBox("data updated")
            conn.Close()
            Me.Close()
            mainfrm.Close()
            Login.Show()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnok2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnok2.Click
        If txtkey.Text = "" Then
            MsgBox("enter key")
            txtkey.Focus()
            txtpswd.Enabled = False
            txtrepswd.Enabled = False
            btnok3.Enabled = False
        ElseIf Not IsNumeric(txtkey.Text) Then
            MsgBox("enter proper key", MsgBoxStyle.Information)
            txtpswd.Enabled = False
            txtrepswd.Enabled = False
            btnok3.Enabled = False
        ElseIf txtkey.Text = 123456789 Then
            txtpswd.Enabled = True
            txtrepswd.Enabled = True
            Label4.Visible = True
            Label5.Visible = True
            btnok3.Enabled = True
            btncancel.Visible = True
            Return
        Else
            MsgBox("enter valid key")
            txtkey.Text = ""
            txtkey.Focus()
            txtpswd.Enabled = False
            txtrepswd.Enabled = False
            btnok3.Enabled = False

        End If
    End Sub

    Private Function source1() As Object
        Throw New NotImplementedException
    End Function

    Private Sub Forgot_password_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Login.Show()
        Me.Hide()
    End Sub


    Private Sub Forgot_password_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        'asci values for encryption
        Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
        Dim symmetricKey As RijndaelManaged = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC

        Me.enc = New System.Text.UTF8Encoding
        Me.encryptor = symmetricKey.CreateEncryptor(KEY_128, IV_128)
        Me.decryptor = symmetricKey.CreateDecryptor(KEY_128, IV_128)
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"

    End Sub

    Private Sub txtpswd_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtpswd.TextChanged
        txtpswd.UseSystemPasswordChar = True
    End Sub

    Private Sub txtrepswd_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtrepswd.TextChanged
        txtrepswd.UseSystemPasswordChar = True
    End Sub

    Private Sub txtuser_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtuser.LostFocus
        'code for validating user
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        conn.Open()

        Command = New MySqlCommand("select username from login where username = '" & txtuser.Text & "'", conn)
        reader = Command.ExecuteReader

        If reader.HasRows Then
            reader.Read()
            If txtuser.Text = "" Then
                MsgBox("enter username")
            ElseIf txtuser.Text = reader.Item("username").ToString Then
                'txtkey.Enabled = True
                btnok2.Enabled = True
                txtkey.Focus()
            End If
        Else
            MsgBox("enter valid username")
            txtuser.Text = ""
            txtuser.Focus()
        End If
        reader.Close()
        conn.Close()
    End Sub

    Private Sub txtkey_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtkey.GotFocus
        If txtuser.Text = "" Then
            MsgBox("enter username")
        End If
    End Sub
End Class