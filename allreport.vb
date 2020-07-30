Imports MySql.Data.MySqlClient
Public Class allreport

    Private Sub allreport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub cmb1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb1.LostFocus
        If cmb1.SelectedIndex = 0 Then
            MaskedTextBox2.Visible = False
            Label10.Visible = False
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

        If cmb1.Text = "" Or MaskedTextBox1.Text = "" Then
            MsgBox("Select Report Type Date", MsgBoxStyle.Critical, "Input Error")
            cmb1.Focus()

        ElseIf cmb1.SelectedIndex = 0 Then
            Label7.Text = MaskedTextBox1.Text
            Label8.Text = MaskedTextBox1.Text
            Dim q As String = "select * from transaction where tdate='" & MaskedTextBox1.Text & "'"
            Dim adpt As New MySqlDataAdapter(q, conn)
            Dim ds As New DataSet()
            adpt.Fill(ds, "transaction")
            DataGridView1.DataSource = ds.Tables(0)
            conn.Close()
        ElseIf cmb1.SelectedIndex = 1 Then
            Label7.Text = MaskedTextBox1.Text
            Label8.Text = MaskedTextBox2.Text
            Dim q As String = "select * from transaction where tdate BETWEEN '" & MaskedTextBox1.Text & "' and '" & MaskedTextBox2.Text & "' "
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
                tot = tot + row.Cells(8).Value
                c = c + row.Cells(7).Value
                d = d + row.Cells(6).Value

            Next
            DataGridView1.Rows(max).Cells(8).Value += tot
            DataGridView1.Rows(max).Cells(7).Value += c
            DataGridView1.Rows(max).Cells(6).Value += d
            DataGridView1.Rows(max).Cells(5).Value = total
            'Label1.Text = tot
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        mainfrm.Show()
        MaskedTextBox1.Text = ""
        MaskedTextBox2.Text = ""
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
    End Sub

    Private Sub btnprnt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprnt.Click
        PrintDialog1.Document = Me.PrintDocument1

        Dim ButtonPressed As DialogResult = PrintDialog1.ShowDialog()
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

    Private Sub allreport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label6.Text = "@" & Login.txtUsername.Text
        Dim todaysdate As String = String.Format("{0:dd/MM/yyyy}", DateTime.Now)
        Label11.Text = todaysdate
    End Sub

    
    Private Sub cmb1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb1.SelectedIndexChanged

    End Sub
End Class