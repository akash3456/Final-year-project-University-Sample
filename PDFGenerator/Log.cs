using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO;
namespace PDFGenerator
{
    class Log
    {
        public void CreateLogFile(Exception exception)
        {
            var ExceptionContents = new List<Exception>();
            ExceptionContents.Add(exception);
            var query = from a in ExceptionContents
                        where a.Message.Equals(exception.Message)
                        select a.Message;
            foreach (var temp in query)
            {
                var getFileDirectory = String.Format(@"{0}\{1}", System.IO.Directory.GetCurrentDirectory(), "ErrorLog.log");
                if (!File.Exists("ErrorLog.log"))
                {
                    File.OpenWrite(getFileDirectory);
                    File.WriteAllLines(String.Format("{0}", getFileDirectory), query);
                }
                else if (File.Exists(getFileDirectory))
                {
                    File.Open(String.Format(@"{0}", getFileDirectory), FileMode.Open, FileAccess.Write);
                    File.WriteAllLines(String.Format("{0", getFileDirectory), query);
                }
            }
        }
    }
}
