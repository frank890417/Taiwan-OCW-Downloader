Public Class Form2
    Dim tt As Integer = 0
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Opacity = 0

    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        tt += 1
        Me.Opacity += 1.0 / 50
        If tt > 250 Then
            Form1.Show()
            Me.Hide()

        End If
    End Sub
End Class