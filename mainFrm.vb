'//main from
Imports System.Media
Imports System.Threading.Tasks

Public Class mainfrm
    Dim path = System.Windows.Forms.Application.StartupPath
    Dim LogOnsound As String
    Dim MyPlayer As New SoundPlayer()


    Private Sub mainfrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub FormProject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.Show()
        LogOnsound = "\LogOff.wav"

        lbluser.Text = "@" & Login.txtUsername.Text

        'Timer1.Start()
        Timer2.Start()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        MyPlayer.SoundLocation = path & LogOnsound
        MyPlayer.Play()
        Me.Close()
        Login.txtUsername.Text = ""
        Login.txtPassword.Text = ""
        Login.CheckBox1.Checked = False
        Login.Show()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Label2.Text = " " + Label2.Text
    End Sub

    Private Sub OpenNewAccountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenNewAccountToolStripMenuItem.Click
        mbrregfrm.Show()
        Me.Hide()
    End Sub

    Private Sub RegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegisterToolStripMenuItem.Click
        Me.Hide()
        Register.Show()
    End Sub

    Private Sub ChangeExistToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeExistToolStripMenuItem1.Click
        Forgot_password.Show()
        Me.Hide()
    End Sub

    Private Sub ViewAccountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAccountToolStripMenuItem.Click
        memberView.Show()
        Me.Hide()
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        fdReg.Show()
        Me.Hide()
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        savingsReg.Show()
        Me.Hide()
    End Sub

    Private Function memberReg() As Object
        Throw New NotImplementedException
    End Function


    Private Sub ViewAccountToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAccountToolStripMenuItem1.Click
        savingView.Show()
        Me.Hide()
    End Sub

    Private Sub ViewAccountToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAccountToolStripMenuItem3.Click
        FDview.Show()
        Me.Hide()
    End Sub

    Private Sub ToolStripMenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem10.Click
        employee.Show()
        Me.Hide()
    End Sub

    Private Sub ViewEmployeeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewEmployeeToolStripMenuItem.Click
        employeeView.Show()
        Me.Hide()
    End Sub

    Private Sub ViewAllMembersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAllMembersToolStripMenuItem.Click
        allmemberview.Show()
        Me.Hide()
    End Sub

    Private Sub ViewAllAccountsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAllAccountsToolStripMenuItem.Click
        viewallsavings.Show()
        Me.Hide()
    End Sub

    Private Sub ViewAllAccountsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAllAccountsToolStripMenuItem1.Click
        viewallfd.Show()
        Me.Hide()
    End Sub

    Private Sub ToolStripMenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem6.Click
        loan.Show()
        Me.Hide()

    End Sub

    Private Sub ViewAccountToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAccountToolStripMenuItem2.Click
        loanview.Show()
        Me.Hide()

    End Sub

    Private Sub ViewAllLoansToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAllLoansToolStripMenuItem.Click
        allloanview.Show()
    End Sub

    Private Sub SavingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SavingsToolStripMenuItem.Click
        reportsb.Show()
        Me.Hide()

    End Sub

    Private Sub FixedDepositsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FixedDepositsToolStripMenuItem.Click

        reportfd.Show()
        Me.Hide()

    End Sub

    Private Sub TotalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TotalToolStripMenuItem.Click
        allreport.show()
        Me.Hide()
    End Sub

    Private Sub LoanToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoanToolStripMenuItem.Click
        reportloan.Show()
        Me.Hide()
    End Sub

    Private Sub Timer1_Tick_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        'If Label1.Left < -1150 Then
        '    Label1.Left = 1500
        'Else
        '    Label1.Left -= 2
        'End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        PictureBox1.Visible = Not PictureBox1.Visible
    End Sub

    Private Sub MenuToolStripMenuItem_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuToolStripMenuItem.MouseEnter
        menuregister.ForeColor = Color.Lime
    End Sub

    Private Sub MenuToolStripMenuItem_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuToolStripMenuItem.MouseLeave
        menuregister.ForeColor = Color.Black
    End Sub

    Private Sub MemberToolStripMenuItem_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberToolStripMenuItem.MouseEnter
        menumbr.ForeColor = Color.Lime
    End Sub

    Private Sub MemberToolStripMenuItem_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberToolStripMenuItem.MouseLeave
        menumbr.ForeColor = Color.Black
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click

    End Sub

    Private Sub ToolStripMenuItem1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.MouseEnter
        ToolStripMenuItem1.ForeColor = Color.Lime
    End Sub

    Private Sub ToolStripMenuItem1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.MouseLeave
        ToolStripMenuItem1.ForeColor = Color.Black
    End Sub

    Private Sub ToolStripMenuItem3_MouseEnter1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.MouseEnter
        ToolStripMenuItem3.ForeColor = Color.Lime
    End Sub

    Private Sub ToolStripMenuItem3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.MouseLeave
        ToolStripMenuItem3.ForeColor = Color.Black
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click

    End Sub

    Private Sub ToolStripMenuItem5_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.MouseEnter
        ToolStripMenuItem5.ForeColor = Color.Lime
    End Sub

    Private Sub ToolStripMenuItem5_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.MouseLeave
        ToolStripMenuItem5.ForeColor = Color.Black
    End Sub

    Private Sub ToolStripMenuItem9_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem9.MouseEnter
        ToolStripMenuItem9.ForeColor = Color.Lime
    End Sub

    Private Sub ToolStripMenuItem9_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem9.MouseLeave
        ToolStripMenuItem9.ForeColor = Color.Black
    End Sub

    Private Sub ToolStripMenuItem11_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem11.MouseEnter
        ToolStripMenuItem11.ForeColor = Color.Lime
    End Sub

    Private Sub ToolStripMenuItem11_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem11.MouseLeave
        ToolStripMenuItem11.ForeColor = Color.Black
    End Sub

End Class