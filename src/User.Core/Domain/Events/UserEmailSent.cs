using BAOZ.Common;
using BAOZ.Common.Models.Dtos;
using EventFlow.EventStores;
using User.Core.Domain.Aggregates;

namespace User.Core.Domain.Events
{
    [EventVersion("UserEmailSent", 1)]
    public class UserEmailSent : BaseEvent<UserAggregate>
    {
        public UserEmailSent(EmailAndName to, string plainText, string htmlBody, string subject)
        {
            To = to;
            PlainText = plainText;
            HtmlBody = htmlBody;
            Subject = subject;
        }

        public EmailAndName From { get; set; } = new EmailAndName("batuhan@batuhan.com", "batuhan Kara");
        public EmailAndName To { get; set; }
        public string PlainText { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
    }
}
