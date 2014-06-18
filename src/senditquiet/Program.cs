using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;


namespace senditquiet
{
    class Program
    {
        private static string DEFAULT_MESSAGE_BODY = "A mail, sent by using senditquite";
        private static string[] arguments;

        static int Main(string[] args)
        {

            System.Environment.ExitCode = System.Environment.ExitCode = (int)ExitCode.Fail; ;
            
            arguments = args;
            TextlogOutput.enabled = !(getOptionalParam("-logfile", "").Equals(""));
            TextlogOutput.logFile = getOptionalParam("-logfile", "senditquiet.log");

            SendMailConfiguration conf = new SendMailConfiguration();
            conf.Host = getRequiredParam("-s");
            conf.Port = int.Parse(getOptionalParam("-port","25"));
            conf.Pwd = getRequiredParam("-p");
            conf.Recipient = getRequiredParam("-t");
            conf.SenderMail = getRequiredParam("-f");
            conf.SslEnabled = getOptionalParam("-protocol", "normal").Equals("ssl");
            conf.UserName = getRequiredParam("-u");
            conf.Subject = getOptionalParam("-subject", "A mail, sent by using senditquite");
            SendMail.getInstance().setConfiguration(conf);
            try
            {
                string body = getMessageBod();
                
                SendMail.getInstance().sendMail(conf.Subject, body, getOptionalParam("-files", "").Split(';'));
                

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
            }

            return System.Environment.ExitCode;

        }

        private static bool isSwitchSpecified(string p)
        {
            for (int i = 0; i < arguments.Length - 1; i++)
            {
                if (p.Equals(arguments[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static string getMessageBod()
        {
            string body = getOptionalParam("-body", DEFAULT_MESSAGE_BODY);
            if (DEFAULT_MESSAGE_BODY.Equals(body))
            {
                string bodyFileName = getOptionalParam("-bodyfile", "");
                if (!"".Equals(bodyFileName))
                {
                    if (File.Exists(bodyFileName))
                    {
                        return File.ReadAllText(bodyFileName, Encoding.UTF8);

                    } else
                    {
                        Console.WriteLine("ERROR: File not found, missing quotes? " + bodyFileName);
                    }
                }
            }
            else
            {
                body = body.Replace("\\n", "<br>");
            }

            return body;
        }

        private static string getRequiredParam(string p)
        {
            String s = getOptionalParam(p,"");
            if (string.IsNullOrEmpty(s))
            {
                s = readFromRegistry(p, s);
            }

            if (string.IsNullOrEmpty(s))
            {
                Console.WriteLine("ERROR: Missing required param : " + p);
                printErrorAndExit();
                return "";
            }
            return s;
        }

        private static string readFromRegistry(string key, string defaultValue)
        {
            object retval = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Senditquiet", key.Trim('-'), defaultValue);
            return retval == null ? defaultValue:retval.ToString();
        }

        private static string getOptionalParam(string p, string defaultVal)
        {
            for (int i = 0; i < arguments.Length-1; i++)
            {
                if (p.Equals(arguments[i],StringComparison.OrdinalIgnoreCase))
                {
                    return arguments[i + 1];
                }
            }

            return readFromRegistry(p, defaultVal);
        }

        private static void printErrorAndExit()
        {
            Console.WriteLine("---------------------");
            String msg =
                @"PARAMETERS:
-s <server>     : SMTP server address (required)
-port <port>    : SMTP server port (Default is 25)
-u <username>   : SMTP user name (reqired)
-p <password>   : SMTP password (required)
-f <from>       : Sender mail address (required)
-t <to>         : Comma seperated recipient list (reqired)
-protocol <protocol>  : SMTP protocol possible values are, ssl, normal.
-subject <subject> : subject line, surround with quotes if you want to include spaces
-body <body> : Mail body. Surround with quotes if you want to include spaces
-bodyfile <filename> :file path contains message body, file encoding assumed as utf8
-files <files>      : Attachment files, (comma seperated).
-logfile <filename> : Optionaly you can specify a log file to have detailed trace of whole communication process.
";

            Console.WriteLine(msg);

            Console.WriteLine("Usage example:");
            Console.WriteLine("");
            Console.WriteLine(@"senditquiet.exe -s smtp.myserver.com -u mysmtpusername -p mypassword -f mymailaddress@myserver.com -t firstrecipient@address.com;secondrecipient@address.com -protocol ssl -subject "+
                "\"A mail subject\" -files afile.zip;anotherfile.rar");
            Console.WriteLine("");
            Console.WriteLine("Visit http://commandlinesendmail.blogspot.com/ for more information and examples.");
            Console.WriteLine("");
            
            System.Environment.Exit(System.Environment.ExitCode);
            
        }
        
    }
}
