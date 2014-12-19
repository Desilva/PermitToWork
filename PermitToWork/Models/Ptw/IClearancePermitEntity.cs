using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermitToWork.Models.Ptw
{
    public interface IClearancePermitEntity
    {
        string statusText { get; set; }
        int ids { get; set; }
        bool isUser { get; set; }
        int create();
        int edit();
        int delete();

        void generateNumber(string ptw_no);
    }
}
