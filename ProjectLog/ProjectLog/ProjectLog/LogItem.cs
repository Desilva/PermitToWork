using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLog
{
    public class LogItem
    {
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string requestHeader { get; set; }
        public string requestBody { get; set; }
        public DateTime dateTime { get; set; }
        public string ipAddress { get; set; }
        public string rawUrl { get; set; }
        public string fileSemiPath { get; set; }

        public string username { get; set; }
        public string user_id { get; set; }

        public string message { get; set; }

        public string generateMessage()
        {
            this.message = string.Format("{0} : {1}({4}) accessing /{2}/{3}", this.dateTime.ToString(), this.username, this.controllerName, this.actionName, this.user_id);
            this.message += "\n" + "Raw Url : " + rawUrl;
            this.message += "\n" + "IP Address : " + ipAddress;
            this.message += "\n" + "Request Header : " + requestHeader;
            this.message += "\n" + "Request Body : " + requestBody;
            this.message += "\n" + "===========================================\n";

            return this.message;
        }

        public bool saveLog()
        {
            generateMessage();
            var date = this.dateTime.ToString("yyyyMMdd");
            if (!File.Exists(this.fileSemiPath + "penting" + date + ".log"))
            {
                File.Create(this.fileSemiPath + "penting" + date + ".log").Close();
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.fileSemiPath + "penting" + date + ".log", true))
            {
                file.WriteLine(message);
            }

            return true;
        }
    }
}
