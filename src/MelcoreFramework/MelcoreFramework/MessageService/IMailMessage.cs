﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelcoreFramework.MessageService
{
    public interface IMailMessage
    {
        #region Public Methods

        Task SendMail(MailName from, List<MailName> to, List<MailName> cc, string title, string body);

        Task SendMail(MailName from, List<MailName> to, string title, string body);

        Task SendMail(MailName from, MailName to, string title, string body);

        Task SendMail(string from, string to, string title, string body);

        #endregion Public Methods
    }
}