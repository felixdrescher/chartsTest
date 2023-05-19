Imports MaterialSkin
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Data.SqlClient
Imports ScottPlot

Public Class Form1

    Private dt_plots As New DataTable()
    Private selectedButton As Button = Button1


    Public Sub New()

        InitializeComponent()

        Dim skinManager As MaterialSkinManager = MaterialSkinManager.Instance
        skinManager.Theme = MaterialSkinManager.Themes.LIGHT
        skinManager.ColorScheme = New ColorScheme(Primary.Blue800, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.WHITE)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TabControl1.Appearance = TabAppearance.FlatButtons
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.SizeMode = TabSizeMode.Fixed

        Button1.PerformClick()

    End Sub

    Private Sub barplot()

        Dim dateneingaenge() As Double = {4, 5, 8}
        Dim statistiken() As String = {"std", "prf", "per"}

        With FormsPlot1.Plot

            Dim plot = .AddBar(dateneingaenge, SystemColors.GradientInactiveCaption)
            plot.ShowValuesAboveBars = MaterialCheckbox3.Checked

            .XLabel("laufende Erhebungen")
            .YLabel("Dateneingänge")
            .Grid(enable:=MaterialCheckbox2.Checked)
            .SetAxisLimits(yMin:=0)
            .XTicks(positions:={0, 1, 2}, labels:=statistiken)
            .YAxis.Ticks(False)

        End With

        For Each ctrl As Windows.Forms.Control In Panel3.Controls

            If TypeOf ctrl Is FormsPlot Then

                Dim formsPlotCtrl As FormsPlot = DirectCast(ctrl, FormsPlot)
                formsPlotCtrl.Configuration.ScrollWheelZoom = False

            End If

        Next

        FormsPlot1.Refresh()

    End Sub

    Private Sub stackedBarplot()

        Dim geliefert() As Double = {3, 0, 10}
        Dim gesamt() As Double = {9, 10, 14}
        Dim statistiken As String() = {"std", "prf", "per"}

        Dim gesamt2(gesamt.Length - 1) As Double

        For i As Integer = 0 To gesamt.Length - 1

            gesamt2(i) = geliefert(i) + gesamt(i)

        Next

        With FormsPlot2.Plot

            Dim bar1 = .AddBar(gesamt2, Color.Tomato)
            bar1.ShowValuesAboveBars = True

            Dim bar2 = .AddBar(geliefert, Color.Green)
            bar2.ShowValuesAboveBars = True

            .SetAxisLimits(yMin:=0)
            .YLabel("Anteil geliefert an Gesamtzahl Melder")
            .XLabel("laufende Erhebungen")
            .XTicks(positions:={0, 1, 2}, labels:=statistiken)
            .Grid(enable:=False)
            .YAxis.Ticks(MaterialCheckbox1.Checked)

        End With

        FormsPlot2.Refresh()

    End Sub

    Private Sub pieChart()

        Dim anzahlen() As Double = {5, 20, 13, 9, 22}
        Dim fehler() As String = {"fehler1", "fehler2", "fehler3", "fehler4", "fehler5"}

        Dim pie = FormsPlot3.Plot.AddPie(anzahlen)
        pie.ShowLabels = True
        pie.ShowPercentages = MaterialCheckbox5.Checked
        pie.ShowValues = MaterialCheckbox4.Checked
        pie.SliceLabels = fehler

        FormsPlot3.Refresh()

    End Sub

    Private Sub regression()

        ' Create some linear but noisy data
        Dim ys As Double() = DataGen.NoisyLinear(Nothing, pointCount:=100, noise:=30)
        Dim xs As Double() = DataGen.Consecutive(ys.Length)
        Dim x1 As Double = xs(0)
        Dim x2 As Double = xs(xs.Length - 1)

        ' Use the linear regression fitter to fit these data
        Dim model As New Statistics.LinearRegressionLine(xs, ys)

        ' Plot the original data and add the regression line
        With FormsPlot4.Plot

            .Title("Lineare Regression" & vbCrLf &
                   $"Y = {model.slope:0.0000}x + {model.offset:0.0} " &
                   $"(R² = {model.rSquared:0.0000})")

            .AddScatter(xs, ys, lineWidth:=0)
            .AddLine(model.slope, model.offset, (x1, x2), lineWidth:=2)

        End With

        FormsPlot4.Refresh()

    End Sub

    Private Sub button_Paint(sender As Object, e As PaintEventArgs) Handles Button1.Paint, Button2.Paint,
        Button3.Paint, Button4.Paint

        Dim button As Button = CType(sender, Button)
        Dim borderThickness As Integer = 3
        Dim borderColor As Color = Color.RoyalBlue

        If button Is selectedButton Then

            ' Calculate the bottom border rectangle
            Dim borderRect As New Rectangle(0, button.Height - borderThickness + 1, button.Width, borderThickness)

            ' Draw the bottom border
            Using borderPen As New Pen(borderColor, borderThickness)

                e.Graphics.DrawLine(borderPen, borderRect.Left, borderRect.Top, borderRect.Right, borderRect.Top)

            End Using

        End If

    End Sub

    Private Sub RefreshButtons()

        Button1.Refresh()
        Button2.Refresh()
        Button3.Refresh()
        Button4.Refresh()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        TabControl1.SelectedTab = TabControl1.TabPages("TabPage1")

        Dim button As Button = CType(sender, Button)
        selectedButton = button
        RefreshButtons()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        TabControl1.SelectedTab = TabControl1.TabPages("TabPage2")

        Dim button As Button = CType(sender, Button)
        selectedButton = button
        RefreshButtons()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        TabControl1.SelectedTab = TabControl1.TabPages("TabPage3")

        Dim button As Button = CType(sender, Button)
        selectedButton = button
        RefreshButtons()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        TabControl1.SelectedTab = TabControl1.TabPages("TabPage4")

        ' plots initialisieren
        barplot()
        stackedBarplot()
        pieChart()
        regression()

        Dim button As Button = CType(sender, Button)
        selectedButton = button
        RefreshButtons()

    End Sub

    Private Sub dashboardPageChanged(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click,
        Button3.Click, Button4.Click

        Select Case TabControl1.SelectedTab.Name

            Case "TabPage1"
                Button1.BackColor = SystemColors.GradientInactiveCaption
                Button1.ImageKey = "iconJobsFilled.png"

                Button2.BackColor = Color.WhiteSmoke
                Button2.ImageKey = "iconJobsStatus.png"

                Button3.BackColor = Color.WhiteSmoke
                Button3.ImageKey = "iconNews.png"

                Button4.BackColor = Color.WhiteSmoke
                Button4.ImageKey = "iconCharts.png"

            Case "TabPage2"
                Button2.BackColor = SystemColors.GradientInactiveCaption
                Button2.ImageKey = "iconJobsStatusFilled.png"

                Button1.BackColor = Color.WhiteSmoke
                Button1.ImageKey = "iconJobs.png"

                Button3.BackColor = Color.WhiteSmoke
                Button3.ImageKey = "iconNews.png"

                Button4.BackColor = Color.WhiteSmoke
                Button4.ImageKey = "iconCharts.png"

            Case "TabPage3"
                Button3.BackColor = SystemColors.GradientInactiveCaption
                Button3.ImageKey = "iconNewsFilled.png"

                Button1.BackColor = Color.WhiteSmoke
                Button1.ImageKey = "iconJobs.png"

                Button2.BackColor = Color.WhiteSmoke
                Button2.ImageKey = "iconJobsStatus.png"

                Button4.BackColor = Color.WhiteSmoke
                Button4.ImageKey = "iconCharts.png"

            Case "TabPage4"
                Button4.BackColor = SystemColors.GradientInactiveCaption
                Button4.ImageKey = "iconChartsFilled.png"

                Button1.BackColor = Color.WhiteSmoke
                Button1.ImageKey = "iconJobs.png"

                Button2.BackColor = Color.WhiteSmoke
                Button2.ImageKey = "iconJobsStatus.png"

                Button3.BackColor = Color.WhiteSmoke
                Button3.ImageKey = "iconNews.png"

        End Select

    End Sub

    Private Sub MaterialCheckbox1_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckbox1.CheckedChanged

        stackedBarplot()

    End Sub

    Private Sub MaterialCheckbox2_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckbox2.CheckedChanged,
        MaterialCheckbox3.CheckedChanged

        barplot()

    End Sub

    Private Sub MaterialCheckbox4_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckbox4.CheckedChanged,
            MaterialCheckbox5.CheckedChanged, MaterialCheckbox6.CheckedChanged

        pieChart()

    End Sub

End Class
