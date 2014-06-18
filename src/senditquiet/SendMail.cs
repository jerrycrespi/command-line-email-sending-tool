using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;


namespace senditquiet
{
    public class SendMail
    {
       static SendMail instance = null;

       private SendMail()
       {

       }

        internal static SendMail getInstance()
        {
            return instance != null ? instance : instance = new SendMail();
        }

        SendMailConfiguration conf;

        internal void setConfiguration(SendMailConfiguration conf)
        {
            this.conf = conf;
        }

        public void sendMail(string subject, string body, string[] attachmentsFileNames)
        {
            try
            {
                SmtpClient client = new SmtpClient(this.conf.Host, this.conf.Port);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(this.conf.UserName, this.conf.Pwd);
                client.EnableSsl = this.conf.SslEnabled;
                MailMessage msg = new MailMessage();

                msg.SubjectEncoding = Encoding.UTF8;
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                msg.From = new MailAddress(this.conf.SenderMail, this.conf.SenderMail,Encoding.UTF8);
                

                string[] receptions = this.conf.Recipient.Split(',', ';', ' ');
                foreach (string s in receptions)
                {
                    msg.To.Add(new MailAddress(s,s,Encoding.UTF8));    
                }
        
                msg.Subject = subject;
                msg.Body = body;

                for (int i = 0; i < attachmentsFileNames.Length; i++)
                {
                    if (!string.IsNullOrEmpty(attachmentsFileNames[i]))
                    {
                        Attachment a = new Attachment(attachmentsFileNames[i]);
                        msg.Attachments.Add(a);
                    }
                }

                Console.WriteLine("Sending mail to :" + this.conf.Recipient);
                client.Timeout = 60000 * 30;
                client.Send(msg);
                Console.WriteLine("done.");
                System.Environment.ExitCode =(int) ExitCode.Success;
            }
            catch (Exception exp)
            {
                Exception expinner = exp;
                while (expinner.InnerException != null)
                    expinner = expinner.InnerException;

                throw exp;
            }
        }


        internal SendMailConfiguration getConfiguration()
        {
            return this.conf;
        }
    }
}
