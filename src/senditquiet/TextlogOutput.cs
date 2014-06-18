using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace senditquiet
{
    public class TextlogOutput : TraceListener
    {
        public static bool enabled = true;

        public static string logFile = "";

        private System.Diagnostics.TextWriterTraceListener tw;

        public override void Flush()
        {
            if (!enabled) return;
            tw.Flush();
        }       

      
        public override void Write(string message)
        {
            if (!enabled) return;
            tw.Write(message);
        }

    
        public override void WriteLine(string message)
        {
            if (!enabled) return;
            tw.WriteLine(message);
        }

        public TextlogOutput()
        {
            tw = new TextWriterTraceListener(logFile);
            System.Diagnostics.Trace.Listeners.Add(this);
        }
       
    }
}
