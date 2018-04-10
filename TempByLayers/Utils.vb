Imports System
Imports System.IO
Imports System.Text

Public Class Utils
    Public Sub New()

    End Sub
    Public Function ReadGcode(ByVal file_path As String) As String
        Dim path As String = file_path
        Dim readText As String = Nothing
        ' This text is added only once to the file.
        If File.Exists(path) = True Then
            readText = File.ReadAllText(path)
        End If
        Return readText
    End Function
    Public Function WriteGcode(ByVal file_path As String, ByVal str As String) As Boolean
        Try
            Dim path As String = file_path
            Dim readText As String = Nothing
            ' This text is added only once to the file.
            'If File.Exists(path) = False Then
            ' Dim createText As String = str + Environment.NewLine
            File.WriteAllText(path, str)
            ' This text is always added, making the file longer over time
            ' if it is not deleted.
            ' Dim appendText As String = "This is extra text" + Environment.NewLine
            ' File.AppendAllText(path, appendText)

            ' Open the file to read from.
            ' End If
            Return True
        Catch

            Return False
        End Try

    End Function
    Public Function ConvertPercentToH(ByVal fan_speeds() As Integer) As Integer()
        Dim maxH As Double = 255.0
        Dim maxD As Double = 100.0
        Dim valDtoH As Double = maxH / maxD
        For i As Integer = 0 To UBound(fan_speeds)
            fan_speeds(i) = Math.Max(0, Math.Min(255.0, CDbl(Math.Min(100, fan_speeds(i))) * valDtoH))
        Next
        Return fan_speeds
    End Function
    Public Function ReplaceSettings(ByVal hotend_temp() As Integer,
                                    ByVal hotend_temps_linked As Boolean,
                                    ByVal heatbed_temp() As Integer,
                                    ByVal heatbed_temps_linked As Boolean,
                                    ByVal flowrate() As Integer,
                                    ByVal flowrate_linked As Boolean,
                                    ByVal print_speed() As Integer,
                                    ByVal print_speed_linked As Boolean,
                                    ByVal fan_speeds() As Integer,
                                    ByVal fans_speed_linked As Boolean,
                                    ByVal str As String,
                                    ByVal slicer_name As String,
                                    ByVal layer_nbr_afterbase As Integer,
                                    ByVal layers() As Integer
                                    ) As String
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim layerstring As String = Nothing
        Dim layerHeight As Double = 0.00

        If fans_speed_linked Then
            For f As Integer = 1 To UBound(fan_speeds)
                fan_speeds(f) = fan_speeds(0)
            Next
        End If
        If hotend_temps_linked Then
            For f As Integer = 1 To UBound(hotend_temp)
                hotend_temp(f) = hotend_temp(0)
            Next
        End If
        If heatbed_temps_linked Then
            For f As Integer = 1 To UBound(hotend_temp)
                hotend_temp(f) = hotend_temp(0)
            Next
        End If
        If flowrate_linked Then
            For f As Integer = 1 To UBound(hotend_temp)
                hotend_temp(f) = hotend_temp(0)
            Next
        End If
        '*****///////////////
        Dim tempStr As String = Nothing
        'g-code start by 1
        If slicer_name.Equals("Cura Engine") Or slicer_name.Equals("Slic3r with custom ""before layer change"" gcode") Then
            layerstring = ";LAYER:"
            tempStr = layerstring & layer_nbr_afterbase & vbCrLf

        Else 'g-code start by 0
            layer_nbr_afterbase -= 1
            layerstring = "; layer ##, Z = "
            tempStr = Replace(layerstring, "##", layer_nbr_afterbase) & (layer_nbr_afterbase * layerHeight)
            tempStr = Replace(tempStr, ",", ".")

        End If
        str = Replace(str, tempStr, "M104 S" & hotend_temp(0) & " ; k13 hotend_temp tower temp0" & vbCrLf &
                                    "M106 S" & fan_speeds(0) & " ; k13 hotend_temp tower fan_sp0" & vbCrLf &
                                    tempStr)

        '***////////////////
        If slicer_name.Equals("Cura Engine") Then
            j = 0

            For i As Integer = 28 To 108 Step 20
                j += 1
                '   Console.WriteLine("layer z=" & (0.2 * CDbl(i + 1)))
                Dim tempStr As String = layerstring & i
                '  Console.WriteLine(tempStr)
                str = Replace(str, tempStr, "M104 S" & hotend_temp(j) & " ; k13 hotend_temp tower hotend_temp" & j & vbCrLf &
                                            "M106 S" & fan_speeds(j) & " ; k13 hotend_temp tower fan_sp" & j & vbCrLf &
                                            tempStr)

            Next
        Else
            If str.Contains(";   layerHeight,") Then
                Dim tofind As String = ";   layerHeight,"
                Dim pos1 As Integer = InStr(str, tofind)
                Dim tempStr_ As String = Mid(str, pos1 + tofind.Length)
                Dim pos2 As Integer = InStr(tempStr_, vbCrLf)
                Try
                    layerHeight = CDbl(Replace(Mid(tempStr_, 1, pos2 - 1), ".", ","))
                    'Dim splitted() As String = Split(layerheight_line, ",")
                Catch
                    layerHeight = CDbl(Mid(tempStr_, 1, pos2 - 1))
                End Try

            End If

            For i As Integer = 29 To 109 Step 20
                Dim tempStr As String = Replace(layerstring, "##", i) & Replace((i * layerHeight), ",", ".") & vbCrLf
                j += 1
                str = Replace(str, tempStr, "M104 S" & hotend_temp(j) & " ; k13 hotend_temp tower hotend_temp" & j & vbCrLf &
                                            "M106 S" & fan_speeds(j) & " ; k13 hotend_temp tower fan_sp" & j & vbCrLf &
                                            tempStr)
            Next
        End If
        Return str()
    End Function
End Class
