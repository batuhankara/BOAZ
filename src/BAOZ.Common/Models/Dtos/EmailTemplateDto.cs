﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BAOZ.Common.Models.Dtos
{
    public class EmailTemplateDto : IDto
    {
        public EmailAndName From { get; set; } = new EmailAndName("batuhan@batuhan.com", "batuhan Kara");
        public EmailAndName To { get; set; }
        public string PlainText { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
    }
    public class EmailAndName
    {
        public EmailAndName(string email, string name)
        {
            Email = email;
            Name = name;
        }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
