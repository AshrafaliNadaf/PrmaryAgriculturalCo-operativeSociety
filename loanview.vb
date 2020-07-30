Imports MySql.Data.MySqlClient
Public Class loanview

    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Dim reader As MySqlDataReader
    Private Sub btngo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngo.Click
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection

        Dim reader As MySqlDataReader
       
            Try
                mysqlconn.Open()
                Dim query As String
                query = "select loanno,name,lamt,dueamt,emi,odate,ltype,duration from pacs.loan,pacs.member where loan.loanno=' " & txtloanno.Text & " ' and  loan.mid=member.mid "
                command = New MySqlCommand(query, mysqlconn)
                reader = command.ExecuteReader
                If reader.HasRows Then
                    While reader.Read
                        txtloanno.Text = reader.GetUInt64("loanno")
                        txtname.Text = reader.GetString("name")
                        txtlamt.Text = reader.GetDouble("lamt")
                        txtdueamt.Text = reader.GetDouble("dueamt")
                        txtemi.Text = reader.GetString("emi")
                        txtOdate.Text = reader.GetString("odate")
                        txttype.Text = reader.GetString("ltype")
                        txttime.Text = reader.GetString("duration")
                    End While
                    reader.Close()
                    Dim sda As New MySqlDataAdapter
                    Dim bSource As New BindingSource
                    'mysqlconn.Open()
                    Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from transaction where loanno='" & txtloanno.Text & "' "
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
                adapter = New MySqlDataAdapter("select photo from pacs.member,pacs.loan where member.mid = loan.mid and loanno='" & txtloanno.Text & "'", mysqlconn)

                commandbuild = New MySqlCommandBuilder(adapter)
                adapter.Fill(data)

                Dim lb() As Byte = data.Rows(0).Item("photo")
                Dim lstr As New System.IO.MemoryStream(lb)
                PictureBox1.Image = Image.FromStream(lstr)
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
                lstr.Close()


                Else : MsgBox("NO DATA FOUND!!!")
                    txtlamt.Text = ""
                    txtname.Text = ""
                    txtdueamt.Text = ""
                    txtloanno.Text = ""
                    txttype.Text = ""
                    txtInterest.Text = ""
                    txtApinterest.Text = ""
                    txtOdate.Text = ""
                    txttime.Text = ""
                txtTotal.Text = ""
                PictureBox1.Image = Nothing
                txtemi.Text = ""
                lbllno.Text = ""
                    DataGridView1.DataSource = Nothing
                    DataGridView1.Refresh()
                End If
                mysqlconn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        If txttype.Text = "Agriculture Loan" Then
            txtInterest.Text = "0.0"
        ElseIf txttype.Text = "Home Loan" Then
            txtInterest.Text = "10.5"
        ElseIf txttype.Text = "Vehicle Loan" Then
            txtInterest.Text = "12.5"
        ElseIf txttype.Text = "other" Then
            txtInterest.Text = "15.5"
        End If

        Dim emi As Double
        Dim p As Integer = Val(txtlamt.Text)
        Dim r As Double = (Val(txtInterest.Text) / 12 / 100)
        Dim n As Integer = Val(txttime.Text)
        emi = (p * r) * (((1 + r) ^ n) / (((1 + r) ^ n) - 1))
        txtemi.Text = emi
        txtTotal.Text = emi * n
        txtApinterest.Text = Val(txtTotal.Text) - Val(txtlamt.Text)
        lbllno.Text = txtloanno.Text
    End Sub

    Private Sub loanview_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub loanview_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.WindowState = FormWindowState.Maximized
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        txtdate.Text = todaysdate
        Label18.Text = "@" & Login.txtUsername.Text

    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        txtloanno.Text = ""
        txtdueamt.Text = ""
        txtname.Text = ""
        txttype.Text = ""
        txtInterest.Text = ""
        txtApinterest.Text = ""
        txtOdate.Text = ""
        txttime.Text = ""
        txtTotal.Text = ""
        txtlamt.Text = ""
        txtemi.Text = ""
        PictureBox1.Image = Nothing
        lbllno.Text = ""
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
    End Sub

    Private Sub btnwtdrw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Close()
        mainfrm.Show()
    End Sub

    Private Sub btndpst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndpst.Click
        Dim trn As Integer
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection

        Dim reader As MySqlDataReader
        Try
            If txtloanno.Text = "" Then
                MsgBox("NO DATA FOUND!!!")
                Return
            ElseIf txtemi.Text = "" Or txtemi.Text = "0" Then
                MsgBox("Enter valid amount")
            ElseIf txtdueamt.Text = "0" Or txtdueamt.Text < 0 Then
                MsgBox("All Loan amount Paid already ", MsgBoxStyle.Information)
            Else

                mysqlconn.Open()
                Dim query As String
                Dim query1 As String
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
                Dim l As String = "EMI Paid"
                query = "update pacs.loan set dueamt=dueamt - ' " & txtemi.Text & " ' where loanno=' " & txtloanno.Text & " '"
                query1 = "insert into transaction(loanno,particulars,amt,tdate,credit)values('" & txtloanno.Text & "','" & l & "','" & txtdueamt.Text & "','" & todaysdate & "','" & txtemi.Text & "')"
                Dim q = String.Concat(query, ";", query1)
                command = New MySqlCommand(q, mysqlconn)
                reader = command.ExecuteReader
                MsgBox("EMI Paid Successfully")
                trn = command.LastInsertedId
                txtdueamt.Text = Val(txtdueamt.Text) - Val(txtemi.Text)
                While reader.Read
                    txtdueamt.Text = reader.GetUInt64("dueamt")
                End While
                mysqlconn.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Try
            mysqlconn.Open()
            Dim query As String
            query = "update transaction set amt=amt-'" & txtemi.Text & "' where tno='" & trn & "'"
            command = New MySqlCommand(query, mysqlconn)
            reader = command.ExecuteReader
        Catch ex As Exception
            MsgBox(ex.Message)
            mysqlconn.Close()
        End Try


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        conn.Open()
        Dim query As String
        query = "select *from pacs.loan where loan.loanno=' " & txtloanno.Text & " '"
        command = New MySqlCommand(query, conn)
        reader = command.ExecuteReader
        If reader.HasRows Then
            reader.Close()

            Try
                If txtloanno.Text = "" Then
                    MsgBox("NO DATA FOUND!!!")
                    Return
                Else
                    'conn.Open()
                    command = New MySqlCommand("delete from pacs.loan where loanno='" & txtloanno.Text & "'", conn)
                    reader = command.ExecuteReader
                    MsgBox("Are you sure to delete..?",MsgBoxStyle.YesNo)
                    conn.Close()
                End If
                reader.Close()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            MsgBox("NO DATA FOUND!!!")
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprnt.Click
        PrintPreviewDialog1.Document = Me.PrintDocument1

        Dim ButtonPressed As DialogResult = PrintPreviewDialog1.ShowDialog()
        If (ButtonPressed = DialogResult.OK) Then
            PrintDocument1.Print()
            Me.Close()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.Panel3.Width, Me.Panel3.Height)

        Panel3.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel3.Width, Me.Panel3.Height))

        e.Graphics.DrawImage(bm, 50, 60)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class