Imports MySql.Data.MySqlClient
Public Class allloanview

    Dim conn As MySqlConnection
    Dim Command As MySqlCommand
    Dim dbDataSet As New DataTable
    Dim sda As New MySqlDataAdapter
    Dim bSource As New BindingSource
    Dim reader As MySqlDataReader
    Private Sub load_table()
        conn = New MySqlConnection
        conn.ConnectionString = "server=localhost;userid=root;database=pacs"
        Dim sda As New MySqlDataAdapter
        Dim bSource As New BindingSource
        Try
            conn.Open()
            Dim Query As String
            Query = "select odate as'Issue date',loanno as'Loan no:',loan.mid as'Adhar no:',ltype as'loan type:',name,city,gender,phone,lamt as'Loan Amount:',dueamt as'Due Amount:' from loan,member where member.mid=loan.mid"
            Command = New MySqlCommand(Query, conn)
            sda.SelectCommand = Command
            sda.SelectCommand = Command
            sda.Fill(dbDataSet)
            bSource.DataSource = dbDataSet
            DataGridView1.DataSource = bSource
            sda.Update(dbDataSet)
            conn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conn.Dispose()
        End Try
    End Sub

    Private Sub allloanview_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub

    Private Sub allloanview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        load_table()
        Label1.Text = "@" & Login.txtUsername.Text
    End Sub

    Private Sub txtsearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtsearch.GotFocus
        txtsearch.Text = ""
    End Sub

    Private Sub txtsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsearch.TextChanged
        Dim dv As New DataView(dbDataSet)
        Try
            dv.RowFilter = String.Format(" name Like '%{0}%'", txtsearch.Text)
            DataGridView1.DataSource = dv
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        mainfrm.Show()
    End Sub
End Class