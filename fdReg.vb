'// FD opening a/c
Imports MySql.Data.MySqlClient
Public Class fdReg
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader
    Private Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave.Click
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        mysqlconn.Open()
    
        If Not IsNumeric(txtamt.Text) Then
            MsgBox("Enter amount in numbers only")
            txtamt.Clear()
            txtamt.Focus()
            Return
        ElseIf cmbYrs.Text = "" Then
            MsgBox("please select duration year!!")
            txtamt.Clear()
            Return
        ElseIf Not IsNumeric(txtadhar.Text) Then
            MsgBox("Enter Adhar number correctly")
            txtadhar.Clear()
            txtadhar.Focus()
            Return
        Else
            ' reader.Close()
            Try
                Dim query As String
                Dim query1 As String
                Dim add As String = "FD Added"
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
                query = "INSERT INTO pacs.fd(mid,facno,odate,famt,duration)values('" & txtadhar.Text & "','" & txtacno.Text & "','" & txtdate.Text & "','" & txtamt.Text & "','" & cmbYrs.Text & "')"
                query1 = "INSERT INTO pacs.transaction(facno,particulars,tdate,amt,credit)values('" & txtacno.Text & "','" & add & "','" & txtdate.Text & "','" & txtamt.Text & "','" & txtamt.Text & "')"
                Dim q = String.Concat(query, ";", query1)
                command = New MySqlCommand(q, mysqlconn)
                reader = command.ExecuteReader
                MessageBox.Show("Account Created Successfully")
                txtamt.Text = ""
                txtadhar.Text = ""
                cmbYrs.Text = ""
                mysqlconn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub fdReg_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        mainfrm.Show()
        Me.Hide()
    End Sub

    Private Sub fdReg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label5.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        'code for auto increment
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.fd"
        command = New MySqlCommand(query, mysqlconn)
        mysqlconn.Open()
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txtacno.Text = "20000" + count
        mysqlconn.Close()
        'code for system date
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        txtdate.Text = todaysdate
    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Hide()
        mainfrm.Show()
        txtamt.Text = ""
        txtadhar.Text = ""
    End Sub

    Private Sub txtacno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtacno.GotFocus
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.fd"
        command = New MySqlCommand(query, mysqlconn)
        mysqlconn.Open()
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txtacno.Text = "20000" + count
        mysqlconn.Close()
        'code for system date
        'Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        'txtdate.Text = todaysdate
    End Sub

End Class