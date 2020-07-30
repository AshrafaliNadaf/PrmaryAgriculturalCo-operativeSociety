'// Project Form

Imports System.Media

Public Class FormProject
    Dim path = System.Windows.Forms.Application.StartupPath
    Dim LogOnsound As String
    Dim MyPlayer As New SoundPlayer()

    Private Sub FormProject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LogOnsound = "\LogOff.wav"
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        MyPlayer.SoundLocation = path & LogOnsound
        MyPlayer.Play()
        Login.Close()
        Me.Close()
    End Sub
End Class