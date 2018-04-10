Public Class ExtruderCal
    Private hotend_1_stepRatio As Double
    Private hotend_2_stepRatio As Double

    Private Sub ExtruderCal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.Text = ">>" & vbCrLf &
            "Générer" & vbCrLf &
            "G-code" &
            vbCrLf &
            ">>"
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        TextBox14.Text = TextBox1.Text
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        TextBox13.Text = CDbl(TextBox4.Text) + 20.0
    End Sub

    Private Sub TextBox23_TextChanged(sender As Object, e As EventArgs) Handles TextBox23.TextChanged
        TextBox26.Text = TextBox23.Text
        hotend_1_stepRatio = Val(TextBox26.Text) * 100.0
    End Sub

    Private Sub TextBox22_TextChanged(sender As Object, e As EventArgs) Handles TextBox22.TextChanged
        TextBox25.Text = TextBox22.Text
        hotend_2_stepRatio = Val(TextBox25.Text) * 100.0
    End Sub


    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles TextBox17.TextChanged, TextBox14.TextChanged
        Try
            If TextBox17.Text.Length > 0 Then
                TextBox20.Text = CDbl(TextBox14.Text) - CDbl(TextBox17.Text)
            End If
        Catch
        End Try

    End Sub



    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged, TextBox13.TextChanged
        TextBox19.Text = Val(TextBox13.Text) - Val(TextBox16.Text)
    End Sub

    Private Sub TextBox26_TextChanged(sender As Object, e As EventArgs) Handles TextBox26.TextChanged
        TextBox29.Text = hotend_1_stepRatio / Val(TextBox26.Text)
    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click
        hotend_1_stepRatio = Val(TextBox26.Text) * 100.0
        hotend_2_stepRatio = Val(TextBox25.Text) * 100.0
        TextBox20.Text = CDbl(TextBox14.Text) - CDbl(TextBox17.Text)
        TextBox19.Text = CDbl(TextBox13.Text) - CDbl(TextBox16.Text)
        TextBox29.Text = CDbl(CInt(hotend_1_stepRatio / (CDbl(TextBox20.Text) + 20) * 100)) / 100.0
        TextBox28.Text = CDbl(CInt(hotend_2_stepRatio / (CDbl(TextBox19.Text) + 20) * 100)) / 100.0
    End Sub
End Class