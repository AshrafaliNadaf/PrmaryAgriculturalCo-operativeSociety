Imports MySql.Data.MySqlClient
Public Class FDview
    Dim mysqlconn As MySqlConnection
    Dim command As MySqlCommand
    Dim dbdataset As New DataTable
    Private Sub FDview_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Me.Hide()
        mainfrm.Show()
        txtamt.Text = ""
        txtname.Text = ""
        txtmdate.Text = ""
        txtacno.Text = ""
        txtInterest.Text = ""
        txtApinterest.Text = ""
        txtTotal.Text = ""
        lblacno.Text = ""
        txtduration.Text = ""
        txtodate.Text = ""
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
            query = "select facno,name,famt,odate,duration from pacs.fd,pacs.member where fd.facno=' " & txtacno.Text & " ' and  fd.mid=member.mid "
            Command = New MySqlCommand(query, mysqlconn)
            reader = command.ExecuteReader
            If reader.HasRows Then

                While reader.Read
                    txtacno.Text = reader.GetUInt64("facno")
                    txtname.Text = reader.GetString("name")
                    txtamt.Text = reader.GetDouble("famt")
                    txtodate.Text = reader.GetString("odate")
                    txtduration.Text = reader.GetInt64("duration")
                End While
                reader.Close()
                Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from pacs.transaction where facno='" & txtacno.Text & "' "
                'Dim q As String = "select tdate as 'Transaction Date',particulars as'Particulars',debit as'Debit',amt as'Amount' from transaction where facno='" & txtacno.Text & "' "
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
                adapter = New MySqlDataAdapter("select photo,sign from pacs.member,pacs.fd where member.mid = fd.mid and facno='" & txtacno.Text & "'", mysqlconn)

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

                mysqlconn.Close()

            Else
                MsgBox("NO DATA FOUND!!!")
                txtduration.Text = ""
                txtamt.Text = ""
                txtname.Text = ""
                txtmdate.Text = ""
                txtTotal.Text = ""
                txtApinterest.Text = ""
                txtInterest.Text = ""
                txtodate.Text = ""
                lblacno.Text = ""
                txtacno.Focus()
                PictureBox1.Image = Nothing
                PictureBox2.Image = Nothing
                dbdataset.Clear()
                DataGridView1.DataSource = Nothing
                DataGridView1.Refresh()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

      
        'code for combobox interest
        If txtduration.Text = "1" Then
            txtInterest.Text = "8.5"
            Dim tdate1 As String = String.Format("{0:dd/MM/yyyy}", Date.Now.AddYears(1))
            txtmdate.Text = tdate1
        ElseIf txtduration.Text = "2" Then
            txtInterest.Text = "10.0"
            Dim todaysdate2 As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddYears(2))
            txtmdate.Text = todaysdate2
        ElseIf txtduration.Text = "3" Then
            txtInterest.Text = "10.5"
            Dim todaysdate3 As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddYears(3))
            txtmdate.Text = todaysdate3
        ElseIf txtduration.Text = "4" Then
            txtInterest.Text = "11.0"
            Dim todaysdate4 As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddYears(4))
            txtmdate.Text = todaysdate4
        ElseIf txtduration.Text = "5" Then
            txtInterest.Text = "11.5"
            Dim todaysdate5 As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now.AddYears(5))
            txtmdate.Text = todaysdate5

        End If
        'code to calculate interest applied and total
        txtTotal.Text = Val(txtamt.Text) * ((1 + ((Val(txtInterest.Text)) / 100)) ^ Val(txtduration.Text))
        txtApinterest.Text = Val(txtTotal.Text) - Val(txtamt.Text)
        lblacno.Text = txtacno.Text
    End Sub

    Private Sub btnwtdrw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnwtdrw.Click
        Dim trn As Integer

        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        'mysqlconn.Open()
        Dim reader As MySqlDataReader
        Dim date1 As String = txtodate.Text
        Dim date2 As String = txtmdate.Text
        If date1 < date2 Then
            Try
                Dim dialog As DialogResult
                dialog = MessageBox.Show("Date is not matured, Are sure to Withdraw amount?", "exit", MessageBoxButtons.YesNo)
                If dialog = DialogResult.Yes Then
                    If txtamt.Text = "" Or txtamt.Text = "0" Then
                        MsgBox("You dont have any Fd Amount", MsgBoxStyle.Information)
                    Else : Try
                            mysqlconn.Open()
                            If txtacno.Text = "" Then
                                MsgBox("NO DATA FOUND!!!")
                                Return
                            Else

                                Dim query As String
                                Dim query1 As String
                                Dim fd As String = "FD Withdrawn"
                                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
                                query = "update pacs.fd set famt=famt-' " & txtamt.Text & " ' where facno=' " & txtacno.Text & " '"
                                query1 = "INSERT INTO pacs.transaction(facno,particulars,tdate,amt,debit)values('" & txtacno.Text & "','" & fd & "','" & todaysdate & "','" & txtamt.Text & "','" & (0.8 * (txtTotal.Text) / 100) + txtamt.Text & "')"
                                Dim q = String.Concat(query, ";", query1)
                                command = New MySqlCommand(q, mysqlconn)
                                reader = command.ExecuteReader
                                MsgBox("Withdraw Successful")
                                txtamt.Text = 0
                                trn = command.LastInsertedId ' obtained last TRN no...

                            End If
                            mysqlconn.Close()
                        Catch ex As Exception
                            MsgBox("NO Data")
                        End Try
                    End If
                    Try
                        mysqlconn.Open()

                        Dim query1 As String

                        query1 = "update pacs.transaction set transaction.amt=amt-'" & txtamt.Text & "' where tno=" & trn & ";"    
                        command = New MySqlCommand(query1, mysqlconn)
                        reader = command.ExecuteReader
                        mysqlconn.Close()

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            If txtamt.Text = "" Or txtamt.Text = "0" Then
                MsgBox("You dont have any Fd Amount", MsgBoxStyle.Information)
            Else : Try
                    mysqlconn.Open()
                    If txtacno.Text = "" Then
                        MsgBox("NO DATA FOUND!!!")
                        Return
                    Else

                        Dim query As String
                        Dim query1 As String
                        Dim fd As String = "FD Withdrawn"
                        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
                        query = "update pacs.fd set famt=famt-' " & txtamt.Text & " ' where facno=' " & txtacno.Text & " '"
                        query1 = "INSERT INTO pacs.transaction(facno,particulars,tdate,amt,debit)values('" & txtacno.Text & "','" & fd & "','" & todaysdate & "','" & txtamt.Text & "','" & txtTotal.Text & "')"
                        Dim q = String.Concat(query, ";", query1)
                        command = New MySqlCommand(q, mysqlconn)
                        reader = command.ExecuteReader
                        MsgBox("Withdraw Successful")

                    End If
                    mysqlconn.Close()
                Catch ex As Exception
                    MsgBox("NO Data")
                End Try
            End If

            Try

                mysqlconn.Open()
                Dim query1 As String
                Dim fd As String = "FD Withdrawn"
                Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
                query1 = "update pacs.transaction set transaction.amt=amt-'" & txtamt.Text & "' where tno=" & trn & ";"
                command = New MySqlCommand(query1, mysqlconn)
                reader = command.ExecuteReader
                mysqlconn.Close()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclear.Click
        txtacno.Text = ""
        txtamt.Text = ""
        txtApinterest.Text = ""
        txtInterest.Text = ""
        txtname.Text = ""
        txtmdate.Text = ""
        txtTotal.Text = ""
        txtduration.Text = ""
        txtodate.Text = ""
        lblacno.Text = ""
        txtacno.Focus()
        dbdataset.Clear()
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
        PictureBox1.Image = Nothing
        PictureBox2.Image = Nothing

    End Sub

    Private Sub FDview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        txttoday.Text = todaysdate
        Label18.Text = "@" & Login.txtUsername.Text
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
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

    Private Sub btndlt_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndlt.Click
        Dim reader As MySqlDataReader
        mysqlconn = New MySqlConnection()
        mysqlconn.ConnectionString = "server=localhost;User ID=root;database=pacs"  '//connection
        mysqlconn.Open()
        Dim query As String
        query = "select *from pacs.fd where fd.facno=' " & txtacno.Text & " '"
        command = New MySqlCommand(query, mysqlconn)
        reader = command.ExecuteReader
        If reader.HasRows Then
            reader.Close()
            Try
                If txtacno.Text = "" Then
                    MsgBox("NO DATA FOUND!!!")
                    Return
                Else
                    Dim dialog As DialogResult
                    dialog = MsgBox("Do you really want to delete?", vbYesNo)
                    If dialog = DialogResult.Yes Then

                        Dim Query1 As String
                        Query1 = "delete from pacs.fd where facno='" & txtacno.Text & "'"
                        command = New MySqlCommand(Query1, mysqlconn)
                        reader = command.ExecuteReader
                        MsgBox("Account deleted")
                        mysqlconn.Close()
                        txtacno.Text = ""
                        txtamt.Text = ""
                        txtApinterest.Text = ""
                        txtInterest.Text = ""
                        txtname.Text = ""
                        txtmdate.Text = ""
                        txtTotal.Text = ""
                        txtduration.Text = ""
                        lblacno.Text = ""
                        txtodate.Text = ""
                        txtacno.Focus()
                        DataGridView1.DataSource = Nothing
                        DataGridView1.Refresh()
                        PictureBox1.Image = Nothing
                        PictureBox2.Image = Nothing
                    End If
                    reader.Close()
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        Else
            MsgBox("NO DATA FOUND!!!")
        End If
        mysqlconn.Close()
    End Sub
End Class