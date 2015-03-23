using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Mail;
using Scryber.Components;

namespace PDFGenerator
{
    public class SendEmail
    {
        String Subject;
        String EmailBody;
        Outlook.Recipients mailrecipents = null;
        Outlook.MailItem mail = null;
        Outlook.Recipient rec = null;
        Outlook.Application app = new Outlook.Application();
        public SendEmail(String Subject, String EmailBody)
        {
            this.Subject = Subject;
            this.EmailBody = EmailBody;
        }
        public void CreateMailItem(String email, String path)
        {
            mail = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            mail.Subject = Subject;
            mail.Body = EmailBody;
            Outlook.Attachment attach = mail.Attachments.Add(path);
            mailrecipents = mail.Recipients;
            rec = mailrecipents.Add(email);
            rec.Resolve();
            mail.Send();
        }
        public void SendEmailtoContacts(String path, String email)
        {
            CreateMailItem(email, path); 
        }
    }
}
