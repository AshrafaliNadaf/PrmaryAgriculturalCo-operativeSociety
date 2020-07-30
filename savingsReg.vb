'// savings opening a/c
Imports MySql.Data.MySqlClient
Public Class savingsReg
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader
    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click
        'code for validate and insert
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        mysqlconn.Open()

        If Not IsNumeric(txtamt.Text) Then
            MsgBox("enter amount in numbers only")
            txtamt.Clear()
            txtamt.Focus()
            Return
        ElseIf Not IsNumeric(txtadhar.Text) Then
            MsgBox("enter Adhar number correctly")
            txtadhar.Clear()
            txtadhar.Focus()
            Return
        Else
            Try

                Dim acc As String = "SB Account Created"
                Dim query1 As String
                Dim query2 As String
                query1 = "INSERT INTO pacs.savings(sacno,mid,odate,samt)values('" & txtacno.Text & "','" & txtadhar.Text & "','" & txtdate.Text & "','" & Val(txtamt.Text) & "')"
                query2 = "INSERT INTO pacs.transaction(sacno,particulars,tdate,amt,credit)values('" & txtacno.Text & "','" & acc & "','" & txtdate.Text & "','" & Val(txtamt.Text) & "','" & Val(txtamt.Text) & "')"
                reader.Close()
                Dim query = String.Concat(query1, ";", query2)
                command = New MySqlCommand(query, mysqlconn)
                reader = command.ExecuteReader
                MessageBox.Show("Account Created Successfully")
                mysqlconn.Close()
                txtadhar.Text = ""
                txtamt.Text = ""
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub savingsReg_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        mainfrm.Show()
        Me.Hide()
    End Sub

    Private Sub savings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label5.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        'code for auto increment
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        mysqlconn.Open()
            Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.savings"
            command = New MySqlCommand(query, mysqlconn)
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txtacno.Text = "10000" + count
        mysqlconn.Close()
            ' code for system date
            Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
            txtdate.Text = todaysdate
    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        mainfrm.Show()
        Me.Hide()
    End Sub

    Private Sub txtacno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtacno.GotFocus
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        mysqlconn.Open()
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.savings"
        command = New MySqlCommand(query, mysqlconn)
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txtacno.Text = "10000" + count
        mysqlconn.Close()
    End Sub

    Private Sub txtadhar_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtadhar.LostFocus

        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        mysqlconn.Open()
        command = New MySqlCommand("select mid from savings where mid = '" & txtadhar.Text & "'", mysqlconn)
        reader = command.ExecuteReader  
        If txtadhar.Text.Length < 12 Then
            MsgBox("Enter valid Adhar number")
        ElseIf reader.HasRows Then
            MsgBox(" Savings account already existed with this Adhar, please try another!!!")
            txtadhar.Text = ""
        End If
    End Sub
End Class