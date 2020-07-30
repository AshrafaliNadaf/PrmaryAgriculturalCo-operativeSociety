Imports MySql.Data.MySqlClient
Public Class savingView
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Private Sub load_table()
      
    End Sub

    Private Sub savingView_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub savingView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label12.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        txtdate.Text = todaysdate
    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        txtacno.Text = ""
        txtbalance.Text = ""
        txtname.Text = ""
        txtamt.Text = ""
        lblacno.Text = ""
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        dbdataset.Clear()
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Close()
        mainfrm.Show()
        txtacno.Text = ""
        txtbalance.Text = ""
        txtname.Text = ""
        txtamt.Text = ""
        lblacno.Text = ""
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing
        dbdataset.Clear()
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
    End Sub

    Private Sub btngo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngo.Click
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection

        Dim reader As MySqlDataReader

        Try
            mysqlconn.Open()
            Dim query As String
            query = "select sacno,name,samt,odate from pacs.savings,pacs.member where savings.mid=member.mid and savings.sacno=' " & txtacno.Text & " '"
            command = New MySqlCommand(query, mysqlconn)
            reader = command.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    txtacno.Text = reader.GetUInt64("sacno")
                    txtname.Text = reader.GetString("name")
                    txtbalance.Text = reader.GetDouble("samt")
                End While
                reader.Close()
                Dim sda As New MySqlDataAdapter
                Dim bSource As New BindingSource
                'mysqlconn.Open()
                Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Balance' from transaction where sacno='" & txtacno.Text & "' "
                Dim adpt As New MySqlDataAdapter(q, mysqlconn)
                Dim ds As New DataSet()
                adpt.Fill(ds, "transaction")
                DataGridView1.DataSource = ds.Tables(0)
                mysqlconn.Close()
                'to feth image
                Dim adapter As New MySqlDataAdapter
                adapter.SelectCommand = command
                Dim data As DataTable
                Dim commandbuild As MySqlCommandBuilder
                data = New DataTable
                mysqlconn = New MySqlConnection
                mysqlconn.ConnectionString = "server=localhost;userid=root;database=pacs"
                mysqlconn.Open()
                adapter = New MySqlDataAdapter("select photo,sign from pacs.member,pacs.savings where member.mid = savings.mid and sacno='" & txtacno.Text & "'", mysqlconn)

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
                txtbalance.Text = ""
                txtname.Text = ""
                txtamt.Text = ""
                lblacno.Text = ""
            End If
            mysqlconn.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        lblacno.Text = txtacno.Text
    End Sub

    Private Sub btndpst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndpst.Click
        Dim trn As Integer
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        Dim a As Integer = 100
        Dim reader As MySqlDataReader
        If txtamt.Text = "" Then
            MsgBox("Enter Valid Amount", MsgBoxStyle.Information)
            txtamt.Focus()
        ElseIf txtamt.Text < a Then
            MsgBox("Minimum Deposit is 100 Rs", MsgBoxStyle.Information)
        Else
            Try
                If txtacno.Text = "" Then
                    MsgBox("NO DATA FOUND!!!")
                    Return
                Else
                    mysqlconn.Open()

                    Dim query1 As String
                    Dim query2 As String
                    Dim acc As String = "Self Deposit"
                    query1 = "update pacs.savings set samt=samt+' " & txtamt.Text & " ' where sacno=' " & txtacno.Text & " '"
                    query2 = "INSERT INTO pacs.transaction(sacno,particulars,tdate,amt,credit)values('" & txtacno.Text & "','" & acc & "','" & txtdate.Text & "','" & txtbalance.Text & "','" & txtamt.Text & "')"
                    Dim query = String.Concat(query1, ";", query2)
                    command = New MySqlCommand(query, mysqlconn)
                    reader = command.ExecuteReader
                    MsgBox("Amount Deposited Successfully")
                    trn = command.LastInsertedId
                    txtbalance.Text = Val(txtbalance.Text) + Val(txtamt.Text)
                    While reader.Read
                        txtbalance.Text = reader.GetUInt64("samt")
                    End While
                    reader.Close()
                    mysqlconn.Close()

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Try
                mysqlconn.Open()
                Dim query1 As String

                query1 = "update pacs.transaction set transaction.amt=amt+'" & txtamt.Text & "' where tno='" & trn & "';"
                command = New MySqlCommand(query1, mysqlconn)
                reader = command.ExecuteReader
                mysqlconn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from transaction where sacno='" & txtacno.Text & "' "
        Dim adpt As New MySqlDataAdapter(q, mysqlconn)
        Dim ds As New DataSet()
        adpt.Fill(ds, "transaction")
        DataGridView1.DataSource = ds.Tables(0)
        mysqlconn.Close()
    End Sub

    Private Sub btnwtdrw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnwtdrw.Click
        Dim trn As Integer
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        Dim a As Integer = 100
        Dim reader As MySqlDataReader
        If txtamt.Text = "" Then
            MsgBox("Enter Valid Amount", MsgBoxStyle.Information)
            txtamt.Focus()
        ElseIf txtamt.Text < a Then
            MsgBox("Minimum Deposit is 100 Rs", MsgBoxStyle.Information)
        ElseIf Val(txtamt.Text) > Val(txtbalance.Text) Then
            MsgBox("Insufficient Balance", MsgBoxStyle.Information)
        Else
            Try
                If txtacno.Text = "" Then
                    MsgBox("NO DATA FOUND!!!")
                    Return
                Else
                    mysqlconn.Open()

                    Dim query1 As String
                    Dim query2 As String
                    Dim acc As String = "Self Withdraw"
                    query1 = "update pacs.savings set samt=samt-' " & txtamt.Text & " ' where sacno=' " & txtacno.Text & " '"
                    query2 = "INSERT INTO pacs.transaction(sacno,particulars,tdate,amt,debit)values('" & txtacno.Text & "','" & acc & "','" & txtdate.Text & "','" & txtbalance.Text & "','" & txtamt.Text & "')"
                    Dim query = String.Concat(query1, ";", query2)
                    command = New MySqlCommand(query, mysqlconn)
                    reader = command.ExecuteReader
                    MsgBox("Amount Withdrawn Successfully")
                    trn = command.LastInsertedId
                    txtbalance.Text = Val(txtbalance.Text) - Val(txtamt.Text)
                    While reader.Read
                        txtbalance.Text = reader.GetUInt64("samt")
                    End While
                    reader.Close()
                    mysqlconn.Close()

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Try
                mysqlconn.Open()
                Dim query1 As String

                query1 = "update pacs.transaction set transaction.amt=amt-'" & txtamt.Text & "' where tno='" & trn & "';"
                command = New MySqlCommand(query1, mysqlconn)
                reader = command.ExecuteReader
                mysqlconn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from transaction where sacno='" & txtacno.Text & "' "
        Dim adpt As New MySqlDataAdapter(q, mysqlconn)
        Dim ds As New DataSet()
        adpt.Fill(ds, "transaction")
        DataGridView1.DataSource = ds.Tables(0)
        mysqlconn.Close()
    End Sub

    Private Sub btnprint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprint.Click
        PrintPreviewDialog1.Document = Me.PrintDocument1

        Dim ButtonPressed As DialogResult = PrintPreviewDialog1.ShowDialog()
        If (ButtonPressed = DialogResult.OK) Then
            PrintDocument1.Print()
            Me.Close()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.Panel2.Width, Me.Panel2.Height)

        Panel2.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel2.Width, Me.Panel2.Height))

        e.Graphics.DrawImage(bm, 50, 60)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub
End Class