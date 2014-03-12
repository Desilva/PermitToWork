using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public int id { get; set; }
    }
}