'rotating wire frame cube by Crazy Pennie
'https://social.msdn.microsoft.com/Forums/vstudio/en-US/811c3452-1f97-452f-8af4-22219b095dd7/drawing-3d-shapes-in-2d?forum=vbgeneral
Imports System.Drawing.Drawing2D
Public Class Form2

    Private SpaceX As New Space
    Private WithEvents T As New Timer With {.Enabled = True, .Interval = 10}
    Private Teta As Double = 2 * Math.PI / 360
    Private R As Double = 100
    Private Front As Integer

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.DoubleBuffered = True
        Me.Size = New Size(650, 550)
        SpaceX.Add(New Point3D(0, 0, 3)) '(x, y, z)
        SpaceX.Add(New Point3D(0, 100, 5))
        SpaceX.Add(New Point3D(0, 200, 3))
        SpaceX.Add(New Point3D(0, 300, 2))
        SpaceX.Add(New Point3D(0, 400, 5))
        SpaceX.Add(New Point3D(0, 500, 7))

        SpaceX.Add(New Point3D(1, 0, 0)) '(x, y, z)
        SpaceX.Add(New Point3D(1, 100, 1))
        SpaceX.Add(New Point3D(1, 200, 1))
        SpaceX.Add(New Point3D(1, 300, 0))
        SpaceX.Add(New Point3D(1, 400, 2))
        SpaceX.Add(New Point3D(1, 500, 6))

        SpaceX.Add(New Point3D(2, 0, 1)) '(x, y, z)
        SpaceX.Add(New Point3D(2, 100, 0))
        SpaceX.Add(New Point3D(2, 200, 0))
        SpaceX.Add(New Point3D(2, 300, 0))
        SpaceX.Add(New Point3D(2, 400, 3))
        SpaceX.Add(New Point3D(2, 500, 5))


    End Sub


    Private Sub Form2_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Up Then
            If SpaceX.cameraY > -150 Then
                SpaceX.cameraY -= 5
            End If
        ElseIf e.KeyCode = Keys.Down Then
            If SpaceX.cameraY < 150 Then
                SpaceX.cameraY += 5
            End If
        ElseIf e.KeyCode = Keys.Left Then
            If SpaceX.CameraZ < 300 Then
                SpaceX.CameraZ += 5
            End If
        ElseIf e.KeyCode = Keys.Right Then
            If SpaceX.CameraZ > 100 Then
                SpaceX.CameraZ -= 5
            End If
        End If
    End Sub


    Private Sub Form2_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        'Draw Visible face



    End Sub


    Private Sub T_Tick(sender As Object, e As EventArgs) Handles T.Tick
        Exit Sub
        SpaceX.DrawingPlane.Clear()
        Dim f As Double = 0

        Dim X As Double = R * Math.Cos(Math.PI / 2 + Teta)
        Dim Z As Double = R * Math.Sin(Math.PI / 2 + Teta)
        SpaceX.Add(New Point3D(X, -50, Z + 350))
        SpaceX.Add(New Point3D(X, 50, Z + 350))
        f = Z : Front = 0

        X = R * Math.Cos(Math.PI + Teta)
        Z = R * Math.Sin(Math.PI + Teta)
        SpaceX.Add(New Point3D(X, -50, Z + 350))
        SpaceX.Add(New Point3D(X, 50, Z + 350))
        If Z < f Then
            f = Z : Front = 2
        End If

        X = R * Math.Cos(0 + Teta)
        Z = R * Math.Sin(0 + Teta)
        SpaceX.Add(New Point3D(X, -50, Z + 350))
        SpaceX.Add(New Point3D(X, 50, Z + 350))
        If Z < f Then
            f = Z : Front = 4
        End If

        X = R * Math.Cos(3 * Math.PI / 2 + Teta)
        Z = R * Math.Sin(3 * Math.PI / 2 + Teta)
        SpaceX.Add(New Point3D(X, -50, Z + 350))
        SpaceX.Add(New Point3D(X, 50, Z + 350))
        If Z < f Then
            f = Z : Front = 6
        End If

        Teta += 2 * Math.PI / 360
        If Teta > 2 * Math.PI Then
            Teta = 0
        End If
        Me.Refresh()
    End Sub

    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        'Draw Edges
        e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(0).D3.ToPoint, SpaceX.DrawingPlane(1).D3.ToPoint)
        ' e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(1).D3.ToPoint, SpaceX.DrawingPlane(2).D3.ToPoint)
        ' e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(2).D3.ToPoint, SpaceX.DrawingPlane(3).D3.ToPoint)
        ' e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(3).D3.ToPoint, SpaceX.DrawingPlane(4).D3.ToPoint)
        '  e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(4).D3.ToPoint, SpaceX.DrawingPlane(5).D3.ToPoint)
        '  e.Graphics.DrawLine(Pens.Black, SpaceX.DrawingPlane(5).D3.ToPoint, SpaceX.DrawingPlane(6).D3.ToPoint)
    End Sub
End Class





Class Space : Inherits List(Of Point3D)
    Public cameraY As Double = 200
    Public CameraZ As Double = 200
    Public DrawingPlane As New List(Of PointSystem)

    Public Shadows Sub Add(Pt As Point3D)
        Dim dZ As Double = CameraZ / (Pt.Z)
        Dim X As Double = Pt.X * dZ
        Dim Y As Double = (Pt.Y + cameraY) * dZ
        DrawingPlane.Add(New PointSystem(Pt, New Point2d(X + 300, Y + 200)))
    End Sub
End Class




Structure Point3D
    Dim X As Double
    Dim Y As Double
    Dim Z As Double
    Sub New(x As Double, y As Double, z As Double)
        Me.X = x
        Me.Y = y
        Me.Z = z
    End Sub
    Public Function ToPoint() As Point
        Return New Point(CInt(X), CInt(Y))
    End Function
End Structure




Structure Point2d
    Dim X As Double
    Dim Y As Double
    Sub New(x As Double, y As Double)
        Me.X = x
        Me.Y = y
    End Sub
    Public Function ToPoint() As Point
        Return New Point(CInt(X), CInt(Y))
    End Function
End Structure




Structure PointSystem
    Dim D3 As Point3D
    Dim D2 As Point2d
    Sub New(D3 As Point3D, d2 As Point2d)
        Me.D3 = D3
        Me.D2 = d2
    End Sub
End Structure