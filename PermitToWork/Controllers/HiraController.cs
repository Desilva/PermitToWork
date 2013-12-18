using PermitToWork.Models.Hira;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class HiraController : Controller
    {
        //
        // GET: /Hira/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, int? id_ptw)
        {
            // The Name of the Upload component is "attachments" 
            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var dummyPath = Path.Combine("\\Upload\\Hira", fileName);
                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var physicalPath = Path.Combine(Server.MapPath("~/Upload/Hira"), fileName);

                // save file
                file.SaveAs(physicalPath);
                if (id_ptw == 0 || id_ptw == null) {
                    HiraEntity hira = new HiraEntity(dummyPath, fileName);
                    hira.addHiraDocument();
                } else {
                    HiraEntity hira = new HiraEntity(dummyPath, fileName, id_ptw);
                    hira.addHiraDocument();
                }
                
                

            }

            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/Upload/Hira"), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    //remove file
                    System.IO.File.Delete(physicalPath);
                }

                HiraEntity hira = new HiraEntity(fileName);
                hira.deleteHiraDocument();
            }
            // Return an empty string to signify success
            return Content("");
        }

    }
}
