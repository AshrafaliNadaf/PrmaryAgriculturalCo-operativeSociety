'// Register Form
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Public Class Register
    Dim sqlLink As MySqlConnection
    Public enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Public decryptor As ICryptoTransform
    Dim type As String

    Private Sub Register_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Login.Show()
        Me.Hide()
    End Sub

    Private Sub Register_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        sqlLink = New MySqlConnection()
        sqlLink.ConnectionString = "server=localhost;User ID=root;Password=;database=pacs"  '//Put your data
        Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
        Dim symmetricKey As RijndaelManaged = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC

        Me.enc = New System.Text.UTF8Encoding
        Me.encryptor = symmetricKey.CreateEncryptor(KEY_128, IV_128)
        Me.decryptor = symmetricKey.CreateDecryptor(KEY_128, IV_128)
    End Sub

    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click

        'Dim myString As String = txtPassword.Text

        If txtUsername.Text = "" Then
            MsgBox("You did not enter a Username !")
            Return

        ElseIf txtPassword.Text = "" Then
            MsgBox("You did not enter a Password !")
            Return

            'ElseIf myString.Length < 6 Then

            '    MsgBox("too small")

            '    ErrorProvider1.SetError(txtPassword, "Password is so small")
            '    txtPassword.Text = ""
            '    txtRePassword.Text = ""
            '    txtPassword.Focus()
            '    Return

            'ElseIf txtPassword.Text <> txtRePassword.Text Then
            '    MsgBox("Passwords do not match !")
            Return
            'ElseIf Regex.IsMatch(myString, "^(.*[0-9]+.*[a-z]+.*[A-Z]+.*)|(.*[0-9]+.*[A-Z]+.*[a-z]+.*)|(.*[a-z]+.*[0-9]+.*[A-Z]+.*)|(.*[a-z]+.*[A-Z]+.*[0-9]+.*)|(.*[A-Z]+.*[a-z]+.*[0-9]+.*)|(.*[A-Z]+.*[0-9]+.*[a-z]+.*)$") Then
            '    'MsgBox("correct")
            'Else

            '    MsgBox("Password should have at least one capital letter, one small letter and any one of !@#$% characters")
            '    ErrorProvider1.SetError(txtPassword, "Password should have at least one capital letter, one small letter and any one of !@#$% characters")
            '    Return
        End If

        Try
            sqlLink.Open()

            'encrypt data
            Dim sPlainText As String = Me.txtPassword.Text

            If Not String.IsNullOrEmpty(sPlainText) Then
                Dim memoryStream As MemoryStream = New MemoryStream()
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.encryptor, CryptoStreamMode.Write)
                cryptoStream.Write(Me.enc.GetBytes(sPlainText), 0, sPlainText.Length)
                cryptoStream.FlushFinalBlock()
                Me.txtPassword.Text = Convert.ToBase64String(memoryStream.ToArray())
                memoryStream.Close()
                cryptoStream.Close()
            End If

            'save data
            Dim sqlOrder As New MySqlCommand()
            sqlOrder.Connection = sqlLink
            sqlOrder.CommandText = "INSERT INTO login(Username,Password) " &
                                   "VALUES('" & txtUsername.Text & "','" & txtPassword.Text & "')"
            sqlOrder.ExecuteNonQuery()
            MsgBox("You have successfully registered, you can log in now !")
            txtPassword.Text = ""
            txtRePassword.Text = ""
            txtkey.Text = ""
            txtUsername.Text = ""
            Me.Close()
            Login.Show()
        Catch EX As Exception
            MsgBox("An error occurred: " & EX.Message)
        Finally
            sqlLink.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Login.Show()
        Me.Hide()
        txtkey.Text = ""
        'txtUsername.Visible = False
        'txtPassword.Visible = False
        'txtRePassword.Visible = False
        'lbluser.Visible = False
        'lblpswd.Visible = False
        'lblrpswd.Visible = False
        'lbltype.Visible = False
        'RadioButton1.Visible = False
        'RadioButton2.Visible = False
        'btnCancel.Visible = False
        'btnRegister.Visible = False

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' type = "admin"
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' type = "guest"
    End Sub

    Private Sub btnenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnenter.Click
        If txtkey.Text = "" Then
            MsgBox("enter key")
            txtkey.Focus()
            txtPassword.Enabled = False
            txtRePassword.Enabled = False
            btnCancel.Enabled = True
            btnRegister.Enabled = False
            txtUsername.Enabled = False
        ElseIf Not IsNumeric(txtkey.Text) Then
            MsgBox("enter serial key correctly", MsgBoxStyle.Information)
            txtkey.Focus()
            txtPassword.Enabled = False
            txtRePassword.Enabled = False
            btnCancel.Enabled = True
            btnRegister.Enabled = False
            txtUsername.Enabled = False
        ElseIf txtkey.Text = 123456789 Then
            txtPassword.Enabled = True
            txtRePassword.Enabled = True
            lblpswd.Visible = True
            lblrpswd.Visible = True
            btnCancel.Enabled = True
            btnRegister.Enabled = True
            lbluser.Visible = True
            txtUsername.Enabled = True
            Return
        Else
            MsgBox("enter valid key")
            txtkey.Text = ""
            txtkey.Focus()
            txtPassword.Enabled = False
            txtRePassword.Enabled = False
            btnCancel.Enabled = True
            btnRegister.Enabled = False
            txtUsername.Enabled = False
        End If
    End Sub

    Private Sub txtPassword_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.LostFocus
        Dim myString As String = txtPassword.Text
        If Regex.IsMatch(myString, "^(.*[0-9]+.*[a-z]+.*[A-Z]+.*)|(.*[0-9]+.*[A-Z]+.*[a-z]+.*)|(.*[a-z]+.*[0-9]+.*[A-Z]+.*)|(.*[a-z]+.*[A-Z]+.*[0-9]+.*)|(.*[A-Z]+.*[a-z]+.*[0-9]+.*)|(.*[A-Z]+.*[0-9]+.*[a-z]+.*)$") Then
            txtRePassword.Focus()
            ErrorProvider1.Clear()
        ElseIf txtPassword.Text.Length < 6 Then
            ErrorProvider1.SetError(txtPassword, "Password should have at least 6 characters")
            txtPassword.Focus()
        Else
            ErrorProvider1.SetError(txtPassword, "Password should have at least one capital letter, one small letter and any one of !@#$% characters")
            txtPassword.Focus()
            End If
    End Sub

    Private Sub txtRePassword_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRePassword.LostFocus
        If txtPassword.Text <> txtRePassword.Text Then
            ErrorProvider1.SetError(txtRePassword, "Password do not match")
            txtRePassword.Focus()
        Else
            ErrorProvider1.Clear()
        End If
    End Sub


    
    Private Sub txtRePassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRePassword.TextChanged

    End Sub
End Class