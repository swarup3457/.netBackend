using Capstone.Models.Entities;
using Nest;
using PdfSharp.Drawing;
using PdfSharp.Pdf;



namespace CapstoneDAL.Models
{

    public class PdfGenerator
    {
        public void CreatePdf(string filePath, List<Ticket> tickets, DateRange dateRange)
        {
            //// Create a new PDF document
            //PdfDocument document = new PdfDocument();
            //document.Info.Title = "Ticket Report";

            //// Create an empty page
            //PdfPage page = document.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //// Define fonts
            //XFont titleFont = new XFont("Arial", 20, XFontStyle.Bold); // Changed font to Arial
            //XFont textFont = new XFont("Arial", 12, XFontStyle.Regular); // Changed font to Arial

            //// Draw the title
            //gfx.DrawString("Ticket Report", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

            //// Draw date range
            //gfx.DrawString($"Date Range: {dateRange.Start} to {dateRange.End}", textFont, XBrushes.Black, new XRect(0, 50, page.Width, page.Height), XStringFormats.TopCenter);

            //// Draw a simple table
            //int yPoint = 100; // Starting point for the table
            //foreach (var ticket in tickets)
            //{
            //    gfx.DrawString($"ID: {ticket.Id}, Subject: {ticket.Subject}, Priority: {ticket.Priority}, Status: {ticket.Status}", textFont, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            //    yPoint += 20; // Move down for next ticket
            //}

            //// Save the document
            //document.Save(filePath);
        }
    }
}
