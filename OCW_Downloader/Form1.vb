Imports System.Net
Imports System.Text
Imports System.IO

Public Class Form1

    Dim oldx, oldy As Integer
    Dim clicked As Boolean = False
    Dim url, url2 As String
    Dim classtitle As String
    Dim pdflist As New ArrayList
    Dim pdfnamelist As New ArrayList
    Dim mp4list As New ArrayList
    Dim mp4namelist As New ArrayList
    Dim waiter As Byte()
    Dim Thread1 As New System.Threading.Thread(AddressOf hhd)
    Dim globalmes As String = ""


    Public Sub status(ByVal Mes As String)

        Try
            Label1.Text = Mes & "..."
            Application.DoEvents()
        Catch ex As Exception
            globalmes = Mes
        End Try
      
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        status("初始化")

        pdflist.Clear()
        mp4list.Clear()

        pdfnamelist.Clear()
        mp4namelist.Clear()

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

        Dim accept As Boolean = True

        If TextBox1.Text.Contains("ocw.nctu.edu.tw") Then
            purseNCTU(TextBox1.Text)
        ElseIf TextBox1.Text.Contains("ocw.nthu.edu.tw") Then
            purseNTHU(TextBox1.Text)
        ElseIf TextBox1.Text.Contains("ocw.aca.ntu.edu.tw") Then
            purseNTU(TextBox1.Text)

        Else
            status("不支援的OCW網站")
            accept = True

        End If

        If accept Then
            status("分析完成! 共有" & pdflist.Count & " 份pdf講義 以及" & mp4list.Count & "份mp4影音")

            For i As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, True)
            Next

            For o As Integer = 0 To ListBox2.Items.Count - 1
                ListBox2.SetItemChecked(o, True)
            Next
        End If
    End Sub



    Public Sub purseNCTU(url As String)
        Dim useurl As String = url.Replace("detail_4", "detail").Replace("detail_3", "detail").Replace("detail_2", "detail").Replace("detail_1", "detail")


        useurl = useurl.Replace("detail", "detail_4")
        hh.Encoding = System.Text.Encoding.UTF8
        Dim datapath As String = Application.StartupPath & "\Data.txt"
        Dim dataa As String = hh.DownloadString(useurl)
        Dim shownext As Boolean = False
        Dim lastext1, lastext2, lastext3, lastext4, lastext5 As String

        For Each ff As String In Split(dataa, """")

            If shownext Then
                shownext = False
                classtitle = Split(Split(ff, ">")(1), "<")(0)
                classtitle = "[交大OCW]" + classtitle.Trim()
                Label2.Text = "課程名稱 <交大OCW>: " + classtitle

            End If
            If ff.Trim.EndsWith(".pdf") Then
                status("取得pdf網址")
                pdflist.Add(ff)

                Dim pdfname As String = Split(Split(lastext3, ">")(2), "<")(0).Trim
                pdfnamelist.Add(pdfname)
                ListBox1.Items.Add(pdfname & ".pdf")
            End If
            If ff.Contains("newstitle5") Then
                status("取得標題")
                shownext = True
            End If
            lastext3 = lastext2
            lastext2 = lastext1
            lastext1 = ff
        Next


        url2 = useurl.Replace("detail_4", "detail_3")
        Dim datapath2 As String = Application.StartupPath & "\Data2.txt"
        If My.Computer.FileSystem.FileExists(datapath2) Then
            My.Computer.FileSystem.DeleteFile(datapath2, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If

        status("下載mp4資料")

        dataa = hh.DownloadString(url2)

        status("分析mp4資料")
        Dim tempname As String
        For Each ff As String In Split(dataa, """")
            If ff.Contains("images/icon_online.jpg") Then
                Dim mp4name As String = Split(Split(lastext5, ">")(4), "<")(0).Trim
                mp4namelist.Add(mp4name)
                tempname = mp4name
            End If
            If ff.Trim.EndsWith(".mp4") Then
                status("取得mp4網址")
                mp4list.Add(ff)
                ListBox2.Items.Add(tempname & ".mp4")
            End If

            lastext5 = lastext4
            lastext4 = lastext3
            lastext3 = lastext2
            lastext2 = lastext1
            lastext1 = ff
        Next

        status("分析完成! 共有" & pdflist.Count & " 份pdf講義 以及" & mp4list.Count & "份mp4影音")

        For i As Integer = 0 To ListBox1.Items.Count - 1
            ListBox1.SetItemChecked(i, True)
        Next

        For o As Integer = 0 To ListBox2.Items.Count - 1
            ListBox2.SetItemChecked(o, True)
        Next
    End Sub



    Public Sub purseNTU(url As String)
        hh.Encoding = System.Text.Encoding.UTF8
        Dim datapath As String = Application.StartupPath & "\Data.txt"
        Dim dataa As String = hh.DownloadString(url)
        Dim shownext As Boolean = False
        Dim lastext1, lastext2, lastext3, lastext4, lastext5 As String


        Dim catching As Boolean = False
        Dim catchnum As Integer = 0
        Dim temp As Integer = -1
        Dim namea As String
        Dim linka As String
        Dim coursename As String = ""
        Dim courselist As New ArrayList
        Dim lasts(7) As String
        Dim coursecount As Integer
        Dim title As String = ""
        Dim gettitle As Boolean = True


        For i As Integer = 1 To 7
            lasts(i) = ""

        Next

        For Each ff As String In Split(dataa, """")
            If ff.Contains("<title>") And gettitle = True Then
                coursename = Split(Split(ff, ">")(2), " - 臺大開放式課")(0)
                Label2.Text = "課程名稱 <台大OCW>: " + coursename
                classtitle = "[台大OCW]" + coursename
                Application.DoEvents()
                gettitle = False
            End If
            If (lasts(6).Contains("本課程共")) Then
                coursecount = CInt(Split(Split(lasts(4), ">")(1), "<")(0))
            End If

            For i As Integer = 6 To 1 Step -1
                lasts(i + 1) = lasts(i)
            Next
            lasts(1) = ff
        Next

        For i As Integer = 1 To 7
            lasts(i) = ""

        Next

        For i As Integer = 1 To coursecount

            status("下載mp4資料")

            dataa = hh.DownloadString(url & "/" & i)
            Dim nowcount As Integer = 1
            Dim cname As String = ""
            Dim filecount As Integer = 0
            For Each piece In Split(dataa, """")

                If piece.Contains("內容：") Then
                    cname = Split(Split(piece, "內容：")(1), "<")(0).Trim()
                End If
                If piece.Contains("下載講義") Then
                    filecount += 1

                    status("取得教材網址")
                    pdflist.Add(lasts(3))

                    pdfnamelist.Add("單元-" & i & cname & "_" & filecount & "." & Split(lasts(3), ".")(Split(lasts(3), ".").Length - 1))
                    ListBox1.Items.Add("單元-" & i & cname & "_" & filecount & "." & Split(lasts(3), ".")(Split(lasts(3), ".").Length - 1))
                End If
                If piece.Contains(".mp4") Then
                    status("取得mp4網址")
                    Dim mp4name As String = "單元-" & i & "_" & cname
                    mp4namelist.Add(mp4name)
                    mp4list.Add(Split(piece, "'")(1))
                    ListBox2.Items.Add(mp4name & ".mp4")
                    nowcount += 1
                    Application.DoEvents()
                End If

                For o As Integer = 6 To 1 Step -1
                    lasts(o + 1) = lasts(o)
                Next
                lasts(1) = piece
            Next
        Next



        status("分析完成! 共有" & pdflist.Count & " 份pdf講義 以及" & mp4list.Count & "份mp4影音")

        For i As Integer = 0 To ListBox1.Items.Count - 1
            ListBox1.SetItemChecked(i, True)
        Next

        For o As Integer = 0 To ListBox2.Items.Count - 1
            ListBox2.SetItemChecked(o, True)
        Next
    End Sub




    Public Sub purseNTHU(url As String)
        hh.Encoding = System.Text.Encoding.UTF8
        Dim datapath As String = Application.StartupPath & "\Data.txt"
        Dim dataa As String = hh.DownloadString(url)
        Dim shownext As Boolean = False
        Dim lastext1, lastext2, lastext3, lastext4, lastext5 As String


        Dim catching As Boolean = False
        Dim catchnum As Integer = 0
        Dim temp As Integer = -1
        Dim namea As String
        Dim linka As String
        Dim coursename As String = ""
        Dim courselist As New ArrayList
        Dim nextname As Integer = -2
        Dim teachername As String = ""
        Dim nextteacher As Integer = -2
        Dim lasts(7) As String

        For i As Integer = 1 To 7
            lasts(i) = ""

        Next

        For Each ff As String In Split(dataa, """")
            If ff = "search2" Then
                catching = True
                Continue For
            End If
            If lasts(6).Contains("授課老師") Then
                teachername = Split(Split(lasts(2), ">")(1), "<")(0)
                Label2.Text = "課程名稱 <清大OCW>: " + coursename + "_" + teachername
                classtitle = "[清大OCW]" + coursename + "_" + teachername
                Application.DoEvents()
            End If

            If (nextname = -1) Then
                coursename = Split(Split(ff, ">")(1), "<")(0)
                Label2.Text = "課程名稱 <清大OCW>: " + coursename
                nextname = 0
            End If
            If ff = "title" And nextname = -2 Then
                nextname = -1
            End If
            If ff.Contains("/ul") Then
                catching = False
            End If

            If (catching = True) Then
                temp += 1
                If temp Mod 2 = 0 Then
                    namea = Split(Split(ff, ">")(1), "<")(0)
                    If linka <> "" Then
                        courselist.Add(New clsClass(namea, "http://ocw.nthu.edu.tw/ocw/" + linka))
                    End If

                    'MsgBox(namea + vbNewLine + linka)
                Else
                    linka = ff

                End If

            End If


            For i As Integer = 6 To 1 Step -1
                lasts(i + 1) = lasts(i)
            Next
            lasts(1) = ff
        Next

        For Each course As clsClass In courselist

            status("下載mp4資料")

            dataa = hh.DownloadString(course.url)
            Dim nowcount As Integer = 1
            For Each piece In Split(dataa, """")
                If piece.Contains("320_240_256.MP4") Then
                    status("取得mp4網址")
                    Dim mp4name As String = course.name + "_" + nowcount.ToString
                    mp4namelist.Add(mp4name)
                    mp4list.Add(piece)
                    ListBox2.Items.Add(mp4name & ".mp4")
                    nowcount += 1
                    Application.DoEvents()
                End If
            Next

        Next


        status("分析完成! 共有" & pdflist.Count & " 份pdf講義 以及" & mp4list.Count & "份mp4影音")

        For i As Integer = 0 To ListBox1.Items.Count - 1
            ListBox1.SetItemChecked(i, True)
        Next

        For o As Integer = 0 To ListBox2.Items.Count - 1
            ListBox2.SetItemChecked(o, True)
        Next
    End Sub


    Public Function getiefilename(ByVal path As String)
        Return path.Substring(path.LastIndexOf("/") + 1)
    End Function

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        status("取得mp4下載位置")
        Dim downloadfolder As String = Application.StartupPath & "\" & classtitle & "\MP4_Files\"
        My.Computer.FileSystem.CreateDirectory(downloadfolder)
        Dim mp4url As String
        Dim counter As Integer = 0
        For Each i In ListBox2.CheckedIndices
            counter += 1
            mp4url = mp4list(i)
            Try
                'My.Computer.Network.DownloadFile(mp4url, downloadfolder & i & "-" & corfilename(mp4namelist(i)) & ".mp4", "", "", True, 100, True)
                dcol.Add(New clsDownInfo(i & "-" & mp4namelist(i), mp4url, downloadfolder & i & "-" & corfilename(mp4namelist(i)) & ".mp4"))
            Catch ex As Exception
            End Try
        Next
        startDownload()
        ' status("下載mp4完畢 共下載了" & counter & "個檔案")

    End Sub

    Dim WithEvents hh As New Net.WebClient
    Dim dcol As New Collection

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        status("取得pdf下載位置")
        Dim downloadfolder As String = Application.StartupPath & "\" & classtitle & "\PDF_Files\"
        My.Computer.FileSystem.CreateDirectory(downloadfolder)
        Dim pdfurl As String
        Dim counter As Integer = 0
        For Each i In ListBox1.CheckedIndices
            counter += 1
            pdfurl = pdflist(i)
            If pdfurl.StartsWith("../") Then
                pdfurl = pdfurl.Replace("../", "http://ocw.nctu.edu.tw/")
            End If
            Try


                ' My.Computer.Network.DownloadFile(pdfurl, downloadfolder & i & "-" & corfilename(pdfnamelist(i)) & ".pdf", "", "", False, 100, True)

                Dim fname As String = pdfnamelist(i)
                If fname.Substring(fname.Length - 5).Contains(".") = False Then
                    fname += ".pdf"
                End If

                dcol.Add(New clsDownInfo(i & "-" & pdfnamelist(i), pdfurl, downloadfolder & i & "-" & corfilename(fname)))
            Catch ex As Exception
            End Try
        Next
        startDownload()
        'status("下載pdf完畢 共下載了" & counter & "個檔案")

    End Sub

    Private Sub Client_DownloadProgressChanged(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs) Handles hh.DownloadProgressChanged '當Client正在下載時
        'e.BytesReceived 取得已下載的檔案大小
        'e.TotalBytesToReceive 取得總共要下載的檔案大小
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Dim lastfolder As String

    Public Sub hhd()
        hh.DownloadFile(New Uri(dcol.Item(1).url), dcol.Item(1).file)
       
    End Sub

    Public Sub startDownload()
        If dcol.Count > 0 Then
            status("下載:" & dcol.Item(1).showname & "中")
            'hh.DownloadDataAsync(New Uri(dcol.Item(1).url))

            Thread1 = New System.Threading.Thread(AddressOf hhd)
            Thread1.Start()
            timer1.start()



            lastfolder = dcol.Item(1).file.ToString.Substring(0, dcol.Item(1).file.ToString.LastIndexOf("\"))
        End If
    End Sub

    Public Sub AbortThisDownload()
        hh.CancelAsync()
        If dcol.Count > 1 Then
            dcol.Remove(1)
            status("下載:" & dcol.Item(1).showname & "中")
            hh.DownloadDataAsync(New Uri(dcol.Item(1).url))
            lastfolder = dcol.Item(1).file.ToString.Substring(0, dcol.Item(1).file.ToString.LastIndexOf("\"))

        End If
    End Sub

    Public Sub AbortAllDownload()
        hh.CancelAsync()
        dcol.Clear()
        ProgressBar1.Value = 0
    End Sub

    Private Sub Client_DownloadProgresscomplete(ByVal sender As Object, ByVal e As System.Net.DownloadDataCompletedEventArgs) Handles hh.DownloadDataCompleted '當Client正在下載時

        'e.BytesReceived 取得已下載的檔案大小
        'e.TotalBytesToReceive 取得總共要下載的檔案大小
        ProgressBar1.Value = 100
        Application.DoEvents()
        If dcol.Count > 1 Then
            My.Computer.FileSystem.WriteAllBytes(dcol.Item(1).file, e.Result, False)
            dcol.Remove(1)
            status("下載:" & dcol.Item(1).showname & "中")
            Application.DoEvents()
            hh.DownloadDataAsync(New Uri(dcol.Item(1).url))
            lastfolder = dcol.Item(1).file.ToString.Substring(0, dcol.Item(1).file.ToString.LastIndexOf("\"))
        ElseIf dcol.Count = 1 Then
            My.Computer.FileSystem.WriteAllBytes(dcol.Item(1).file, e.Result, False)
            dcol.Clear()
            status("下載完畢")
            ProgressBar1.Value = 0

            Process.Start(lastfolder)
        End If

    End Sub



    Public Function corfilename(ByVal filename As String)
        Dim nouse As String = "\m/m:m*m?m""m<m>m|"
        Dim rep As String = "_"
        For Each cc In Split(nouse, "m")
            filename = filename.Replace(cc, rep)
        Next
        Return filename
    End Function



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ResizeRedraw = True
        hh.Encoding = System.Text.Encoding.UTF8
        Dim dataa As String = hh.DownloadString("http://www.monoame.com/ocw/count.php")
        hh.Encoding = System.Text.Encoding.UTF8
        dataa = hh.DownloadString("http://www.monoame.com/ocw/news.php")
        MsgBox(dataa)

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If sender.Text = "Unselect All" Then
            For i As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, False)
            Next
            sender.Text = "Select All"
        Else
            For i As Integer = 0 To ListBox1.Items.Count - 1
                ListBox1.SetItemChecked(i, True)
            Next
            sender.Text = "Unselect All"
        End If

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        If sender.Text = "Unselect All" Then
            For i As Integer = 0 To ListBox2.Items.Count - 1
                ListBox2.SetItemChecked(i, False)
            Next
            sender.Text = "Select All"
        Else
            For i As Integer = 0 To ListBox2.Items.Count - 1
                ListBox2.SetItemChecked(i, True)
            Next
            sender.Text = "Unselect All"
        End If
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        oldx = e.X
        oldy = e.Y
        clicked = True
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If clicked Then
            Me.Left += e.X - oldx
            Me.Top += e.Y - oldy

        End If

    End Sub

    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp

        clicked = False

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Application.Exit()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Process.Start("https://www.facebook.com/MajerOWOb")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form3.Show()

    End Sub


    Public Sub pursee3(url As String)
        Application.DoEvents()
        hh.Encoding = System.Text.Encoding.UTF8
        Dim datapath As String = Application.StartupPath & "\Data.txt"
        Dim dataa As String = url
        Dim shownext As Boolean = False
        Dim lastext1, lastext2, lastext3, lastext4, lastext5 As String


        Dim catching As Boolean = False
        Dim catchnum As Integer = 0
        Dim temp As Integer = -1
        Dim namea As String
        Dim linka As String
        Dim coursename As String = ""
        Dim courselist As New ArrayList
        Dim lasts(9) As String
        Dim coursecount As Integer
        Dim title As String = ""
        Dim gettitle As Boolean = True
        Dim AttachMediaId As String
        Dim CourseId As String
        Dim nameaa As String
        Dim testhtml As String = "<!DOCTYPE html><head runat='server'>" + vbNewLine

        '<script type='text/javascript'>

        For i As Integer = 1 To 9
            lasts(i) = ""

        Next
        Dim count As Integer = 0

        For Each ff As String In Split(dataa, """")
            If ff.Contains("AttachMediaId") Then
                AttachMediaId = Split(Split(ff, "AttachMediaId=")(1), ",")(0)
                CourseId = Split(Split(ff, "CourseId=")(1), ",")(0)
                CourseId = CourseId.Substring(0, CourseId.Length - 1)

            End If
            If lasts(7).Contains("AttachMediaId") Then
              

                count += 1
                nameaa = Split(Split(ff, ">")(1), "<")(0)


                status("取得教材網址")
                Dim urll As String = "https://e3.nctu.edu.tw/NCTU_Easy_E3P/LMS2/common_get_content_media_attach_file.ashx?StudyLog=1&AttachMediaId=" & AttachMediaId & "&CourseId=" & CourseId
                pdflist.Add(urll)

                pdfnamelist.Add(nameaa)
                ListBox1.Items.Add(nameaa)
                testhtml += "<a href='" & urll & "' >" & nameaa & "</a><br>"
                'testhtml += "var win =window.open('" & urll & "', null , 'width=0,height=0');" + vbNewLine
                'testhtml += "<iframe style=""display:none"" "
                'testhtml += "src=""" & urll & """> </iframe>" + vbNewLine
           
                'testhtml += "win.document.execCommand(""SaveAs"", true, """ & nameaa & """);" + vbNewLine

                'testhtml += "win.close();" + vbNewLine

            End If
            For i As Integer = 8 To 1 Step -1
                lasts(i + 1) = lasts(i)
            Next
            lasts(1) = ff
        Next

        For i As Integer = 1 To 8
            lasts(i) = ""

        Next


        '</script> 
        testhtml += "</head></html>"
        My.Computer.FileSystem.WriteAllText(Application.StartupPath + "\temp.html", testhtml, False)

        Process.Start("" + Application.StartupPath + "\temp.html")
        Form4.WebBrowser1.DocumentText = testhtml
        Form4.webdata = testhtml
        status("分析完成! 共有" & pdflist.Count & " 份pdf講義 以及" & mp4list.Count & "份mp4影音")

        For i As Integer = 0 To ListBox1.Items.Count - 1
            ListBox1.SetItemChecked(i, True)
        Next

        For o As Integer = 0 To ListBox2.Items.Count - 1
            ListBox2.SetItemChecked(o, True)
        Next
    End Sub

    Dim ttt As Integer = 0
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Thread1.IsAlive = True Then
            ttt += 1
            If ttt Mod 5 = 0 Then
                Dim pp As Integer = ProgressBar1.Value
                If pp + 1 < ProgressBar1.Maximum Then
                    ProgressBar1.Value = pp + 1
                Else
                    ProgressBar1.Value = 0
                End If
            End If

        Else
            dcol.Remove(1)
            If dcol.Count > 0 Then
                startDownload()
            Else
                Timer1.Stop()
            End If


        End If
    End Sub
End Class
