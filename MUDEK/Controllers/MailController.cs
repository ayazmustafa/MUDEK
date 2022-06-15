using Microsoft.AspNetCore.Mvc;
using Mudek.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Mudek.Controllers
{
    [Authorize]
    public class MailController : Controller
    {

        private readonly ApplicationDbContext _context;
        public MailController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendGraduateMail(Survey survey)
        {
            var id = Guid.NewGuid();
            MimeMessage message = new MimeMessage();

            // sender
            message.From.Add(new MailboxAddress("", ""));

            // reciever
            message.To.Add(MailboxAddress.Parse(survey.Mail));

            message.Subject = "Anket";

            message.Body = new TextPart("plain")
            {
                Text = @$"Anket için şu sayfaya gidin. => https://localhost:44372/Survey/GraduateSurvey/{id}"
            };

            SmtpClient client = new();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate();
                client.Send(message);
                survey.Id = id;
                survey.EmailCreationDate = DateTime.Now;
                survey.SurveyType = "Graduate";
                _context.Add(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            return Redirect("/Mail/Index");
        }


        public async Task<IActionResult> SendEmployerMail(Survey survey)
        {
            var id = Guid.NewGuid();
            MimeMessage message = new MimeMessage();

            // sender
            message.From.Add(new MailboxAddress("", ""));

            // reciever
            message.To.Add(MailboxAddress.Parse(survey.Mail));

            message.Subject = "Anket";

            message.Body = new TextPart("plain")
            {
                Text = @$"Anket için şu sayfaya gidin. => https://localhost:44372/Survey/EmployerSurvey/{id}"
            };

            SmtpClient client = new();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("", "");
                client.Send(message);
                survey.Id = id;
                survey.EmailCreationDate = DateTime.Now;
                survey.SurveyType = "Employer";
                _context.Add(survey);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            return Redirect("/Mail/Index");
        }
    }
}
