Imports MySql.Data.MySqlClient
Public Class loan
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader
    Private Sub loan_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub loan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label12.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        'Code for auto increment
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        Dim query As String
        Dim count As Integer
        mysqlconn.Open()
        query = "select count(*) from pacs.loan"
        Command = New MySqlCommand(query, mysqlconn)
        count = Convert.ToUInt64(Command.ExecuteScalar()) + 1
        txtloan.Text = "30000" + count
        mysqlconn.Close()
        ' code for system date
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        txtodate.Text = todaysdate
    End Sub

    Private Sub txtloan_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtloan.GotFocus, txtrate.GotFocus
        'code for auto increment
        mysqlconn = New MySqlConnection
        mysqlconn.ConnectionString = "Server=localhost; User Id=root ; Database=pacs"
        mysqlconn.Open()
        Dim query As String
        Dim count As Integer
        query = "select count(*) from pacs.loan"
        command = New MySqlCommand(query, mysqlconn)
        count = Convert.ToUInt64(command.ExecuteScalar()) + 1
        txtloan.Text = "30000" + count
        mysqlconn.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        mainfrm.Show()
        Me.Hide()
        txtamt.Text = ""
        txtmid.Text = ""
        txtmonth.Text = ""
        cmbtype.Text = "select loan type"
        txtemi.Text = ""
        txtintamt.Text = ""
        txtrate.Text = ""
        txttotamt.Text = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'code for validate and insert
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        mysqlconn.Open()
        command = New MySqlCommand("select mid from loan where mid = '" & txtmid.Text & "'and ltype='" & cmbtype.Text & "'", mysqlconn)
        reader = command.ExecuteReader
        If txttotamt.Text = "" Or txtemi.Text = "" Or txtintamt.Text = "" Or txtmid.Text = "" Or txtmonth.Text = "" Or txtloan.Text = "" Or txtodate.Text = "" Then
            MsgBox("All fields are mandatory")

        ElseIf reader.HasRows Then
            MsgBox(" loan issued already with this Adhar, please try another!!!")
            txtmid.Text = ""
            txtmid.Focus()
        ElseIf Not IsNumeric(txtamt.Text) Then
            MsgBox("enter amount in numbers only")
            txtamt.Clear()
            Return
        Else
            Try
                Dim query As String
                Dim query1 As String
                Dim l As String = "Loan Issued"
                query = "INSERT INTO loan(loanno,mid,odate,ltype,lamt,duration,dueamt,emi)values('" & txtloan.Text & "','" & txtmid.Text & "','" & txtodate.Text & "','" & cmbtype.Text & "','" & Val(txtamt.Text) & "','" & txtmonth.Text & "','" & Val(txtamt.Text) & "','" & Val(txtemi.Text) & "')"
                query1 = "insert into transaction(loanno,particulars,amt,tdate,debit)values('" & txtloan.Text & "','" & l & "','" & Val(txtamt.Text) & "','" & txtodate.Text & "','" & Val(txtamt.Text) & "')"
                reader.Close()
                Dim q = String.Concat(query, ";", query1)
                command = New MySqlCommand(q, mysqlconn)
                reader = command.ExecuteReader
                MessageBox.Show("Loan Account Created Successfully")
                mysqlconn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim emi As Double
        Dim p As Integer = Val(txtamt.Text)
        Dim r As Double = (Val(txtrate.Text) / 12 / 100)
        Dim n As Integer = Val(txtmonth.Text)
        emi = (p * r) * (((1 + r) ^ n) / (((1 + r) ^ n) - 1))
        txtemi.Text = emi
        txttotamt.Text = emi * n
        txtintamt.Text = Val(txttotamt.Text) - Val(txtamt.Text)
    End Sub

    Private Sub cmbtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbtype.SelectedIndexChanged

        If cmbtype.SelectedItem = "Agriculture Loan" Then
            txtrate.Text = "0.0"
        ElseIf cmbtype.SelectedItem = "Home Loan" Then
            txtrate.Text = "10.5"
        ElseIf cmbtype.SelectedItem = "Vehicle Loan" Then
            txtrate.Text = "12.5"
        Else
            txtrate.Text = "15.5"
        End If
    End Sub
    Private Sub txtmid_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmid.LostFocus
        If Not IsNumeric(txtmid.Text) Then
            MsgBox("enter Adhar number correctly")
            txtmid.Clear()
            Return
        ElseIf txtmid.Text.Length < 12 Then
            MsgBox("Enter valid adhar number")
        End If
    End Sub

    Private Sub txtmonth_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmonth.LostFocus
        Dim emi As Double
        Dim p As Integer = Val(txtamt.Text)
        Dim r As Double = (Val(txtrate.Text) / 12 / 100)
        Dim n As Integer = Val(txtmonth.Text)
        emi = (p * r) * (((1 + r) ^ n) / (((1 + r) ^ n) - 1))
        txtemi.Text = emi
        txttotamt.Text = emi * n
        txtintamt.Text = Val(txttotamt.Text) - Val(txtamt.Text)
    End Sub

    Private Sub btnclr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclr.Click
        txtamt.Text = ""
        txtmid.Text = ""
        txtmonth.Text = ""
        cmbtype.Text = "select loan type"
        txtemi.Text = ""
        txtintamt.Text = ""
        txtrate.Text = ""
        txttotamt.Text = ""
    End Sub
End Class