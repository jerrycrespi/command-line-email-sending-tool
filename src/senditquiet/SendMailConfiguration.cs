using System;
using System.Collections.Generic;
using System.Text;

namespace senditquiet
{
    [Serializable]
   public class SendMailConfiguration
   {
        string host;
        int port;
        string pwd;
        bool sslEnabled;
        string userName;
        string senderMail;
        string recipient;
        private string repotTimes;
        private string subject;
        private string format;

        public SendMailConfiguration()
        {
            timeSpansReportTimes = new List<TimeSpan>();
        }

        public string Recipient
        {
            get { return recipient; }
            set { recipient = value; }
        }

        public string SenderMail
        {
            get { return senderMail; }
            set { senderMail = value; }
        }

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public bool SslEnabled
        {
            get { return sslEnabled; }
            set { sslEnabled = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string RepotTimes
        {
            get { return repotTimes; }
            set
            {
                repotTimes = value;

                this.timeSpansReportTimes.Clear();

                string s = repotTimes;

                string[] strTimes = s.Split(new char[] { ',', ';',' ' });

                foreach (var time in strTimes)
                {
                    TimeSpan spn;
                    if (TimeSpan.TryParse(time, out spn))
                        timeSpansReportTimes.Add(spn);
                }

                this.repotTimes = "";

                foreach (TimeSpan list in this.timeSpansReportTimes)
                {
                    this.repotTimes += list.Hours.ToString("00") + ":" + list.Minutes.ToString("00") + ",";
                }
                this.repotTimes = this.repotTimes.Trim(',');

            }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Format
        {
            get {
                if (string.IsNullOrEmpty(format))
                    format = "PDF";
                return format; 
            }
            set { format = value; }
        }

        private List<TimeSpan> timeSpansReportTimes;

        public List<TimeSpan> getMailingTimes()
        {
            return this.timeSpansReportTimes;
        }
   }
}
