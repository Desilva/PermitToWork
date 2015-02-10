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
            return PartialView();
        }

        public JsonResult BindingFile(string fileId)
        {
            fileId = fileId != null ? fileId.Split('#')[1] : null;
            string path = fileId != null ? Server.MapPath("~/" + fileId + "/") : Server.MapPath("~/Upload/Hira/");

            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files
            DirectoryInfo[] Directories = d.GetDirectories();

            List<HiraTreeViewModel> hiraTreeView = new List<HiraTreeViewModel>();
            Uri folder = new Uri(Server.MapPath("~"));

            foreach (DirectoryInfo dir in Directories)
            {
                hiraTreeView.Add(new HiraTreeViewModel { fileId = "FOLDER#" + folder.MakeRelativeUri(new Uri(dir.FullName)).ToString(), name = dir.Name, hasChildren = true, spriteCssClass = "folder" });
            }

            foreach (FileInfo file in Files)
            {
                hiraTreeView.Add(new HiraTreeViewModel { fileId = "PDF#" + folder.MakeRelativeUri(new Uri(file.FullName)).ToString(), name = file.Name, hasChildren = false, spriteCssClass = "pdf" });
            }

            return Json(hiraTreeView);
        }

        public JsonResult Save(IEnumerable<HttpPostedFileBase> files, int? id_ptw)
        {
            List<string> result = new List<string>();
            // The Name of the Upload component is "attachments" 
            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                string folder = fileName.Split('-')[0];
                var dummyPath = Path.Combine("Upload\\Hira\\" + folder, fileName);
                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                bool isExists = System.IO.Directory.Exists(Server.MapPath("~/Upload/Hira/" + folder));

                if (!isExists)
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Upload/Hira/" + folder));

                var physicalPath = Path.Combine(Server.MapPath("~/Upload/Hira/" + folder), fileName);

                // save file
                file.SaveAs(physicalPath);
                if (id_ptw == 0 || id_ptw == null) {
                    HiraEntity hira = new HiraEntity(dummyPath, fileName);
                    hira.addHiraDocument();
                } else {
                    HiraEntity hira = new HiraEntity(dummyPath, fileName, id_ptw);
                    hira.addHiraDocument();
                }

                result.Add(dummyPath.Replace('\\','/'));
            }

            // Return an empty string to signify success
            return Json(result);
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                string name = HttpUtility.UrlDecode(fullName);
                var fileName = Path.GetFileName(name);
                string path = Server.MapPath("~/" + name);
                var physicalPath = path;

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    //remove file
                    System.IO.File.Delete(physicalPath);
                }

                //HiraEntity hira = new HiraEntity(fileName);
                //hira.deleteHiraDocument();
            }
            // Return an empty string to signify success
            return Content("");
        }

    }
}
