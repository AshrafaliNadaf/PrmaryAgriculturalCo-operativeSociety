'// Login Form
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Media
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Public Class Login
    Dim path = System.Windows.Forms.Application.StartupPath
    Dim LogOnsound As String
    Dim MyPlayer As New SoundPlayer()
    Public enc As System.Text.UTF8Encoding
    Private encryptor As ICryptoTransform
    Public decryptor As ICryptoTransform

    Dim sqlLink As MySqlConnection

    
    Private Sub Login_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim dialog As DialogResult
        dialog = MessageBox.Show("Do you really want to close?", "exit", MessageBoxButtons.YesNo)
        If dialog = DialogResult.No Then
            e.Cancel = True
        Else
            Application.ExitThread()

        End If
    End Sub

    Private Sub Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Timer1.Start()
       
        unusetext.Select() 'to focus on 3rd textbox
        Dim KEY_128 As Byte() = {42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23}
        Dim IV_128 As Byte() = {234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78}
        Dim symmetricKey As RijndaelManaged = New RijndaelManaged()
        symmetricKey.Mode = CipherMode.CBC

        Me.enc = New System.Text.UTF8Encoding
        Me.encryptor = symmetricKey.CreateEncryptor(KEY_128, IV_128)
        Me.decryptor = symmetricKey.CreateDecryptor(KEY_128, IV_128)
        LogOnsound = "\LogOn.wav"
        sqlLink = New MySqlConnection()
        sqlLink.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//Put your data
    End Sub


    Private Sub btnLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        Try
            sqlLink.Open()

            Dim sqlOrder As New MySqlCommand()
            sqlOrder.Connection = sqlLink

            Dim sPlainText As String = txtPassword.Text
            If Not String.IsNullOrEmpty(sPlainText) Then
                Dim memoryStream As MemoryStream = New MemoryStream()
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, Me.encryptor, CryptoStreamMode.Write)
                cryptoStream.Write(Me.enc.GetBytes(sPlainText), 0, sPlainText.Length)
                cryptoStream.FlushFinalBlock()
                Me.txtPassword.Text = Convert.ToBase64String(memoryStream.ToArray())
                memoryStream.Close()
                cryptoStream.Close()
            End If

            sqlOrder.CommandText = "SELECT * FROM login WHERE Username ='" & txtUsername.Text & "' AND Password ='" & txtPassword.Text & "' "


            Dim sqlReading As MySqlDataReader = sqlOrder.ExecuteReader()
            If (sqlReading.Read() = True) Then
                MyPlayer.SoundLocation = path & LogOnsound
                MyPlayer.Play()
                mainfrm.Show()
                Me.Hide()
            ElseIf txtUsername.Text = "" Then
                MsgBox("enter username")
                txtUsername.Focus()
            ElseIf txtPassword.Text = "" Then
                MsgBox("enter password")
                txtPassword.Focus()
            ElseIf txtUsername.Text = "" Or txtPassword.Text = "" Then
                Beep()
                txtPassword.Text = ""
                txtUsername.Text = ""
                MsgBox("Enter details !")

            Else
                Beep()
                
                MsgBox("Incorrect User Details", MsgBoxStyle.Exclamation, "Input Error")
                txtPassword.Text = ""
                txtUsername.Text = ""

            End If
            sqlReading.Close()
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            sqlLink.Close()

        End Try
    End Sub
    
    Private Sub Txtusername_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsername.Click
        txtUsername.Text = ""
        txtUsername.ForeColor = Color.Black
    End Sub

    Private Sub txtPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.Click
        txtPassword.Text = ""
        txtPassword.ForeColor = Color.Black
    End Sub

    Private Sub txtPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.GotFocus
        txtPassword.UseSystemPasswordChar = True
        txtPassword.Text = ""
        txtPassword.ForeColor = Color.Black
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        Forgot_password.Show()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged, CheckBox1.Click
        txtPassword.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub txtUsername_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsername.GotFocus
        txtUsername.Text = ""
        txtUsername.ForeColor = Color.Black
    End Sub

    Private Sub Label1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label1.MouseEnter
        Label1.ForeColor = Color.Black
    End Sub

    Private Sub Label1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label1.MouseLeave
        Label1.ForeColor = Color.White
    End Sub

    Private Sub pictureBox2_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox2.MouseEnter
        ToolTip1.SetToolTip(Me.PictureBox2, "Enter Password")
    End Sub

    Private Sub pictureBox1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseEnter
        ToolTip1.SetToolTip(Me.PictureBox1, "Enter your Username")
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Register.Show()
        Me.Hide()
    End Sub

    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Dim dialog As DialogResult
        dialog = MessageBox.Show("Do you really want to close?", "exit", MessageBoxButtons.YesNo)
        If dialog = DialogResult.No Then
            'e.cancel = True
        Else
            Application.ExitThread()

        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label2.Visible = Not Label2.Visible
        

    End Sub
End Class
