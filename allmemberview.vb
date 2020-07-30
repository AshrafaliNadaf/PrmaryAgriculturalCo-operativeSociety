Imports MySql.Data.MySqlClient
Public Class allmemberview

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
            Query = "select mid as'adhar no:',name,address,city,gender,phone,dob as 'Date of Birth',amount as'Amount' from pacs.member"
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

    Private Sub allmemberview_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        DataGridView1.DataSource = Nothing
        DataGridView1.Refresh()
        Me.Hide()
        mainfrm.Show()

    End Sub

    Private Sub allmemberview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        load_table()
        Label1.Text = "@" & Login.txtUsername.Text
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
        mainfrm.Show()
    End Sub
End Class