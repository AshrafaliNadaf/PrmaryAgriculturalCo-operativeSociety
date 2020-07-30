Imports MySql.Data.MySqlClient
Public Class reportloan

    Private Sub reportloan_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub cmb1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb1.LostFocus
        If cmb1.SelectedIndex = 0 Then
            MaskedTextBox2.Visible = False
            Label10.Visible = False
            Label5.Visible = False
            MaskedTextBox2.Text = ""
        Else
            MaskedTextBox2.Visible = True
            Label10.Visible = True
            Label5.Visible = True

        End If
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim str As String = "Server=localhost;userid=root;database=pacs"
        Dim conn As New MySqlConnection(str)

        lblfrmdate.Text = MaskedTextBox1.Text
        lbltodate.Text = MaskedTextBox2.Text
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        lbldate.Text = todaysdate

        If cmb1.Text = "" Or MaskedTextBox1.Text = "" Then
            MsgBox("Select Report Type and Date", MsgBoxStyle.Critical, "Input Error")
            cmb1.Focus()

        ElseIf cmb1.SelectedIndex = 0 Then

            Dim q As String = "select tdate as 'Transaction Date',loanno as'Loan No',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from transaction where tdate='" & MaskedTextBox1.Text & "' and loanno like '3%' "
            Dim adpt As New MySqlDataAdapter(q, conn)
            Dim ds As New DataSet()
            adpt.Fill(ds, "transaction")
            DataGridView1.DataSource = ds.Tables(0)
            conn.Close()
        ElseIf cmb1.SelectedIndex = 1 Then

            Dim q As String = "select tdate as 'Transaction Date',loanno as'Loan No',particulars as'Particulars',debit as'Debit',credit as'Credit',amt as'Amount' from transaction where tdate between '" & MaskedTextBox1.Text & "' and '" & MaskedTextBox2.Text & "' and loanno like '3%' "
            Dim adpt As New MySqlDataAdapter(q, conn)
            Dim ds As New DataSet()
            adpt.Fill(ds, "transaction")
            DataGridView1.DataSource = ds.Tables(0)
            conn.Close()

        End If
        Try
            'declaring variable as integer to store the value of the total rows in the datagridview

            Dim max As Integer = DataGridView1.Rows.Count - 1
            Dim total As String = "TOTAL="
            Dim tot As Integer = 0
            Dim c As Integer = 0
            Dim d As Integer = 0
            'getting the values of a specific rows


            For Each row As DataGridViewRow In DataGridView1.Rows
                'formula for adding the values in the rows  
                tot = tot + row.Cells(5).Value
                c = c + row.Cells(4).Value
                d = d + row.Cells(3).Value

            Next
            DataGridView1.Rows(max).Cells(5).Value += tot
            DataGridView1.Rows(max).Cells(4).Value += c
            DataGridView1.Rows(max).Cells(3).Value += d
            DataGridView1.Rows(max).Cells(2).Value = total
            'Label1.Text = tot
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub reportloan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label6.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
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
        Dim bm As New Bitmap(Me.Panel1.Width, Me.Panel1.Height)

        Panel1.DrawToBitmap(bm, New Rectangle(0, 0, Me.Panel1.Width, Me.Panel1.Height))

        e.Graphics.DrawImage(bm, 50, 60)
        Dim aPS As New PageSetupDialog
        aPS.Document = PrintDocument1
    End Sub
End Class