Imports MySql.Data.MySqlClient
Public Class viewallfd
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
            Query = "select odate as'opening date', fd.mid as'adhar no:',facno as'account no:',name,city,gender,phone,dob as 'Date of Birth',famt as'Amount' from fd,member where member.mid=fd.mid"
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

    Private Sub viewallfd_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        mainfrm.Show()
    End Sub
    Private Sub viewallfd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "@" & Login.txtUsername.Text
        Me.WindowState = FormWindowState.Maximized
        load_table()
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