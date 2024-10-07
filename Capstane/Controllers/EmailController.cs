using Capstone.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Capstone.Services;
using CsvHelper;

namespace Capstane.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly TicketService _ticketRepository;
        private readonly IConfiguration _configuration;

        public EmailController(TicketService ticketRepository, IConfiguration configuration)
        {
            _ticketRepository = ticketRepository;
            _configuration = configuration;
        }

        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport([FromBody] ReportRequest reportRequest)
        {
            try
            {
                var startDate = DateTime.Parse(reportRequest.DateRange.Start);
                var endDate = DateTime.Parse(reportRequest.DateRange.End);

                List<Ticket> tickets;

                if (reportRequest.UserId.HasValue)
                {
                    tickets = await _ticketRepository.FindTicketsByUserIdAndCreatedAtBetweenAsync(reportRequest.UserId.Value, startDate, endDate);
                }
                else
                {
                    tickets = await _ticketRepository.FindTicketsByCreatedAtBetweenAsync(startDate, endDate);
                }

                // Generate CSV file
                var csvFilePath = Path.Combine(Path.GetTempPath(), "ticket_report.csv");
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteField("Ticket ID");
                    csv.WriteField("Subject");
                    csv.WriteField("Priority");
                    csv.WriteField("Status");
                    csv.WriteField("Assigned Agent");
                    csv.WriteField("Created At");
                    csv.WriteField("Updated At");
                    csv.NextRecord();

                    foreach (var ticket in tickets)
                    {
                        csv.WriteField(ticket.Id);
                        csv.WriteField(ticket.Subject);
                        csv.WriteField(ticket.Priority);
                        csv.WriteField(ticket.Status);
                        csv.WriteField(ticket.AssignedAgent ?? "Unassigned");
                        csv.WriteField(ticket.CreatedAt);
                        csv.WriteField(ticket.UpdatedAt);
                        csv.NextRecord();
                    }
                }

                // Send Email
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("capstone.dayforce@gmail.com"); // Replace with your email
                    mailMessage.Subject = "Your Report";
                    mailMessage.Body = $"Here is your requested report for the date range: {reportRequest.DateRange.Start} to {reportRequest.DateRange.End}";
                    mailMessage.To.Add(string.Join(",", reportRequest.Emails));
                    mailMessage.Attachments.Add(new Attachment(csvFilePath));

                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("capstone.dayforce@gmail.com", _configuration["EmailSettings:Password"]); // App password
                        smtpClient.EnableSsl = true;

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                // Delete the CSV file after sending
                System.IO.File.Delete(csvFilePath);

                return Ok("Report sent successfully");
            }
            catch (SmtpException smtpEx)
            {
                // Log more details for debugging
                Console.WriteLine($"SMTP Exception: {smtpEx.Message} - Status Code: {smtpEx.StatusCode}");
                return StatusCode(500, "Email sending failed: " + smtpEx.Message);
            }
            catch (Exception ex)
            {
                // Log the full exception details
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

        public class DateRange
        {
            public string Start { get; set; }
            public string End { get; set; }
        }
    }
}
