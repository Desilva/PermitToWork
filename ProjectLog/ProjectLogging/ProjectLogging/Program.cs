using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            LogItem log = new LogItem();
            log.controllerName = "abcd";
            log.actionName = "efgh";
            log.requestHeader = "abcd";
            log.requestBody = "efgh";
            log.dateTime = DateTime.Now.AddDays(2);
            log.ipAddress = "::1";
            log.rawUrl = "/abcd/efgh/";
            log.fileSemiPath = "d://";
            log.saveLog();
        }
    }
}
