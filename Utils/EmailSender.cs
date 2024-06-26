using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web;

namespace Assignment.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "";

        public async Task Send(String toEmail, String subject, String contents, String attachmentPath, String attachmentExtension)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("abc@gmail.com", "FIT5032 Example Email User");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";



            string[] UserEmailList = toEmail.Split(',');

            List<EmailAddress> toEmailList = new List<EmailAddress>();

            foreach (string userEmail in UserEmailList)
            {
                var to = new EmailAddress(userEmail, "");
                toEmailList.Add(to);
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmailList, subject, plainTextContent, htmlContent);

            if (!attachmentPath.Equals("") && !attachmentExtension.Equals(""))
            {
                var bytes = File.ReadAllBytes(attachmentPath);
                var attachmentFile = Convert.ToBase64String(bytes);

                switch (attachmentExtension)
                {
                    case ".png":
                        msg.AddAttachment("attachment.png", attachmentFile);
                        break;
                    case ".jpg":
                        msg.AddAttachment("attachment.jpg", attachmentFile);
                        break;
                    case ".txt":
                        msg.AddAttachment("attachment.txt", attachmentFile);
                        break;
                    case ".pdf":
                        msg.AddAttachment("attachment.pdf", attachmentFile);
                        break;
                    case ".doc":
                        msg.AddAttachment("attachment.doc", attachmentFile);
                        break;
                    case ".docx":
                        msg.AddAttachment("attachment.docx", attachmentFile);
                        break;
                } 
            }
            var response = await client.SendEmailAsync(msg);
        }
    }
}


