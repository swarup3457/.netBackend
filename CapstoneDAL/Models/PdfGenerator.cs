using Capstone.Models.Entities;
using CapstoneDAL.Models.Dtos;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PdfSharpCore.Drawing; // Ensure you're consistently using PdfSharpCore
using PdfSharpCore.Pdf;
using System.IO;
using System.Linq;

namespace CapstoneDAL.Models
{
    public class PdfGenerator
    {
        private const int TicketsPerPage = 15; // Adjust this value for your layout
        private const int Margin = 40; // Margin for the PDF content
        private const int TitleHeight = 40; // Height for title
        private const int DateRangeHeight = 30; // Height for date range
        private const int TicketHeight = 30; // Height for each ticket
        private const int ChartHeight = 300; // Height for each chart
        private const int TableHeaderHeight = 30; // Height for table header

        public void CreatePdf(string filePath, List<Ticket> tickets, DateRange dateRange)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Ticket Report";

            // Handle pagination for tickets
            for (int pageIndex = 0; pageIndex < (tickets.Count + TicketsPerPage - 1) / TicketsPerPage; pageIndex++)
            {
                CreatePage(document, tickets, dateRange, pageIndex);
            }

            // Save the document to the specified file path
            document.Save(filePath);
        }

        private void CreatePage(PdfDocument document, List<Ticket> tickets, DateRange dateRange, int pageIndex)
        {
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Define fonts
            XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
            XFont subtitleFont = new XFont("Verdana", 16);
            XFont textFont = new XFont("Verdana", 12);
            XFont footerFont = new XFont("Verdana", 10);

            // Draw the title
            gfx.DrawString("Ticket Report", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, TitleHeight), XStringFormats.TopCenter);

            // Draw date range
            gfx.DrawString($"Date Range: {dateRange.Start} to {dateRange.End}", subtitleFont, XBrushes.Black, new XRect(0, TitleHeight, page.Width, DateRangeHeight), XStringFormats.TopCenter);

            // Draw total ticket count
            gfx.DrawString($"Total Tickets: {tickets.Count}", textFont, XBrushes.Black, new XRect(0, TitleHeight + DateRangeHeight, page.Width, DateRangeHeight), XStringFormats.TopCenter);

            // Draw the ticket table
            int startTicketIndex = pageIndex * TicketsPerPage;
            int ticketsToDisplay = Math.Min(TicketsPerPage, tickets.Count - startTicketIndex);

            int yPoint = TitleHeight + DateRangeHeight + 2 * DateRangeHeight; // Start below title and date range
            DrawTicketTable(gfx, tickets.Skip(startTicketIndex).Take(ticketsToDisplay).ToList(), ref yPoint, page);

            // Draw charts only if there is space left
            if (yPoint + ChartHeight + Margin < page.Height)
            {
                yPoint += Margin; // Add margin before the chart
                AddPieChart(gfx, tickets, page, ref yPoint);
                yPoint += ChartHeight + Margin; // Adjust for the height of the pie chart and some margin
            }

