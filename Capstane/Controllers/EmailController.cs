using Capstone.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Capstone.Services;
using System.IO;
using System.Collections.Generic; // Ensure this is included
using PdfSharpCore.Drawing; // Adjust depending on your library usage
using PdfSharpCore.Pdf;
using CapstoneDAL.Models;
using CapstoneDAL.Models.Dtos;

namespace Capstone.Controllers // Corrected namespace
{
    [Route("api")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly TicketService _ticketRepository;
        private readonly IConfiguration _configuration;
        private readonly PdfGenerator _pdfGenerator;

        public EmailController(TicketService ticketRepository, IConfiguration configuration, PdfGenerator pdfGenerator)
        {
            _ticketRepository = ticketRepository;
            _configuration = configuration;
            _pdfGenerator = pdfGenerator; // Initialize PdfGenerator
        }

        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport([FromBody] ReportRequest reportRequest)
        {
            try
            {
                // Use DateTime.TryParse to safely parse the strings to DateTime
                if (!DateTime.TryParse(reportRequest.DateRange.Start, out var startDate))
                {
                    return BadRequest("Invalid start date format.");
                }

                if (!DateTime.TryParse(reportRequest.DateRange.End, out var endDate))
                {
                    return BadRequest("Invalid end date format.");
                }

                List<Ticket> tickets;

                if (reportRequest.UserId.HasValue)
                {
                    tickets = await _ticketRepository.FindTicketsByUserIdAndCreatedAtBetweenAsync(reportRequest.UserId.Value, startDate, endDate);
                }
                else
                {
                    tickets = await _ticketRepository.FindTicketsByCreatedAtBetweenAsync(startDate, endDate);
                }

                // Create DateRange object with string properties
                var dateRange = new DateRange
                {
                    Start = reportRequest.DateRange.Start,
                    End = reportRequest.DateRange.End
                };

                // Generate PDF report using PdfGenerator
                var pdfFilePath = Path.Combine(Path.GetTempPath(), "ticket_report.pdf");
                _pdfGenerator.CreatePdf(pdfFilePath, tickets, dateRange); // Pass the DateRange object

                // Send Email
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("capstone.dayforce@gmail.com"); // Replace with your email
                    mailMessage.Subject = "Your Report";
                    mailMessage.Body = $"Here is your requested report for the date range: {reportRequest.DateRange.Start} to {reportRequest.DateRange.End}";
                    mailMessage.To.Add(string.Join(",", reportRequest.Emails));
                    mailMessage.Attachments.Add(new Attachment(pdfFilePath));

                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("capstone.dayforce@gmail.com", _configuration["EmailSettings:Password"]); // App password
                        smtpClient.EnableSsl = true;

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                // Delete the PDF file after sending
                System.IO.File.Delete(pdfFilePath);

                return Ok("Report sent successfully");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Exception: {smtpEx.Message} - Status Code: {smtpEx.StatusCode}");
                return StatusCode(500, "Email sending failed: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Failed to send report");
            }
        }

        public class ReportRequest
        {
            public List<string> Emails { get; set; }
            public DateRange DateRange { get; set; }
            public long? UserId { get; set; }
        }

      
    }
}
