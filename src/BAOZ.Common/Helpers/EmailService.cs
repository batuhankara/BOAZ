using BAOZ.Common.Models.Dtos;
using Hangfire;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAOZ.Common.Helpers
{

    public static class EmailService
    {
        public static void Execute(EmailTemplateDto model)
        {
            var jobId = BackgroundJob.Enqueue(
                () => Send(model));

        }
        public static void Send(EmailTemplateDto model)
        {

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(model.From.Email, model.From.Name);
            var subject = model.Subject;
            var to = new EmailAddress(model.To.Email, model.To.Name);
            var plainTextContent = model.PlainText;
            var htmlContent = model.HtmlBody;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg).Wait();
        }

    }
}