            // Draw bar chart on every page
            if (yPoint + ChartHeight + Margin < page.Height)
            {
                yPoint += Margin; // Add margin before the chart
                AddBarChart(gfx, tickets, page, ref yPoint);
            }
        }

        private void DrawTicketTable(XGraphics gfx, List<Ticket> tickets, ref int yPoint, PdfPage page)
        {
            // Draw table header
            gfx.DrawRectangle(XBrushes.LightGray, Margin, yPoint, page.Width - 2 * Margin, TableHeaderHeight);
            gfx.DrawString("ID", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black, new XRect(Margin + 10, yPoint + 5, 50, TableHeaderHeight), XStringFormats.TopLeft);
            gfx.DrawString("Subject", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black, new XRect(Margin + 60, yPoint + 5, 200, TableHeaderHeight), XStringFormats.TopLeft);
            gfx.DrawString("Priority", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black, new XRect(Margin + 270, yPoint + 5, 100, TableHeaderHeight), XStringFormats.TopLeft);
            gfx.DrawString("Status", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black, new XRect(Margin + 370, yPoint + 5, 100, TableHeaderHeight), XStringFormats.TopLeft);
            yPoint += TableHeaderHeight;

            // Draw each ticket in the table
            foreach (var ticket in tickets)
            {
                gfx.DrawString(ticket.Id.ToString(), new XFont("Verdana", 12), XBrushes.Black, new XRect(Margin + 10, yPoint + 5, 50, TicketHeight), XStringFormats.TopLeft);
                gfx.DrawString(ticket.Subject, new XFont("Verdana", 12), XBrushes.Black, new XRect(Margin + 60, yPoint + 5, 200, TicketHeight), XStringFormats.TopLeft);
                gfx.DrawString(ticket.Priority, new XFont("Verdana", 12), XBrushes.Black, new XRect(Margin + 270, yPoint + 5, 100, TicketHeight), XStringFormats.TopLeft);
                gfx.DrawString(ticket.Status, new XFont("Verdana", 12), XBrushes.Black, new XRect(Margin + 370, yPoint + 5, 100, TicketHeight), XStringFormats.TopLeft);
                yPoint += TicketHeight; // Move down for the next ticket
            }

            yPoint += Margin; // Add some margin after the table
        }

        private void AddPieChart(XGraphics gfx, List<Ticket> tickets, PdfPage page, ref int yPoint)
        {
            var pieModel = CreateTicketStatusPieChart(tickets);
            var piePngPath = Path.Combine(Path.GetTempPath(), "pie_chart.png");
            using (var stream = File.Create(piePngPath))
            {
                var exporter = new OxyPlot.SkiaSharp.PngExporter { Width = 600, Height = ChartHeight };
                exporter.Export(pieModel, stream);
            }

            // Draw the pie chart image in the PDF
            XImage pieChartImage = XImage.FromFile(piePngPath);
            gfx.DrawImage(pieChartImage, Margin, yPoint, page.Width - 2 * Margin, ChartHeight); // Use full width

            // Clean up temporary images
            File.Delete(piePngPath);
        }

        private void AddBarChart(XGraphics gfx, List<Ticket> tickets, PdfPage page, ref int yPoint)
        {
            var barModel = CreateTicketStatusPlot(tickets);
            var barPngPath = Path.Combine(Path.GetTempPath(), "bar_chart.png");
            using (var stream = File.Create(barPngPath))
            {
                var exporter = new OxyPlot.SkiaSharp.PngExporter { Width = 600, Height = ChartHeight };
                exporter.Export(barModel, stream);
            }

            // Draw the bar chart image in the PDF
            XImage barChartImage = XImage.FromFile(barPngPath);
            gfx.DrawImage(barChartImage, Margin, yPoint, page.Width - 2 * Margin, ChartHeight); // Use full width

            // Clean up temporary images
            File.Delete(barPngPath);
        }

        private PlotModel CreateTicketStatusPlot(List<Ticket> tickets)
        {
            var model = new PlotModel { Title = "Ticket Status Distribution" };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 };

            var statusCounts = tickets.GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() }).ToList();
            var barSeries = new BarSeries { Title = "Tickets", StrokeColor = OxyColors.Black, FillColor = OxyColors.SkyBlue };

            foreach (var statusCount in statusCounts)
            {
                categoryAxis.Labels.Add(statusCount.Status);
                barSeries.Items.Add(new BarItem { Value = statusCount.Count });
            }

            model.Series.Add(barSeries);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }

        private PlotModel CreateTicketStatusPieChart(List<Ticket> tickets)
        {
            var model = new PlotModel { Title = "Ticket Status Distribution" };
            var pieSeries = new PieSeries { StrokeThickness = 1, InsideLabelPosition = 0.8 };

            var statusCounts = tickets.GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() }).ToList();

            foreach (var statusCount in statusCounts)
            {
                pieSeries.Slices.Add(new PieSlice(statusCount.Status, statusCount.Count) { IsExploded = false });
            }

            model.Series.Add(pieSeries);
            return model;
        }
    }
}
