Public Class Form4
    Public webdata As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WebBrowser1.Navigate("https://dcpc.nctu.edu.tw/")

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        WebBrowser1.DocumentText += webdata
    End Sub
End Class